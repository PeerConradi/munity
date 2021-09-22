using Microsoft.EntityFrameworkCore;
using MUNity.Database.Context;
using MUNity.Database.Models.Conference;
using MUNity.Database.Models.Conference.Roles;
using MUNity.Database.Models.Organization;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUNity.Database.General;
using MUNityBase;

namespace MUNityDatabaseTest.ConferenceTests
{
    [TestFixture]
    public class ConferenceRolesTest
    {
        private MunityContext _context;

        private Organization organization;

        private Project project;

        private Conference conference;

        [OneTimeSetUp]
        public void Setup()
        {
            // Datenbank für den Test erzeugen und falls vorhanden erst einmal leeren und neu erstellen!
            var optionsBuilder = new DbContextOptionsBuilder<MunityContext>();
            optionsBuilder.UseSqlite("Data Source=test_conferenceRoles.db");
            _context = new MunityContext(optionsBuilder.Options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            organization = new Organization()
            {
                OrganizationName = "Test Organization",
                OrganizationShort = "TO"
            };
            project = new Project()
            {
                ProjectName = "Test Project",
                ProjectOrganization = organization,
                ProjectShort = "TP",
            };
            conference = new Conference()
            {
                ConferenceProject = project,
                ConferenceShort = "TC",
                CreationDate = DateTime.Now,
                EndDate = new DateTime(2021, 1, 4),
                StartDate = new DateTime(2021, 1, 1),
                FullName = "Test Conference",
                Name = "Test Conference"
            };
            var committee = new Committee()
            {
                Article = "die",
                CommitteeShort = "GV",
                Conference = conference,
                FullName = "Generalversammlung",
                Name = "Generalversammlung"
            };
            _context.Organizations.Add(organization);
            _context.Projects.Add(project);
            _context.Conferences.Add(conference);
            _context.Committees.Add(committee);
            _context.SaveChanges();
        }

        [Test]
        [Order(0)]
        public void TestEnvironmentIsCreated()
        {
            var orga = _context.Organizations
                .Include(n => n.Projects)
                .ThenInclude(n => n.Conferences)
                .FirstOrDefault();
            Assert.AreEqual(1, orga.Projects.Count);
            Assert.AreEqual(1, orga.Projects[0].Conferences.Count);
        }

        [Test]
        [Order(1)]
        public void TestCreateTeamRoleProjectLeader()
        {
            var leaderAuth = new ConferenceRoleAuth()
            {
                Conference = conference,
                CanEditConferenceSettings = true,
                CanEditParticipations = true,
                CanSeeApplications = true,
                PowerLevel = 1,
                RoleAuthName = "Project-Owner"
            };
            _context.ConferenceRoleAuthorizations.Add(leaderAuth);

            var leaderRoleGroup = new TeamRoleGroup()
            {
                FullName = "die Projektleitung",
                Name = "Projektleitung",
                TeamRoleGroupShort = "PL",
                GroupLevel = 1
            };
            _context.TeamRoleGroups.Add(leaderRoleGroup);

            var leaderRole = new ConferenceTeamRole()
            {
                Conference = conference,
                IconName = "pl",
                RoleFullName = "Projektleiter",
                ConferenceRoleAuth = leaderAuth,
                RoleName = "Projektleiter",
                RoleShort = "PL",
                TeamRoleGroup = leaderRoleGroup,
                TeamRoleLevel = 1,
            };

            _context.TeamRoles.Add(leaderRole);
            _context.SaveChanges();

            var leaderRoleCallback = _context.TeamRoles.FirstOrDefault();
            Assert.NotNull(leaderRoleCallback);
            Assert.AreEqual(1, _context.ConferenceRoleAuthorizations.Count());
            Assert.AreEqual(1, _context.TeamRoles.Count());
            Assert.AreEqual(1, _context.TeamRoleGroups.Count());
            Assert.AreEqual("TeamRole", leaderRoleCallback.RoleType);
        }

        [Test]
        [Order(2)]
        public void TestGenerateFirstLevelTeam()
        {
            var leaderRole = _context.TeamRoles.FirstOrDefault(n => n.Conference == conference &&
            n.TeamRoleLevel == 1);

            Assert.NotNull(leaderRole);

            var firstLevelAuth = new ConferenceRoleAuth()
            {
                CanEditConferenceSettings = false,
                CanEditParticipations = false,
                CanSeeApplications = true,
                Conference = conference,
                PowerLevel = 2,
                RoleAuthName = "Erweiterte Projektleitung",
            };

            _context.ConferenceRoleAuthorizations.Add(firstLevelAuth);

            var firstLevelGroup = new TeamRoleGroup()
            {
                FullName = "Erweiterte Projektleitung",
                GroupLevel = 2,
                Name = "Erweiterte Projektleitung",
                TeamRoleGroupShort = "EPL"
            };

            _context.TeamRoleGroups.Add(firstLevelGroup);

            var teamCoordinatorRole = new ConferenceTeamRole()
            {
                Conference = conference,
                IconName = "un",
                ParentTeamRole = leaderRole,
                ConferenceRoleAuth = firstLevelAuth,
                RoleFullName = "Team- und Materialkoordination",
                RoleName = "Team- und Materialkoordination",
                RoleShort = "TMK",
                TeamRoleGroup = firstLevelGroup,
                TeamRoleLevel = 2
            };

            var financeManager = new ConferenceTeamRole()
            {
                Conference = conference,
                IconName = "un",
                ParentTeamRole = leaderRole,
                ConferenceRoleAuth = firstLevelAuth,
                RoleFullName = "Finanzen",
                RoleName = "Finanzen",
                RoleShort = "CASH",
                TeamRoleGroup = firstLevelGroup,
                TeamRoleLevel = 2
            };

            _context.TeamRoles.Add(teamCoordinatorRole);
            _context.TeamRoles.Add(financeManager);
            _context.SaveChanges();
            Assert.AreEqual(3, _context.TeamRoles.Count(n => n.Conference.ConferenceId == conference.ConferenceId));
        }

        [Test]
        [Order(3)]
        public void TestCreateSecretaryGeneralRole()
        {
            var participantAuth = new ConferenceRoleAuth()
            {
                CanEditConferenceSettings = false,
                CanEditParticipations = false,
                CanSeeApplications = false,
                Conference = conference,
                PowerLevel = 3,
                RoleAuthName = "Teilnehmende"
            };
            _context.ConferenceRoleAuthorizations.Add(participantAuth);

            var sgRole = new ConferenceSecretaryGeneralRole()
            {
                Conference = conference,
                IconName = "un",
                ConferenceRoleAuth = participantAuth,
                RoleFullName = "GeneralsekretärIn",
                RoleName = "GeneralsekretärIn",
                RoleShort = "GS",
                Title = "Seine/Ihre Exzellenz der/die GeneralsekretärIn",
            };

            _context.SecretaryGenerals.Add(sgRole);
            _context.SaveChanges();
            Assert.AreEqual(1, _context.SecretaryGenerals.Count());
            Assert.AreEqual(1, conference.Roles.OfType<ConferenceSecretaryGeneralRole>().Count());

        }

        [Test]
        [Order(4)]
        public void TestCreateDelegateRole()
        {
            var auth = _context.ConferenceRoleAuthorizations.FirstOrDefault(n => n.Conference == conference && n.RoleAuthName == "Teilnehmende");
            Assert.NotNull(auth);

            var committee = _context.Committees.FirstOrDefault(n => n.Conference.ConferenceId == conference.ConferenceId);
            Assert.NotNull(committee);

            var delegation = new Delegation()
            {
                Conference = conference,
                DelegationShort = "DE",
                FullName = "Delegations Deutschland",
                Name = "Deutschland"
            };

            _context.Delegations.Add(delegation);

            var country = new Country()
            {
                Continent = EContinent.Europe,
                FullName = "Bundesrepublik Deutschland",
                Iso = "de",
                Name = "Deutschland"
            };

            _context.Countries.Add(country);

            var delegateRole = new ConferenceDelegateRole()
            {
                Committee = committee,
                Conference = conference,
                IconName = "de",
                Delegation = delegation,
                ConferenceRoleAuth = auth,
                IsDelegationLeader = true,
                RoleFullName = "Abgeordneter Deutschlands",
                RoleName = "Deutschland",
                RoleShort = "DE",
                Title = "Abgeordneter Deutschlands in der Generalversammlung",
                DelegateCountry = country
            };

            _context.Delegates.Add(delegateRole);
            _context.SaveChanges();
            Assert.AreEqual(1, _context.Delegates.Count());
        }

        [Test]
        [Order(5)]
        public void CreatePressRole()
        {
            var auth = _context.ConferenceRoleAuthorizations.FirstOrDefault(n => n.Conference == conference && n.RoleAuthName == "Teilnehmende");
            Assert.NotNull(auth);

            var delegation = new Delegation()
            {
                DelegationShort = "presse",
                Conference = conference,
                FullName = "Konferenzpresse",
                Name = "Presse"
            };

            _context.Delegations.Add(delegation);

            var pressRole = new ConferenceDelegateRole()
            {
                Committee = null,
                Conference = conference,
                DelegateCountry = null,
                Delegation = delegation,
                IconName = "un",
                IsDelegationLeader = true,
                ConferenceRoleAuth = auth,
                RoleFullName = "Redaktionsleitung",
                RoleName = "Redaktionsleitung",
                RoleShort = "RL",
                Title = "ChefredakteurIn"
            };
            _context.Delegates.Add(pressRole);
            _context.SaveChanges();
            Assert.AreEqual(2, _context.Delegations.Count());
            Assert.AreEqual(2, _context.Delegates.Count());
        }

        [Test]
        [Order(6)]
        public void CreateNonGovernmentRole()
        {
            var auth = _context.ConferenceRoleAuthorizations.FirstOrDefault(n => n.Conference == conference && n.RoleAuthName == "Teilnehmende");
            Assert.NotNull(auth);

            var delegation = new Delegation()
            {
                DelegationShort = "ai",
                Conference = conference,
                FullName = "Amnesty International",
                Name = "Amnesty International"
            };

            _context.Delegations.Add(delegation);

            var role = new ConferenceDelegateRole()
            {
                Delegation = delegation,
                Committee = null,
                Conference = conference,
                DelegateCountry = null,
                IconName = "ai",
                IsDelegationLeader = true,
                RoleFullName = "Vertreter Amnesty International",
                RoleName = "Amnesty International",
                RoleShort = "Amnesty",
                Title = "Nichtstaatlicher Akteur für Amnesty International",
                ConferenceRoleAuth = auth,
            };

            _context.Delegates.Add(role);
            _context.SaveChanges();
            Assert.AreEqual(3, _context.Delegates.Count());
        }

    }
}
