using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MUNity.Database.Context;
using NUnit.Framework;
using MUNity.Database.Extensions;
using MUNity.Database.Model.Conference;
using MUNity.Database.Model.Website;
using MUNity.Database.Models.Conference;
using MUNity.Database.Models.Organization;
using MUNity.Database.Models.User;
using MUNityBase;

namespace MUNity.Database.Test.MUNBW22Tests
{

    public class CompleteTestRunMUNBW22
    {
        private MunityContext _context;

        private IServiceProvider _serviceProvider;


        [OneTimeSetUp]
        public void SetupDatabase()
        {

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<MunityContext>(options =>
                options.UseSqlite("Data Source=testmunbw.db"));

            serviceCollection.AddIdentity<MunityUser, MunityRole>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<MunityContext>();

            // Needed to get the Identity Provider to run!
            serviceCollection.AddLogging();

            this._serviceProvider = serviceCollection.BuildServiceProvider();

            // Reset the Database.
            _context = _serviceProvider.GetRequiredService<MunityContext>();
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        [SetUp]
        public void ReloadContextToClearBuffer()
        {
            _context = _serviceProvider.GetRequiredService<MunityContext>();
        }

        // Id 0-50 Tests
        #region Plattoform Setup

        [Test]
        [Order(1)]
        public void TestSetupCountries()
        {
            var recaff = _context.SetupBaseCountries();
            Assert.NotZero(recaff);
            Assert.NotZero(_context.Countries.Count());
            Assert.NotZero(_context.CountryNameTranslations.Count());
        }

        [Test]
        [Order(2)]
        public void TestSetupBaseRoles()
        {
            var recaff = _context.SetupBaseRoles();
            Assert.NotZero(recaff);
            Assert.NotZero(_context.Roles.Count());
        }

        [Test]
        [Order(3)]
        public void TestCreateSomeUsers()
        {
            

        }

        #endregion

        // ID 51 - 99 Tests
        #region Account Setup

        [Test]
        [Order(51)]
        public async Task TestRegisterUsers()
        {
            var user1 = new MunityUser("pparker", "parker@spiderman.com") {Forename = "Peter", Lastname = "Parker"};
            var user2 = new MunityUser("tonystark", "tony@stark-industries.com") {Forename = "Antony", Lastname = "Stark"};
            var user3 = new MunityUser("muricaboi", "captain@amrica.com") { Forename = "Steve", Lastname = "Rogers"};
            var user4 = new MunityUser("blackwidow", "b.widow@avangers.com") { Forename = "Natasha", Lastname = "Romanoff"};
            var userManager = _serviceProvider.GetRequiredService<UserManager<MunityUser>>();
            var resultPeterParker = await userManager.CreateAsync(user1, "Passwort123");
            var resultTonyStark = await userManager.CreateAsync(user2, "Passwort123");
            var resultSteveRodgers = await userManager.CreateAsync(user3, "Passwort123");
            var resultNatashaRomanoff = await userManager.CreateAsync(user4, "Passwort123");
            Assert.IsTrue(resultPeterParker.Succeeded);
            Assert.IsTrue(resultTonyStark.Succeeded);
            Assert.IsTrue(resultSteveRodgers.Succeeded);
            Assert.IsTrue(resultNatashaRomanoff.Succeeded);
        }
        #endregion

        // ID 100 Setup the Organization DMUN
        #region Organization Setup
        [Test]
        [Order(100)]
        public void TestAddDMUNOrganization()
        {
            var orga = _context.AddOrganization(options =>
                options.WithName("Deutsche Model United Nations e.V.")
                    .WithShort("DMUN e.V.")
                    .WithAdminRole()
                    );
            Assert.NotNull(orga);
            Assert.AreEqual(1, _context.Organizations.Count());
            Assert.IsTrue(_context.Organizations.Any(n => n.OrganizationId == "dmunev"));
        }

        [Test]
        [Order(101)]
        public void TestMakeUserTheOrganizationAdmin()
        {
            var role = _context.OrganizationRoles
                .FirstOrDefault(n => n.RoleName == "Admin"
                                     && n.Organization.OrganizationId == "dmunev");
            Assert.NotNull(role);
            var user = _context.Users.FirstOrDefault(n => n.UserName == "tonystark");
            Assert.NotNull(user, "The required user was not found...");
            var membership = _context.AddMemberIntoRole(role, user);
            Assert.NotNull(membership);
            Assert.AreEqual(1, _context.OrganizationMembers.Count());
        }

        [Test]
        [Order(102)]
        public void TestCreateOrganizationMemberRole()
        {
            var organization = _context.Organizations.FirstOrDefault(n => n.OrganizationId == "dmunev");
            Assert.NotNull(organization);
            var memberRole = new OrganizationRole()
            {
                Organization = organization,
                RoleName = "Mitglied",
                CanCreateProject = false,
                CanCreateRoles = false,
                CanManageMembers = false
            };
            _context.OrganizationRoles.Add(memberRole);
            _context.SaveChanges();
            Assert.AreEqual(2, _context.OrganizationRoles.Count());
            Assert.AreEqual(2,
                _context.Organizations
                    .Include(n => n.Roles)
                    .FirstOrDefault(n => n.OrganizationId == "dmunev").Roles
                    .Count);
        }

        [Test]
        [Order(103)]
        public void TestAddUsersToOrganizationAsMembers()
        {
            var peterParker = _context.Users.FirstOrDefault(n => n.UserName == "pparker");
            var memberRole = _context.OrganizationRoles.FirstOrDefault(n => n.RoleName == "Mitglied" && n.Organization.OrganizationId == "dmunev");
            Assert.NotNull(memberRole);
            Assert.NotNull(peterParker);
            _context.AddMemberIntoRole(memberRole, peterParker);
            Assert.AreEqual(2, _context.OrganizationMembers.Count());
        }
        #endregion

        // Id 200 Tests (Project)
        #region Project MUNBW Setup
        [Test]
        [Order(200)]
        public void TestAddMUNBWProject()
        {
            var tonyStark = _context.Users.FirstOrDefault(n => n.UserName == "tonystark");
            Assert.NotNull(tonyStark);
            var project = _context.AddProject(options =>
                options.WithShort("MUNBW")
                    .WithName("Model United Nations Baden-Würrtemberg")
                    .WithOrganization("dmunev")
                    .WithCreationUser(tonyStark));
            Assert.NotNull(project);
            Assert.AreEqual(1, _context.Projects.Count());
            Assert.IsTrue(_context.Projects.Any(n => n.ProjectId == "munbw"));
        }

        [Test]
        [Order(201)]
        public void TestLoadingDmunContainsProject()
        {
            var dmun = _context.Organizations
                .Include(n => n.Projects)
                .FirstOrDefault(n => n.OrganizationId == "dmunev");
            Assert.AreEqual(1, dmun.Projects.Count);
        }
        #endregion

        // Id 300 Tests (Basic Conference)
        #region Basic Conference Setup
        [Test]
        [Order(300)]
        public void TestAddConference()
        {
            var tonyStark = _context.Users.FirstOrDefault(n => n.UserName == "tonystark");
            Assert.NotNull(tonyStark);
            var conference = _context.AddConference(options =>
                options.WithShort("MUNBW 22")
                    .WithName("Model United Nations Baden-Würrtemberg 2022")
                    .WithFullName("Model United Nations Baden-Würrtemberg 2022")
                    .WithProject("munbw")
                    .WithStartDate(new DateTime(2022, 5, 12))
                    .WithEndDate(new DateTime(2022, 5, 16))
                    .WithBasePrice(70m)
                    .ByUser(tonyStark));
            Assert.NotNull(conference);
            Assert.AreEqual(1, _context.Conferences.Count());
            Assert.IsTrue(_context.Conferences.Any(n => n.ConferenceId == "munbw22"));
            Assert.IsTrue(_context.Conferences.Any(n => n.Name == "Model United Nations Baden-Würrtemberg 2022"));
            Assert.IsTrue(_context.Conferences.Any(n => n.FullName == "Model United Nations Baden-Würrtemberg 2022"));
        }

        [Test]
        [Order(310)]
        public void TestAddGeneralversammlungUndHA3()
        {
            var conference = _context.Conferences.FirstOrDefault(n => n.ConferenceId == "munbw22");
            Assert.NotNull(conference);

            conference.AddCommittee(gv =>
                gv
                    .WithName("Generalversammlung")
                    .WithFullName("Generalversammlung")
                    .WithShort("GV")

                    .WithTopic("Nachhaltige soziale und wirtschaftliche Entwicklung nach Covid-19")
                    .WithTopic("Beschleunigung der Maßnahmen zur Umsetzung des Pariser Klimaabkommens")

                    .WithSubCommittee(ha3 => ha3.WithName("Hauptausschuss 3")
                        .WithFullName("Ausschuss für soziale, humanitäre und kulturelle Fragen")
                        .WithShort("HA3")
                        .WithTopic("Implementierung der UN-Behindertenrechtskonvention")
                        .WithTopic("Koordinierung internationaler Zusammenarbeit in der humanitären Hilfe")
                        .WithTopic("Rückgabe von Kunstgegenständen und kulturellen Artefakten")));
            var recaff = _context.SaveChanges();
            Assert.AreEqual(2, _context.Committees.Count());
            Assert.IsTrue(_context.Committees.Any(n => n.CommitteeId == "munbw22-gv"));
            Assert.IsTrue(_context.Committees.Any(n => n.CommitteeId == "munbw22-ha3"));
            Assert.AreEqual(5, _context.CommitteeTopics.Count());
        }

        [Test]
        [Order(311)]
        public void TestAddTeamLeaderAuth()
        {
            var recaff = _context.AddBasicConferenceAuthorizations("munbw22");
            Assert.NotZero(recaff);
            Assert.NotZero(_context.ConferenceRoleAuthorizations.Count());
        }

        [Test]
        [Order(311)]
        public void TestAddSIcherheitsratUndKFK()
        {
            var conference = _context.Conferences.FirstOrDefault(n => n.ConferenceId == "munbw22");
            Assert.NotNull(conference);

            conference.AddCommittee(sr => sr
                .WithName("Sicherheitsrat")
                .WithFullName("Sicherheitsrat")
                .WithShort("SR")
                .WithTopic("Aktuelle Situation in Afghanistan")
                .WithTopic("Nukleare Situation im Iran nach dem JCPOA")
                .WithTopic("Bedrohung des internationalen Friedens durch nichtstaatliche Akteure")

                .WithSubCommittee(kfk => kfk
                    .WithName("Komission für Friedenskonsolidierung")
                    .WithFullName("Komission für Friedenskonsolidierung")
                    .WithShort("KFK")
                    .WithTopic("Einsatz robuster Mandate in der Friedenssicherung")
                    .WithTopic("Langfristiger Frieden in Zypern")
                    .WithTopic("Internationale Kooperation in der Krisenprävention")));
            _context.SaveChanges();
            Assert.AreEqual(4, _context.Committees.Count());
            Assert.IsTrue(_context.Committees.Any(n => n.CommitteeId == "munbw22-sr"));
            Assert.IsTrue(_context.Committees.Any(n => n.CommitteeId == "munbw22-kfk"));
        }

        [Test]
        [Order(312)]
        public void TestAddWiSoUndKBE()
        {
            var conference = _context.Conferences.Find("munbw22");
            conference.AddCommittee(wiso => wiso
                .WithName("Wirtschafts- und Sozialrat")
                .WithFullName("Wirtschafts- und Sozialrat")
                .WithShort("WiSo")
                .WithTopic("Implementierung von Kreislaufwirtschaft zum Erreichen der nachhaltigen Entwicklungsziele")
                .WithTopic("Einführung einer globalen Mindeststeuer")

                .WithSubCommittee(kbe => kbe
                    .WithName("Komission für Bevölkerung und Entwicklung")
                    .WithFullName("Komission für Bevölkerung und Entwicklung")
                    .WithShort("KBE")
                    .WithTopic("Maßnahmen zur Bekämpfung der Luftverschmutzung")
                    .WithTopic("Tourismus und nachhaltige Entwicklung")
                    .WithTopic("Resiliente und nachhaltige Landwirtschaft")));
            _context.SaveChanges();
            Assert.AreEqual(6, _context.Committees.Count());
            Assert.IsTrue(_context.Committees.Any(n => n.CommitteeId == "munbw22-wiso"));
            Assert.IsTrue(_context.Committees.Any(n => n.CommitteeId == "munbw22-kbe"));
        }

        [Test]
        [Order(313)]
        public void TestAddRatderInternationalenOrganisation()
        {
            var conference = _context.Conferences.Find("munbw22");
            conference.AddCommittee(rat => rat
                .WithName("Rat der Internationalen Organisation für Migration")
                .WithFullName("Rat der Internationalen Organisation für Migration")
                .WithShort("Rat")
                .WithTopic("Implementierung des UN-Migrationspakts")
                .WithTopic("Gesundheitsversorgung von Migrant*innen")
                .WithTopic("Umgang mit traumatisierten Geflüchteten"));
            _context.SaveChanges();
            Assert.AreEqual(7, _context.Committees.Count());
            Assert.IsTrue(_context.Committees.Any(n => n.CommitteeId == "munbw22-rat"));
        }

        [Test]
        [Order(314)]
        public void TestAddKlimakonferenz()
        {
            var conference = _context.Conferences.Find("munbw22");
            conference.AddCommittee(kk => kk
                .WithName("Klimakonferenz")
                .WithFullName("Klimakonferenz")
                .WithShort("KK")
                .WithTopic("Rolle der Jugend bei der Umsetzung des Pariser Klimaabkommens")
                .WithTopic("Adaption an den Meeresspiegelanstieg in tiefliegenden Gebieten und Inselstaaten")
                .WithTopic("Umsetzung von SDG 7: Nachhaltige Energie für alle"));
            _context.SaveChanges();
            Assert.AreEqual(8, _context.Committees.Count());
            Assert.IsTrue(_context.Committees.Any(n => n.CommitteeId == "munbw22-kk"));
        }

        [Test]
        [Order(315)]
        public void TestAddMenschenrechtsrat()
        {
            var conference = _context.Conferences.Find("munbw22");
            conference.AddCommittee(mrr => mrr
                .WithName("Menschenrechtsrat")
                .WithFullName("Menschenrechtsrat")
                .WithShort("MRR")
                .WithTopic("Menschenrechtslage in der Republik Myanmar")
                .WithTopic("Bekämpfung der Diskriminierung aufgrund sexueller Orientierung und Identität")
                .WithTopic("Pressefreiheit und Schutz von Journalist*innen"));
            _context.SaveChanges();
            Assert.AreEqual(9, _context.Committees.Count());
            Assert.IsTrue(_context.Committees.Any(n => n.CommitteeId == "munbw22-mrr"));
        }

        [Test]
        [Order(316)]
        public void TestConferenceHasNineCommittees()
        {
            var conference = _context.Conferences
                .Include(n => n.Committees)
                .FirstOrDefault(n => n.ConferenceId == "munbw22");

            Assert.AreEqual(9, conference.Committees.Count);
        }

        #endregion

        // Id 400 Tests (Team)
        #region Team Setup

        [Test]
        [Order(400)]
        public void TestAddProjektleitung()
        {
            var conference = _context.Conferences.FirstOrDefault(n => n.ConferenceId == "munbw22");
            var role = conference.AddTeamRoleGroup(options => 
                options.WithShort("PL")
                .WithFullName("die  Projektleitung von MUNBW 2022")
                .WithName("Projektleitung")
                .WithRole(roleBuilder => 
                    roleBuilder.WithShort("PL")
                        .WithFullName("Projektleiter")
                        .WithName("Projektleiter")
                        .WithLevel(1)));
            _context.SaveChanges();
            Assert.NotNull(role);
            Assert.AreEqual(1, _context.TeamRoleGroups.Count());
            Assert.AreEqual(1, _context.ConferenceTeamRoles.Count());
        }

        [Test]
        [Order(401)]
        public void TestAddErweiterteProjektleitung()
        {
            var conference = _context.Conferences.FirstOrDefault(n => n.ConferenceId == "munbw22");
            var projektleitung = _context.ConferenceTeamRoles.FirstOrDefault(n =>
                n.Conference.ConferenceId == "munbw22" && n.RoleName == "Projektleiter");

            Assert.NotNull(projektleitung);

            var group = conference.AddTeamRoleGroup(options =>
                options.WithShort("EPL")
                    .WithFullName("die  erweiterte Projektleitung von MUNBW 2022")
                    .WithName("Erweiterte Projektleitung")
                    .WithRole("Generalsekretär", "GS")
                    .WithRole("Leitung Inhalt & Sekretariat", "Sek")
                    .WithRole("Leitung Nichtstaatlicher Akteure", "NAB")
                    .WithRole("Teilnehmendenwerbung", "TNW")
                    .WithRole("Team- & Materialkoordination", "TMK")
                    .WithRole("Chairbetreuung", "CB")
                    .WithRole("Leitung Technik/Digitalisierung", "TECH")
                    .WithRole("Leitung Medien")
                    .WithRole("Leitung Bildungsprogramm")
                    .WithRole("Teilnehmenden- & Kommservice-Betreuung", "TNB")
                    .WithRole("Kassenwart", "CASH")
                    .WithRole("Fundraising")
                    .WithRoleLevel(2)
                    .WithParentRole(projektleitung));
            _context.SaveChanges();

            Assert.AreEqual(2, _context.TeamRoleGroups.Count());
            Assert.AreEqual(13, _context.ConferenceTeamRoles.Count());
        }

        [Test]
        [Order(402)]
        public void TestConferenceHas13Roles()
        {
            var conference = _context.Conferences.Include(n => n.Roles)
                .FirstOrDefault(n => n.ConferenceId == "munbw22");
            Assert.AreEqual(13, conference.Roles.Count);
        }

        [Test]
        [Order(403)]
        public void TestMakeTonyStarkProjektleiter()
        {
            var projektleitung = _context.ConferenceTeamRoles
                .FirstOrDefault(n =>
                n.RoleName == "Projektleiter" && n.Conference.ConferenceId == "munbw22");

            var tonyStark = _context.Users.FirstOrDefault(n => n.UserName == "tonystark");
            Assert.NotNull(tonyStark);

            Assert.NotNull(projektleitung);
            var participation = new Participation()
            {
                Role = projektleitung,
                User = tonyStark,
                Cost = 50,
                IsMainRole = true,
                Paid = 0,
                ParticipationSecret = "GENERATE_SECRET"
            };
            _context.Participations.Add(participation);
            _context.SaveChanges();
            Assert.IsTrue(_context.Participations.Any(n => n.Role.Conference.ConferenceId == "munbw22" && n.User.UserName == "tonystark"));
        }

        #endregion

        // ID 500 Tests (Seats)
        #region Seats

        [Test]
        [Order(500)]
        public void TestAddSeatsToGV()
        {
            _context.AddSeat("munbw22-gv", "Abgeordnete*r Afghanistan", 2);
            _context.AddSeat("munbw22-gv", "Abgeordnete*r Deutschland", 38);
            _context.AddSeat("munbw22-gv", "Abgeordnete*r Frankreich", 50);
            _context.AddSeat("munbw22-gv", "Abgeordnete*r Vereinigte Staaten", 207);

            Assert.AreEqual(4, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-gv"));
        }

        [Test]
        [Order(501)]
        public void TestAddSeatsToHA3()
        {
            _context.AddSeat("munbw22-ha3", "Abgeordnete*r Deutschland", 38);
            _context.AddSeat("munbw22-ha3", "Abgeordnete*r Frankreich", 50);
            _context.AddSeat("munbw22-ha3", "Abgeordnete*r Vereinigte Staaten", 207);

            Assert.AreEqual(3, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-ha3"));
        }

        [Test]
        [Order(502)]
        public void TestAddSeatsToSR()
        {
            _context.AddSeat("munbw22-sr", "Abgeordnete*r Deutschland", 38);
            _context.AddSeat("munbw22-sr", "Abgeordnete*r Frankreich", 50);
            _context.AddSeat("munbw22-sr", "Abgeordnete*r Vereinigte Staaten", 207);

            Assert.AreEqual(3, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-sr"));
        }

        [Test]
        [Order(503)]
        public void TestAddSeatsToKFK()
        {
            _context.AddSeat("munbw22-kfk", "Abgeordnete*r Deutschland", 38);
            _context.AddSeat("munbw22-kfk", "Abgeordnete*r Frankreich", 50);
            _context.AddSeat("munbw22-kfk", "Abgeordnete*r Vereinigte Staaten", 207);

            Assert.AreEqual(3, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-kfk"));
        }

        [Test]
        [Order(504)]
        public void TestAddSeatsToWiSo()
        {
            _context.AddSeat("munbw22-wiso", "Abgeordnete*r Deutschland", 38);
            _context.AddSeat("munbw22-wiso", "Abgeordnete*r Frankreich", 50);
            _context.AddSeat("munbw22-wiso", "Abgeordnete*r Vereinigte Staaten", 207);

            Assert.AreEqual(3, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-wiso"));
        }

        [Test]
        [Order(505)]
        public void TestAddSeatsToKBE()
        {
            _context.AddSeat("munbw22-kbe", "Abgeordnete*r Deutschland", 38);
            _context.AddSeat("munbw22-kbe", "Abgeordnete*r Frankreich", 50);
            _context.AddSeat("munbw22-kbe", "Abgeordnete*r Vereinigte Staaten", 207);

            Assert.AreEqual(3, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-kbe"));
        }

        [Test]
        [Order(506)]
        public void TestAddSeatsToRat()
        {
            _context.AddSeat("munbw22-rat", "Abgeordnete*r Deutschland", 38);
            _context.AddSeat("munbw22-rat", "Abgeordnete*r Frankreich", 50);
            _context.AddSeat("munbw22-rat", "Abgeordnete*r Vereinigte Staaten", 207);

            Assert.AreEqual(3, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-rat"));
        }

        [Test]
        [Order(507)]
        public void TestAddSeatsToKK()
        {
            _context.AddSeat("munbw22-kk", "Abgeordnete*r Deutschland", 38);
            _context.AddSeat("munbw22-kk", "Abgeordnete*r Frankreich", 50);
            _context.AddSeat("munbw22-kk", "Abgeordnete*r Vereinigte Staaten", 207);

            Assert.AreEqual(3, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-kk"));
        }

        [Test]
        [Order(508)]
        public void TestAddSeatsToMRR()
        {
            _context.AddSeat("munbw22-mrr", "Abgeordnete*r Deutschland", 38);
            _context.AddSeat("munbw22-mrr", "Abgeordnete*r Frankreich", 50);
            _context.AddSeat("munbw22-mrr", "Abgeordnete*r Vereinigte Staaten", 207);

            Assert.AreEqual(3, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-mrr"));
        }

        [Test]
        [Order(509)]
        public void TestAddNAs()
        {

        }

        [Test]
        [Order(510)]
        public void TestAddPress()
        {

        }

        [Test]
        [Order(511)]
        public void TestCreateDelegations()
        {


            _context.GroupRolesOfCountryIntoADelegation("munbw22", 38, "Deutschland");
            _context.GroupRolesOfCountryIntoADelegation("munbw22", 50, "Frankreich");
            _context.GroupRolesOfCountryIntoADelegation("munbw22", 207, "Vereinigte Staaten");

            var allDelegations = _context.Delegations
                .Include(n => n.ChildDelegations)
                .Include(n => n.Roles)
                .Where(n => n.Conference.ConferenceId == "munbw22")
                .ToList();

            Assert.AreEqual(3, allDelegations.Count);

            var delegationDeutschland = allDelegations.FirstOrDefault(n => n.Name == "Deutschland");
            Assert.NotNull(delegationDeutschland);
            Assert.AreEqual(9, delegationDeutschland.Roles.Count);
            Assert.IsTrue(_context.Delegations.Any(n => n.DelegationId == "munbw22-deutschland"));
        }

        [Test]
        [Order(512)]
        public void TestAddCostRuleForMrr()
        {
            var mrr = _context.Committees.FirstOrDefault(n => n.CommitteeId == "munbw22-mrr");
            var costRule = new ConferenceParticipationCostRule()
            {
                Committee = mrr,
                Conference = null,
                Role = null,
                Delegation = null,
                AddPercentage = null,
                CostRuleTitle = "Preis für Online-Gremien",
                Costs = 50.00m,
                CutPercentage = null,
                UserMaxAge = null,
                UserMinAge = null
            };
            _context.ConferenceParticipationCostRules.Add(costRule);
            _context.SaveChanges();
            Assert.AreEqual(1, _context.ConferenceParticipationCostRules.Count());
        }

        #endregion

        // ID 600 Test Registration/Application
        #region Application Phase (Anmeldephase) - 600

        [Test]
        [Order(600)]
        public void TestCreateTheApplicationPage()
        {
            var conference = _context.Conferences.Find("munbw22");
            Assert.NotNull(conference);
            var page = new ConferenceApplicationPage()
            {
                Conference = conference,
                Title = "Anmeldung",
                ApplicationEndDate = new DateTime(2022, 2, 20, 12, 0, 0),
                ApplicationStartDate = new DateTime(2021, 12, 12, 12, 0, 0),
                DescriptionExperience = "Giben Sie hier an, welche MUN Erfahrungen Sie/Ihr bereits mitbringt",
                DescriptionMotivation =
                    "Giben Sie hier an, warum gerade Sie/Ihr für diese Delegation/diese Rolle geeignet seid",
                ExperienceMaxLength = 500,
                ExperienceMinLength = 10,
                ExperienceRequired = true,
                IsActive = true,
                MotivationMaxLength = 500,
                MotivationMinLength = 10,
                MotivationRequired = true,
                PostContent =
                    "Die Anmeldungen werden am 20.02.2022 abgeschlossen. Dann Erfahren Sie, welche Delegation und Rolle Sie bekommen",
                PreContent = "Die Anmeldung bei Model United Nations Baden-Würrtemberg...",
                SchoolRequired = false,
                ShowExperienceField = true,
                ShowMotivationField = true,
                ShowSchoolField = true,
                TitleChooseDelegation = "Wunschdelegation auswählen",
                TitleExperience = "Erfahrung",
                TitleMotivation = "Motivation"
            };
            _context.ConferenceApplicationPages.Add(page);
            _context.SaveChanges();
            Assert.IsTrue(_context.ConferenceApplicationPages.Any(n => n.Conference.ConferenceId == "munbw22"));
        }

        [Test]
        [Order(610)]
        public void TestSetApplicationStateOnDelegations()
        {
            var delegationDeutschland = _context.Delegations
                .Include(n => n.Roles)
                .FirstOrDefault(n => n.Conference.ConferenceId == "munbw22" && n.Name == "Deutschland");

            Assert.NotNull(delegationDeutschland);

            foreach (var role in delegationDeutschland.Roles)
            {
                role.ApplicationState = EApplicationStates.DelegationApplication;
            }

            var result = _context.SaveChanges();
            Assert.AreEqual(9, result);
        }

        [Test]
        [Order(611)]
        [Description("This test should show that an application can be added to a specific committee.")]
        public void TestCreateTheApplicationForGermany()
        {
            var delegationGermany =
                _context.Delegations.FirstOrDefault(
                    n => n.Conference.ConferenceId == "munbw22" && n.Name == "Deutschland");

            var delegationFrance =
                _context.Delegations.FirstOrDefault(
                    n => n.Conference.ConferenceId == "munbw22" && n.Name == "Frankreich");

            var delegationUSA =
                _context.Delegations.FirstOrDefault(
                    n => n.Conference.ConferenceId == "munbw22" && n.Name == "Vereinigte Staaten");

            Assert.NotNull(delegationGermany);
            Assert.NotNull(delegationFrance);
            Assert.NotNull(delegationUSA);

            var application = new DelegationApplication()
            {
                Delegations = new List<DelegationApplicationPickedDelegation>(),
                Users = new List<DelegationApplicationUserEntry>(),
                Title = "Anmeldung der Schule 1",
                ApplyDate = DateTime.Now,
                Experience = "Hier steht der Text über die Erfahrung der Bewerber",
                Motivation = "Hier steht ein Absatz über die Motivation der Bewerber",
                OpenToPublic = false,
                SchoolName = "Schule 1",
                Status = ApplicationStatuses.Writing
            };

            application.Delegations.Add(new DelegationApplicationPickedDelegation()
            {
                Delegation = delegationGermany,
                Application = application,
                Priority = 1
            });

            application.Delegations.Add(new DelegationApplicationPickedDelegation()
            {
                Delegation = delegationFrance,
                Application = application,
                Priority = 2
            });

            application.Delegations.Add(new DelegationApplicationPickedDelegation()
            {
                Delegation = delegationUSA,
                Application = application,
                Priority = 3
            });

            var user = _context.Users.FirstOrDefault(n => n.UserName == "muricaboi");
            Assert.NotNull(user);

            application.Users.Add(new DelegationApplicationUserEntry()
            {
                User = user,
                Application = application,
                Status = DelegationApplicationUserEntryStatuses.Joined,
                CanWrite = true
            });

            _context.DelegationApplications.Add(application);
            _context.SaveChanges();
        }

        [Test]
        [Order(612)]
        [Description("This test ensures that the application has been created and can be found inside a list of all applications.")]
        public void TestThatConferenceHasOneApplication()
        {
            var applications = _context.DelegationApplications
                .Where(n => n.Delegations
                    .Any(a => a.Delegation.Conference.ConferenceId == "munbw22"))
                .ToList();

            Assert.AreEqual(1, applications.Count);
            Assert.AreEqual(1, _context.DelegationApplicationUserEntries.Count());
            Assert.AreEqual(3, _context.DelegationApplicationPickedDelegations.Count());
        }

        [Test]
        [Order(613)]
        [Description("This test should allow a user to become part of an application. This should only be allowed if the application is Open.")]
        public void TestUserCanApplyOnApplication()
        {
            var user = _context.Users.FirstOrDefault(n => n.UserName == "blackwidow");
            Assert.NotNull(user);

            var application = _context.DelegationApplications.FirstOrDefault();
            Assert.NotNull(application);

            var entry = new DelegationApplicationUserEntry()
            {
                User = user,
                Application = application,
                Status = DelegationApplicationUserEntryStatuses.RequestJoining,
                CanWrite = false,
                Message = "Hallo, ich bin Black Widow und möchte bei euch dabei sein :)."
            };

            _context.DelegationApplicationUserEntries.Add(entry);

            _context.SaveChanges();

            var reloadApplication = _context.DelegationApplications
                .Include(n => n.Users)
                .FirstOrDefault();
            Assert.AreEqual(2, reloadApplication.Users.Count);
        }

        [Test]
        [Order(614)]
        public void TestCalculateCostOfApplication()
        {
            var application = _context.DelegationApplications
                .Include(n => n.Delegations)
                .ThenInclude(n => n.Delegation)
                .ThenInclude(n => n.Roles)
                .ThenInclude(n => n.Committee)
                .ThenInclude(n => n.Conference)
                .FirstOrDefault();
            Assert.NotNull(application);

            var conferenceBasePrice = application.Delegations.First().Delegation.Conference.GeneralParticipationCost;
            Assert.AreEqual(70, conferenceBasePrice);

            Dictionary<string, decimal> priceDelegations = new Dictionary<string, decimal>();

            // Durch alle Delegationen auf der Liste durchgehen
            foreach (var delegation in application.Delegations.Select(a => a.Delegation))
            {
                Console.WriteLine($"Preisberechnung für {delegation.Name}");
                decimal sumPriceDelegation = 0;

                // Als Standardpreis wird der Konferenzpreis angezogen
                decimal tempPriceRole = conferenceBasePrice;
                decimal priceDelegation = conferenceBasePrice;

                // ggf. anfallender Sonderpreis auf die Delegation:
                var costForDelegation =
                    _context.ConferenceParticipationCostRules.FirstOrDefault(n =>
                        n.Delegation.DelegationId == delegation.DelegationId);

                if (costForDelegation?.Costs != null)
                {
                    priceDelegation = costForDelegation.Costs.Value;
                }

                // Dafür noch einmal alle Rollen durch gehen und ggf. anfallende Ausnahmen in 
                // betracht ziehen
                foreach (var role in delegation.Roles)
                {
                    tempPriceRole = priceDelegation;

                    var costForCommittee =
                        _context.ConferenceParticipationCostRules.FirstOrDefault(n =>
                            n.Committee.CommitteeId == role.Committee.CommitteeId);

                    if (costForCommittee?.Costs != null)
                    {
                        tempPriceRole = costForCommittee.Costs.Value;
                    }

                    var costForRole =
                        _context.ConferenceParticipationCostRules.FirstOrDefault(n => n.Role.RoleId == role.RoleId);

                    if (costForRole?.Costs != null)
                    {
                        tempPriceRole = costForRole.Costs.Value;
                    }

                    Console.WriteLine($"Teilnahme in {role.Committee.CommitteeId} kostet {tempPriceRole}");

                    sumPriceDelegation += tempPriceRole;
                }

                // Preis für 8 normale Gremien (70€) und ein online Gremium (50€).
                Console.WriteLine("---------------------");
                Console.WriteLine($"Summe:\t{sumPriceDelegation}");
                priceDelegations.Add(delegation.Name, sumPriceDelegation);
                //Assert.AreEqual(610m, sumPriceDelegation);
            }

            Assert.AreEqual(610, priceDelegations["Deutschland"]);
            Assert.AreEqual(610, priceDelegations["Frankreich"]);
            Assert.AreEqual(610, priceDelegations["Vereinigte Staaten"]);
        }

        #endregion

        // ID 700 Test Website

    }
}
