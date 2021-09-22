using Microsoft.EntityFrameworkCore;
using MUNity.Database.Context;
using MUNity.Database.Models.Conference;
using MUNity.Database.Models.Conference.Roles;
using MUNity.Database.Models.Organization;
using MUNity.Database.Models.User;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUNity.Database.General;
using MUNityBase;

namespace MUNityDatabaseTest
{
    public abstract class AbstractDatabaseTests
    {
        public MunityContext _context;

        private Organization testOrganization;
        public Organization TestOrganization { 
            get
            {
                if (testOrganization == null)
                    testOrganization = CreateTestOrganization();
                return testOrganization;
            }
        }

        private Project testProject;
        public Project TestProject { 
            get
            {
                if (testProject == null)
                    testProject = CreateTestProject();
                return testProject;
            }
        }

        private Conference testConference;
        public Conference TestConference { 
            get
            {
                if (testConference == null)
                    testConference = CreateTestConference();
                return testConference;
            }
        }

        private Committee testCommittee;
        public Committee TestCommittee
        {
            get
            {
                if (testCommittee == null)
                    testCommittee = CreateTestCommittee();
                return testCommittee;
            }
        }

        public Committee TestCommitteeGV { get;  }

        string dbName = "munity_test.db";

        [OneTimeSetUp]
        public void Setup()
        {
            // Datenbank für den Test erzeugen und falls vorhanden erst einmal leeren und neu erstellen!
            var optionsBuilder = new DbContextOptionsBuilder<MunityContext>();
            optionsBuilder.UseSqlite("Data Source=test_conference.db");
            _context = new MunityContext(optionsBuilder.Options);
            ResetDatabase();   
        }

        public void ResetDatabase()
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        private Organization CreateTestOrganization()
        {
            var organization = new Organization()
            {
                OrganizationName = "Deutsche Model United Nations e.V.",
                OrganizationShort = "DMUN e.V."
            };
            _context.Organizations.Add(organization);
            _context.SaveChanges();
            return organization;
        }

        private Project CreateTestProject()
        {
            var project = new Project()
            {
                ProjectName = "MUN Schleswig-Holstein 2022",
                ProjectOrganization = TestOrganization,
                ProjectShort = "MUN-SH 2022",
            };
            _context.Projects.Add(project);
            _context.SaveChanges();
            return project;
        }

        private Conference CreateTestConference()
        {
            var conference = new Conference()
            {
                ConferenceProject = TestProject,
                ConferenceShort = "MUN-SH 2022",
                Name = "MUN Schleswig-Holstein 2022",
                FullName = "Model United Nations Schleswig-Holstein 2022"
            };
            _context.Conferences.Add(conference);
            _context.SaveChanges();
            return conference;
        }

        private Committee CreateTestCommittee()
        {
            return CreateCommittee("Generalversammlung", "Generalversammlung", "GV");
        }

        public Committee CreateCommittee(string name, string fullName, string committeeShort)
        {
            var committee = new Committee()
            {
                Conference = TestConference,
                Article = "die",
                FullName = "Generalversammlung",
                Name = "Generalversammlung"
            };
            _context.Committees.Add(committee);
            _context.SaveChanges();
            return committee;
        }

        public MunityUser CreateATestUser(string username)
        {
            var user = new MunityUser()
            {
                AccessFailedCount = 0,
                Email = $"{username}@mail.com",
                EmailConfirmed = true,
                Forename = "",
                Lastname = "",
                NormalizedUserName = username.ToUpper(),
                NormalizedEmail = $"{username.ToUpper()}@MAIL.COM",
            };
            _context.Users.Add(user);
            return user;
        }

        public ConferenceRoleAuth EnsureProjectOwnerAuth()
        {
            var ownerAuth = _context.ConferenceRoleAuthorizations.FirstOrDefault(n => n.RoleAuthName == "Project-Owner");
            if (ownerAuth == null)
            {
                ownerAuth = new ConferenceRoleAuth()
                {
                    Conference = TestConference,
                    CanEditConferenceSettings = true,
                    CanEditParticipations = true,
                    CanSeeApplications = true,
                    PowerLevel = 1,
                    RoleAuthName = "Project-Owner"
                };
                _context.ConferenceRoleAuthorizations.Add(ownerAuth);
                _context.SaveChanges();
            }
            return ownerAuth;
        }

        public ConferenceRoleAuth EnsureParticipantAuth()
        {
            var memberAuth = _context.ConferenceRoleAuthorizations.FirstOrDefault(n => n.RoleAuthName == "Participant");
            if (memberAuth == null)
            {
                memberAuth = new ConferenceRoleAuth()
                {
                    Conference = TestConference,
                    CanEditConferenceSettings = true,
                    CanEditParticipations = true,
                    CanSeeApplications = true,
                    PowerLevel = 1,
                    RoleAuthName = "Participant"
                };
                _context.ConferenceRoleAuthorizations.Add(memberAuth);
                _context.SaveChanges();
            }
            return memberAuth;
        }

        public Delegation EnsureDelegation(string name, string fullName, string shortName)
        {
            var delegation = _context.Delegations.FirstOrDefault(n => n.Name == name);
            if (delegation == null)
            {
                delegation = new Delegation()
                {
                    Conference = TestConference,
                    DelegationShort = shortName,
                    FullName = fullName,
                    Name = name
                };
                _context.Delegations.Add(delegation);
                _context.SaveChanges();
            }
            return delegation;
        }

        public Country EnsureGermany()
        {
            var country = _context.Countries.FirstOrDefault(n => n.Name == "Deutschland");
            if (country == null)
            {
                country = new Country()
                {
                    Continent = EContinent.Europe,
                    FullName = "Bundesrepublik Deutschland",
                    Iso = "de",
                    Name = "Deutschland"
                };

                _context.Countries.Add(country);
                _context.SaveChanges();
            }
            return country;
        }


        public ConferenceDelegateRole EnsureGermanyDelegateRole()
        {
            var delegateRole = _context.Delegates.FirstOrDefault(n => n.RoleName == "Deutschland");
            if (delegateRole == null)
            {
                delegateRole = new ConferenceDelegateRole()
                {
                    Committee = TestCommittee,
                    Conference = TestConference,
                    IconName = "de",
                    Delegation = EnsureDelegation("Deutschland", "Deutschland", "de"),
                    ConferenceRoleAuth = EnsureParticipantAuth(),
                    IsDelegationLeader = true,
                    RoleFullName = "Abgeordneter Deutschlands",
                    RoleName = "Deutschland",
                    RoleShort = "DE",
                    Title = "Abgeordneter Deutschlands in der Generalversammlung",
                    DelegateCountry = EnsureGermany()
                };

                _context.Delegates.Add(delegateRole);
                _context.SaveChanges();
            }
            return delegateRole;
        }

        public ConferenceDelegateRole CreateDelegateRole(Delegation delegation, Committee committee, string fullName, string name, string shortName)
        {
            var delegateRole = new ConferenceDelegateRole()
            {
                Committee = committee,
                Conference = TestConference,
                IconName = "de",
                Delegation = delegation,
                ConferenceRoleAuth = EnsureParticipantAuth(),
                IsDelegationLeader = true,
                RoleFullName = "Abgeordneter Deutschlands",
                RoleName = "Deutschland",
                RoleShort = "DE",
                Title = "Abgeordneter Deutschlands in der Generalversammlung",
                DelegateCountry = EnsureGermany()
            };

            _context.Delegates.Add(delegateRole);
            _context.SaveChanges();
            return delegateRole;
        }
        

        public TeamRoleGroup EnsureOwnerTeamRoleGroup()
        {
            var leaderRoleGroup = _context.TeamRoleGroups.FirstOrDefault(n => n.Name == "Projektleitung");
            if (leaderRoleGroup == null)
            {
                leaderRoleGroup = new TeamRoleGroup()
                {
                    FullName = "die Projektleitung",
                    Name = "Projektleitung",
                    TeamRoleGroupShort = "PL",
                    GroupLevel = 1
                };
                _context.TeamRoleGroups.Add(leaderRoleGroup);
                _context.SaveChanges();
            }
            return leaderRoleGroup;
        }

        public ConferenceTeamRole EnsureProjectOwnerRole()
        {
            var leaderRole = _context.TeamRoles.FirstOrDefault(n => n.RoleName == "Projektleiter");
            if (leaderRole == null)
            {
                var leaderAuth = EnsureProjectOwnerAuth();
                var leaderRoleGroup = EnsureOwnerTeamRoleGroup();
                leaderRole = new ConferenceTeamRole()
                {
                    Conference = TestConference,
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
            }
            return leaderRole;
        }


        public AbstractDatabaseTests(string dataSourceName)
        {
            this.dbName = dataSourceName;
        }
    }
}
