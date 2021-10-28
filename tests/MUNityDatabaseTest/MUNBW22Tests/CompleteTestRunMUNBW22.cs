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
using MUNity.Database.FluentAPI;
using MUNity.Database.Models.Conference;
using MUNity.Database.Models.Website;
using MUNity.Database.Models.Conference.Roles;
using MUNity.Database.Models.Organization;
using MUNity.Database.Models.User;
using MUNityBase;
using MUNity.Database.Extensions;

namespace MUNity.Database.Test.MUNBW22Tests
{

    public class CompleteTestRunMUNBW22
    {
        private MunityContext _context;

        private IServiceProvider _serviceProvider;

        public static class TestUsers
        {
            public static IEnumerable<MunityUser> AllUsers
            {
                get
                {
                    yield return Avangers.FoundingMembers.TonyStark;

                    yield return Avangers.SixtiesRecruits.CaptainAmerica;

                    yield return Avangers.SeventiesRecruits.BlackWidow;

                    yield return Avangers.NinetiesRecruits.PeterParker;

                    yield return XMen.OriginalMembers.ProfessorX;
                    yield return XMen.OriginalMembers.Cyclops;
                    yield return XMen.OriginalMembers.Iceman;
                    yield return XMen.OriginalMembers.Beast;
                    yield return XMen.OriginalMembers.Angel;
                    yield return XMen.OriginalMembers.MarvelGirl;
                }
            }

            public static class Avangers
            {
                /// <summary>
                ///  Use the founding members as EPL
                /// </summary>
                public static class FoundingMembers
                {
                    /// <summary>
                    /// User is the owner of the Organization (Clemens)
                    /// weitere Organisationsbenutzer sind in diesem Test nicht notwendig, da es hier um MUNBW und nicht um organisationen an sich geht...
                    /// </summary>
                    public static MunityUser TonyStark { get; set; } = new MunityUser("tonystark", "tony@stark-industries.com") { Forename = "Antony", Lastname = "Stark" };
                    public static MunityUser HankPym { get; set; } = new MunityUser("hankpym", "hank-pym@avangers.com") { Forename = "Henry Jonathan", Lastname = "Pym" };
                    
                    /// <summary>
                    /// Projektleiter 1 (J. T.)
                    /// </summary>
                    public static MunityUser JanetVanDyne { get; set; } = new MunityUser("jandyne", "janet-dyne@avangers.com") { Forename = "Janet", Lastname = "van Dyne" };
                    
                    /// <summary>
                    /// Projektleiter 2 (M. I.)
                    /// </summary>
                    public static MunityUser TheHulk { get; set; } = new MunityUser("hulk", "hulk@avangers.com") { Forename = "Robert Bruce", Lastname = "Banner" };
                    
                    /// <summary>
                    /// Projektleiter 3 (T. S.)
                    /// </summary>
                    public static MunityUser Thor { get; set; } = new MunityUser("rickjones", "rock@avangers.com") { Forename = "Richard Milhouse", Lastname = "Jones" };

                }

                public static class SixtiesRecruits
                {
                    /// <summary>
                    /// Generalsekretär (J. M.)
                    /// </summary>
                    public static MunityUser CaptainAmerica { get; set; } = new MunityUser("muricaboi", "captain@amrica.com") { Forename = "Steve", Lastname = "Rogers" };
                    
                    /// <summary>
                    /// Leitung Inhalt & Sekretariat (K. V.)
                    /// </summary>
                    public static MunityUser Hawkeye { get; set; } = new MunityUser("hawkeye", "hawkeye@amrica.com") { Forename = "Clinton Francis", Lastname = "Barton" };


                }

                public static class SeventiesRecruits
                {
                    public static MunityUser BlackWidow { get; set; } = new MunityUser("blackwidow", "b.widow@avangers.com") { Forename = "Natasha", Lastname = "Romanoff" };

                }

                public static class NinetiesRecruits
                {
                    public static MunityUser PeterParker { get; set; } = new MunityUser("pparker", "parker@spiderman.com") { Forename = "Peter Benjamin", Lastname = "Parker" };

                }




            }

            public static class XMen
            {
                /// <summary>
                /// Delegation aus 6
                /// </summary>
                public static class OriginalMembers
                {
                    public static MunityUser ProfessorX { get; set; } = new MunityUser("professorx", "professorx@x-men.com") { Forename = "Charles Francis", Lastname = "Xavier" };
                    public static MunityUser Cyclops { get; set; } = new MunityUser("cyclops", "cyclops@x-men.com") { Forename = "Scott", Lastname = "Summers" };
                    public static MunityUser Iceman { get; set; } = new MunityUser("iceman", "iceman@x-men.com") { Forename = "Robert Louis", Lastname = "Drake" };
                    public static MunityUser Beast { get; set; } = new MunityUser("beast", "beast@x-men.com") { Forename = "Henry Philip", Lastname = "McCoy" };
                    public static MunityUser Angel { get; set; } = new MunityUser("angel", "angel@x-men.com") { Forename = "Warren Kenneth", Lastname = "Worthington III" };
                    public static MunityUser MarvelGirl { get; set; } = new MunityUser("marvelgirl", "marvel.girl@x-men.com") { Forename = "Jean Elaine", Lastname = "Grey" };

                }
            }
        }


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

        #endregion

        // ID 51 - 99 Tests
        #region Account Setup

        [Test]
        [Order(51)]
        public async Task TestRegisterUsers()
        {
            var userManager = _serviceProvider.GetRequiredService<UserManager<MunityUser>>();

            foreach(var user in TestUsers.AllUsers)
            {
                var creationResult = await userManager.CreateAsync(user, "Passwort123");
                Assert.IsTrue(creationResult.Succeeded);
            }
        }
        #endregion

        // ID 100 Setup the Organization DMUN
        #region Organization Setup
        [Test]
        [Order(100)]
        public void TestAddDMUNOrganization()
        {
            var orga = _context.Fluent.Organization.AddOrganization(options =>
                options.WithName("Deutsche Model United Nations e.V.")
                    .WithShort("DMUN e.V.")
                    .WithAdminRole()
                    .WithMemberRole("Mitglied"));
            // Assert that the organization exists
            Assert.NotNull(orga);
            Assert.AreEqual(1, _context.Organizations.Count());
            Assert.IsTrue(_context.Organizations.Any(n => n.OrganizationId == "dmunev"));

            // Assert that the roles exist
            Assert.AreEqual(2, _context.OrganizationRoles.Count(n => n.Organization.OrganizationId == "dmunev"));
        }

        [Test]
        [Order(101)]
        public void TestMakeUserTheOrganizationAdmin()
        {
            var membership = _context.Fluent.ForOrganization("dmunev").AddUserIntoRole("tonystark", "Admin");
            Assert.NotNull(membership);
            Assert.AreEqual(1, _context.OrganizationMembers.Count());
        }

        [Test]
        [Order(103)]
        public void TestAddUsersToOrganizationAsMembers()
        {
            _context.Fluent.ForOrganization("dmunev").AddUserIntoRole("pparker", "Mitglied");
            Assert.AreEqual(2, _context.OrganizationMembers.Count());
            Assert.IsTrue(_context.Fluent.ForOrganization("dmunev").HasUserMembership("pparker"));
        }

        [Test]
        [Order(104)]
        public void TestAddUnknownUserThrowsException()
        {
            Assert.Throws<UserNotFoundException>(() => _context.Fluent.ForOrganization("dmunev").AddUserIntoRole("unknownUser", "Admin"));
        }

        [Test]
        [Order(105)]
        public void TestAddUserUnknowRoleThrowsException()
        {
            Assert.Throws<OrganizationRoleNotFoundException>(() => _context.Fluent.ForOrganization("dmunev").AddUserIntoRole("blackwidow", "NO ROLE"));
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
            var project = _context.Fluent.Project.AddProject(options =>
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
            var conference = _context.Fluent.Conference.AddConference(options =>
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

            _context.Fluent.ForConference("munbw22").AddCommittee(gv =>
                gv
                    .WithName("Generalversammlung")
                    .WithFullName("Generalversammlung")
                    .WithShort("GV")
                    .WithType(CommitteeTypes.AtLocation)
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

            _context.Fluent.ForConference("munbw22").AddCommittee(sr => sr
                .WithName("Sicherheitsrat")
                .WithFullName("Sicherheitsrat")
                .WithShort("SR")
                .WithType(CommitteeTypes.AtLocation)
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
            Assert.AreEqual(4, _context.Committees.Count());
            Assert.IsTrue(_context.Committees.Any(n => n.CommitteeId == "munbw22-sr"));
            Assert.IsTrue(_context.Committees.Any(n => n.CommitteeId == "munbw22-kfk"));
        }

        [Test]
        [Order(312)]
        public void TestAddWiSoUndKBE()
        {
            _context.Fluent.ForConference("munbw22").AddCommittee(wiso => wiso
                .WithName("Wirtschafts- und Sozialrat")
                .WithFullName("Wirtschafts- und Sozialrat")
                .WithShort("WiSo")
                .WithType(CommitteeTypes.AtLocation)
                .WithTopic("Implementierung von Kreislaufwirtschaft zum Erreichen der nachhaltigen Entwicklungsziele")
                .WithTopic("Einführung einer globalen Mindeststeuer")

                .WithSubCommittee(kbe => kbe
                    .WithName("Komission für Bevölkerung und Entwicklung")
                    .WithFullName("Komission für Bevölkerung und Entwicklung")
                    .WithShort("KBE")
                    .WithTopic("Maßnahmen zur Bekämpfung der Luftverschmutzung")
                    .WithTopic("Tourismus und nachhaltige Entwicklung")
                    .WithTopic("Resiliente und nachhaltige Landwirtschaft")));
            Assert.AreEqual(6, _context.Committees.Count());
            Assert.IsTrue(_context.Committees.Any(n => n.CommitteeId == "munbw22-wiso"));
            Assert.IsTrue(_context.Committees.Any(n => n.CommitteeId == "munbw22-kbe"));
        }

        [Test]
        [Order(313)]
        public void TestAddRatderInternationalenOrganisation()
        {
            _context.Fluent.ForConference("munbw22").AddCommittee(rat => rat
                .WithName("Rat der Internationalen Organisation für Migration")
                .WithFullName("Rat der Internationalen Organisation für Migration")
                .WithShort("IOM")
                .WithType(CommitteeTypes.AtLocation)
                .WithTopic("Implementierung des UN-Migrationspakts")
                .WithTopic("Gesundheitsversorgung von Migrant*innen")
                .WithTopic("Umgang mit traumatisierten Geflüchteten"));
            Assert.AreEqual(7, _context.Committees.Count());
            Assert.IsTrue(_context.Committees.Any(n => n.CommitteeId == "munbw22-iom"));
        }

        [Test]
        [Order(314)]
        public void TestAddKlimakonferenz()
        {
            _context.Fluent.ForConference("munbw22").AddCommittee(kk => kk
                .WithName("Klimakonferenz")
                .WithFullName("Klimakonferenz")
                .WithShort("KK")
                .WithType(CommitteeTypes.AtLocation)
                .WithTopic("Rolle der Jugend bei der Umsetzung des Pariser Klimaabkommens")
                .WithTopic("Adaption an den Meeresspiegelanstieg in tiefliegenden Gebieten und Inselstaaten")
                .WithTopic("Umsetzung von SDG 7: Nachhaltige Energie für alle"));
            Assert.AreEqual(8, _context.Committees.Count());
            Assert.IsTrue(_context.Committees.Any(n => n.CommitteeId == "munbw22-kk"));
        }

        [Test]
        [Order(315)]
        public void TestAddMenschenrechtsrat()
        {
            _context.Fluent.ForConference("munbw22").AddCommittee(mrr => mrr
                .WithName("Menschenrechtsrat")
                .WithFullName("Menschenrechtsrat")
                .WithShort("MRR")
                .WithType(CommitteeTypes.Online)
                .WithTopic("Menschenrechtslage in der Republik Myanmar")
                .WithTopic("Bekämpfung der Diskriminierung aufgrund sexueller Orientierung und Identität")
                .WithTopic("Pressefreiheit und Schutz von Journalist*innen"));
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
            var role = _context.Fluent.ForConference("munbw22").AddTeamRoleGroup(options => 
                options.WithShort("PL")
                .WithFullName("die  Projektleitung von MUNBW 2022")
                .WithName("Projektleitung")
                .WithRole(roleBuilder => 
                    roleBuilder.WithShort("PL")
                        .WithFullName("Projektleiter")
                        .WithName("Projektleiter")
                        .WithLevel(1)));
            Assert.NotNull(role);
            Assert.AreEqual(1, _context.TeamRoleGroups.Count());
            Assert.AreEqual(1, _context.ConferenceTeamRoles.Count());
        }

        [Test]
        [Order(401)]
        public void TestAddErweiterteProjektleitung()
        {
            var projektleitung = _context.ConferenceTeamRoles.FirstOrDefault(n =>
                n.Conference.ConferenceId == "munbw22" && n.RoleName == "Projektleiter");

            Assert.NotNull(projektleitung);

            var group = _context.Fluent.ForConference("munbw22").AddTeamRoleGroup(options =>
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
            _context.Fluent.ForConference("munbw22").MakeUserParticipateInTeamRole("tonystark", "Projektleiter");
            Assert.IsTrue(_context.Participations.Any(n => n.Role.Conference.ConferenceId == "munbw22" && n.User.UserName == "tonystark"));
        }

        #endregion

        // ID 500 Tests (Seats)
        #region Seats

        [Test]
        [Order(500)]
        public void TestAddSeatsToGV()
        {
            // Afrika
            _context.Fluent.ForCommittee("munbw22-gv").AddSeatsByCountryNames(
                "Ägypten", "Algerien", "Angola", "Äthiopien",
                "Burkina Faso", "Elfenbeinküste", "Eritrea", "Gabun",
                "Ghana", "Kamerun", "Kenia", "Demokratische Republik Kongo",
                "Republik Kongo", "Libyen", "Madagaskar", "Mosambik",
                "Nigeria", "Ruanda", "Seychellen", "Südafrika",
                "Sudan", "Südsudan", "Tschad", "Tunesien",
                "Uganda", "Zentralafrikanische Republik",

                // Asien
                "Bangladesch", "Volksrepublik China", "Fidschi", "Indien",
                "Indonesien", "Iran", "Japan", "Jemen",
                "Kasachstan", "Katar", "Myanmar", "Pakistan",
                "Palau", "Papua-Neuguinea", "Philippinen", "Samoa",
                "Saudi-Arabien", "Singapur", "Südkorea", "Syrien",
                "Thailand", "Usbekistan", "Vereinigte Arabische Emirate", "Vietnam",
                "Zypern",

                // Osteuropa
                "Albanien", "Bosnien und Herzegowina", "Estland", "Kroatien",
                "Lettland", "Polen", "Rumänien", "Russland", "Ukraine",
                "Ungarn", "Belarus",

                // Lateinamerika
                "Argentinien", "Brasilien", "Chile", "Costa Rica",
                "Dominikanische Republik", "Ecuador", "Haiti", "Jamaika",
                "Kolumbien", "Kuba", "Mexiko", "Nicaragua",
                "Peru", "Trinidad und Tobago", "Uruguay", "Venezuela",

                // Westeuropa
                "Australien", "Deutschland", "Frankreich", "Irland",
                "Israel", "Italien", "Kanada", "Niederlande",
                "Norwegen", "Schweden", "Schweiz", "Türkei",
                "Vereinigte Staaten", "Vereinigtes Königreich"
                );

            Assert.AreEqual(92, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-gv"));
        }

        [Test]
        [Order(501)]
        public void TestAddSeatsToHA3()
        {
            // Afrika
            _context.Fluent.ForCommittee("munbw22-ha3").AddSeatsByCountryNames(
                "Ägypten", "Angola", "Burkina Faso", "Ghana",
                "Kamerun", "Nigeria","Südafrika", "Zentralafrikanische Republik",

                // Asien
                "Volksrepublik China", "Indien", "Indonesien", "Pakistan",
                "Palau", "Papua-Neuguinea", "Syrien", "Vietnam",

            // Osteuropa
            "Albanien", "Kroatien", "Polen", "Russland",

            // Lateinamerika
            "Argentinien", "Brasilien", "Chile", "Peru",
            "Venezuela",

            // Westeuropa
            "Deutschland", "Frankreich", "Türkei", "Vereinigte Staaten", 
            "Vereinigtes Königreich");


            Assert.AreEqual(30, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-ha3"));
        }

        [Test]
        [Order(502)]
        public void TestAddSeatsToSR()
        {
            // Afrika
            _context.Fluent.ForCommittee("munbw22-sr").AddSeatByCountryName("Gabun");
            _context.Fluent.ForCommittee("munbw22-sr").AddSeatByCountryName("Ghana");
            _context.Fluent.ForCommittee("munbw22-sr").AddSeatByCountryName("Kenia");

            // Asien
            _context.Fluent.ForCommittee("munbw22-sr").AddSeatByCountryName("Volksrepublik China");
            _context.Fluent.ForCommittee("munbw22-sr").AddSeatByCountryName("Indien");
            _context.Fluent.ForCommittee("munbw22-sr").AddSeatByCountryName("Vereinigte Arabische Emirate");

            // Osteuropa
            _context.Fluent.ForCommittee("munbw22-sr").AddSeatByCountryName("Albanien");
            _context.Fluent.ForCommittee("munbw22-sr").AddSeatByCountryName("Russland");

            // Lateinamerika
            _context.Fluent.ForCommittee("munbw22-sr").AddSeatByCountryName("Brasilien");
            _context.Fluent.ForCommittee("munbw22-sr").AddSeatByCountryName("Mexiko");

            // Westeuropa
            _context.Fluent.ForCommittee("munbw22-sr").AddSeatByCountryName("Frankreich");
            _context.Fluent.ForCommittee("munbw22-sr").AddSeatByCountryName("Irland");
            _context.Fluent.ForCommittee("munbw22-sr").AddSeatByCountryName("Norwegen");
            _context.Fluent.ForCommittee("munbw22-sr").AddSeatByCountryName("Vereinigte Staaten");
            _context.Fluent.ForCommittee("munbw22-sr").AddSeatByCountryName("Vereinigtes Königreich");


            Assert.AreEqual(15, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-sr"));
        }

        [Test]
        [Order(503)]
        public void TestAddSeatsToKFK()
        {
            // Afrika
            _context.Fluent.ForCommittee("munbw22-kfk").AddSeatByCountryName("Ägypten");
            _context.Fluent.ForCommittee("munbw22-kfk").AddSeatByCountryName("Äthiopien");
            _context.Fluent.ForCommittee("munbw22-kfk").AddSeatByCountryName("Kenia");
            _context.Fluent.ForCommittee("munbw22-kfk").AddSeatByCountryName("Nigeria");
            _context.Fluent.ForCommittee("munbw22-kfk").AddSeatByCountryName("Ruanda");
            _context.Fluent.ForCommittee("munbw22-kfk").AddSeatByCountryName("Südafrika");

            // Asien
            _context.Fluent.ForCommittee("munbw22-kfk").AddSeatByCountryName("Bangladesch");
            _context.Fluent.ForCommittee("munbw22-kfk").AddSeatByCountryName("Volksrepublik China");
            _context.Fluent.ForCommittee("munbw22-kfk").AddSeatByCountryName("Indien");
            _context.Fluent.ForCommittee("munbw22-kfk").AddSeatByCountryName("Japan");
            _context.Fluent.ForCommittee("munbw22-kfk").AddSeatByCountryName("Jemen");
            _context.Fluent.ForCommittee("munbw22-kfk").AddSeatByCountryName("Myanmar");
            _context.Fluent.ForCommittee("munbw22-kfk").AddSeatByCountryName("Pakistan");
            _context.Fluent.ForCommittee("munbw22-kfk").AddSeatByCountryName("Südkorea");
            _context.Fluent.ForCommittee("munbw22-kfk").AddSeatByCountryName("Zypern");

            // Osteuropa
            _context.Fluent.ForCommittee("munbw22-kfk").AddSeatByCountryName("Russland");
            _context.Fluent.ForCommittee("munbw22-kfk").AddSeatByCountryName("Ukraine");

            // Lateinamerika
            _context.Fluent.ForCommittee("munbw22-kfk").AddSeatByCountryName("Brasilien");
            _context.Fluent.ForCommittee("munbw22-kfk").AddSeatByCountryName("Costa Rica");
            _context.Fluent.ForCommittee("munbw22-kfk").AddSeatByCountryName("Haiti");
            _context.Fluent.ForCommittee("munbw22-kfk").AddSeatByCountryName("Kolumbien");
            _context.Fluent.ForCommittee("munbw22-kfk").AddSeatByCountryName("Venezuela");

            // Westeuropa
            _context.Fluent.ForCommittee("munbw22-kfk").AddSeatByCountryName("Frankreich");
            _context.Fluent.ForCommittee("munbw22-kfk").AddSeatByCountryName("Kanada");
            _context.Fluent.ForCommittee("munbw22-kfk").AddSeatByCountryName("Niederlande");
            _context.Fluent.ForCommittee("munbw22-kfk").AddSeatByCountryName("Norwegen");
            _context.Fluent.ForCommittee("munbw22-kfk").AddSeatByCountryName("Schweiz");
            _context.Fluent.ForCommittee("munbw22-kfk").AddSeatByCountryName("Vereinigte Staaten");
            _context.Fluent.ForCommittee("munbw22-kfk").AddSeatByCountryName("Vereinigtes Königreich");

            Assert.AreEqual(29, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-kfk"));
        }

        [Test]
        [Order(504)]
        public void TestAddSeatsToWiSo()
        {
            // Afrika
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Ägypten");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Algerien");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Angola");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Äthiopien");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Elfenbeinküste");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Gabun");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Ghana");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Kamerun");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Kenia");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Demokratische Republik Kongo");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Republik Kongo");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Libyen");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Madagaskar");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Nigeria");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Tunesien");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Uganda");

            // Asien
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Bangladesch");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Volksrepublik China");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Indien");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Indonesien");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Japan");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Kasachstan");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Katar");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Philippinen");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Saudi-Arabien");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Singapur");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Südkorea");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Thailand");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Vereinigte Arabische Emirate");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Vietnam");

            // Osteuropa
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Kroatien");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Lettland");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Polen");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Rumänien");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Russland");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Ukraine");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Ungarn");

            // Lateinamerika
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Argentinien");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Brasilien");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Chile");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Ecuador");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Jamaika");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Kolumbien");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Kuba");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Mexiko");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Nicaragua");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Peru");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Uruguay");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Venezuela");

            // Westeuropa
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Australien");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Deutschland");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Frankreich");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Irland");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Israel");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Italien");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Kanada");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Niederlande");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Norwegen");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Schweden");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Schweiz");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Türkei");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Vereinigte Staaten");
            _context.Fluent.ForCommittee("munbw22-wiso").AddSeatByCountryName("Vereinigtes Königreich");



            Assert.AreEqual(63, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-wiso"));
        }

        [Test]
        [Order(505)]
        public void TestAddSeatsToKBE()
        {
            // Afrika
            _context.Fluent.ForCommittee("munbw22-kbe").AddSeatByCountryName("Ägypten");
            _context.Fluent.ForCommittee("munbw22-kbe").AddSeatByCountryName("Äthiopien");
            _context.Fluent.ForCommittee("munbw22-kbe").AddSeatByCountryName("Burkina Faso");
            _context.Fluent.ForCommittee("munbw22-kbe").AddSeatByCountryName("Elfenbeinküste");
            _context.Fluent.ForCommittee("munbw22-kbe").AddSeatByCountryName("Demokratische Republik Kongo");
            _context.Fluent.ForCommittee("munbw22-kbe").AddSeatByCountryName("Libyen");

            // Asien
            _context.Fluent.ForCommittee("munbw22-kbe").AddSeatByCountryName("Bangladesch");
            _context.Fluent.ForCommittee("munbw22-kbe").AddSeatByCountryName("Volksrepublik China");
            _context.Fluent.ForCommittee("munbw22-kbe").AddSeatByCountryName("Indien");
            _context.Fluent.ForCommittee("munbw22-kbe").AddSeatByCountryName("Iran");
            _context.Fluent.ForCommittee("munbw22-kbe").AddSeatByCountryName("Japan");
            _context.Fluent.ForCommittee("munbw22-kbe").AddSeatByCountryName("Philippinen");

            // Osteuropa
            _context.Fluent.ForCommittee("munbw22-kbe").AddSeatByCountryName("Russland");
            _context.Fluent.ForCommittee("munbw22-kbe").AddSeatByCountryName("Ukraine");

            // Lateinamerika
            _context.Fluent.ForCommittee("munbw22-kbe").AddSeatByCountryName("Argentinien");
            _context.Fluent.ForCommittee("munbw22-kbe").AddSeatByCountryName("Haiti");
            _context.Fluent.ForCommittee("munbw22-kbe").AddSeatByCountryName("Kolumbien");
            _context.Fluent.ForCommittee("munbw22-kbe").AddSeatByCountryName("Mexiko");

            // Westeuropa
            _context.Fluent.ForCommittee("munbw22-kbe").AddSeatByCountryName("Australien");
            _context.Fluent.ForCommittee("munbw22-kbe").AddSeatByCountryName("Israel");
            _context.Fluent.ForCommittee("munbw22-kbe").AddSeatByCountryName("Niederlande");
            _context.Fluent.ForCommittee("munbw22-kbe").AddSeatByCountryName("Türkei");
            _context.Fluent.ForCommittee("munbw22-kbe").AddSeatByCountryName("Vereinigte Staaten");



            Assert.AreEqual(23, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-kbe"));
        }

        [Test]
        [Order(506)]
        public void TestAddSeatsToIOM()
        {
            // Afrika
            _context.Fluent.ForCommittee("munbw22-iom").AddSeatByCountryName("Algerien");
            _context.Fluent.ForCommittee("munbw22-iom").AddSeatByCountryName("Eritrea");
            _context.Fluent.ForCommittee("munbw22-iom").AddSeatByCountryName("Libyen");
            _context.Fluent.ForCommittee("munbw22-iom").AddSeatByCountryName("Ruanda");
            _context.Fluent.ForCommittee("munbw22-iom").AddSeatByCountryName("Südafrika");
            _context.Fluent.ForCommittee("munbw22-iom").AddSeatByCountryName("Sudan");
            _context.Fluent.ForCommittee("munbw22-iom").AddSeatByCountryName("Südsudan");
            _context.Fluent.ForCommittee("munbw22-iom").AddSeatByCountryName("Uganda");

            // Asien
            _context.Fluent.ForCommittee("munbw22-iom").AddSeatByCountryName("Bangladesch");
            _context.Fluent.ForCommittee("munbw22-iom").AddSeatByCountryName("Volksrepublik China");
            _context.Fluent.ForCommittee("munbw22-iom").AddSeatByCountryName("Iran");
            _context.Fluent.ForCommittee("munbw22-iom").AddSeatByCountryName("Japan");
            _context.Fluent.ForCommittee("munbw22-iom").AddSeatByCountryName("Jemen");
            _context.Fluent.ForCommittee("munbw22-iom").AddSeatByCountryName("Myanmar");
            _context.Fluent.ForCommittee("munbw22-iom").AddSeatByCountryName("Pakistan");
            _context.Fluent.ForCommittee("munbw22-iom").AddSeatByCountryName("Usbekistan");

            // Osteuropa
            _context.Fluent.ForCommittee("munbw22-iom").AddSeatByCountryName("Bosnien und Herzegowina");
            _context.Fluent.ForCommittee("munbw22-iom").AddSeatByCountryName("Russland");
            _context.Fluent.ForCommittee("munbw22-iom").AddSeatByCountryName("Ukraine");
            _context.Fluent.ForCommittee("munbw22-iom").AddSeatByCountryName("Belarus");

            // Lateinamerika
            _context.Fluent.ForCommittee("munbw22-iom").AddSeatByCountryName("Costa Rica");
            _context.Fluent.ForCommittee("munbw22-iom").AddSeatByCountryName("Kolumbien");
            _context.Fluent.ForCommittee("munbw22-iom").AddSeatByCountryName("Mexiko");
            _context.Fluent.ForCommittee("munbw22-iom").AddSeatByCountryName("Nicaragua");
            _context.Fluent.ForCommittee("munbw22-iom").AddSeatByCountryName("Venezuela");

            // Westeuropa
            _context.Fluent.ForCommittee("munbw22-iom").AddSeatByCountryName("Deutschland");
            _context.Fluent.ForCommittee("munbw22-iom").AddSeatByCountryName("Italien");
            _context.Fluent.ForCommittee("munbw22-iom").AddSeatByCountryName("Kanada");
            _context.Fluent.ForCommittee("munbw22-iom").AddSeatByCountryName("Türkei");
            _context.Fluent.ForCommittee("munbw22-iom").AddSeatByCountryName("Vereinigte Staaten");


            Assert.AreEqual(30, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-iom"));
        }

        [Test]
        [Order(507)]
        public void TestAddSeatsToKK()
        {
            _context.Fluent.ForCommittee("munbw22-kk").AddSeatsByCountryNames("Ägypten", 
                "Algerien",  "Angola", "Äthiopien", "Burkina Faso", "Elfenbeinküste",
                "Ghana", "Kamerun", "Kenia", "Demokratische Republik Kongo", "Madagaskar", "Mosambik", "Nigeria",
                "Ruanda", "Seychellen", "Südafrika", "Sudan", "Tschad", "Tunesien", "Uganda",

                "Bangladesch", "Volksrepublik China", "Fidschi", "Indien",
                "Indonesien", "Iran", "Japan", "Kasachstan",
                "Katar", "Myanmar", "Pakistan", "Palau",
                "Papua-Neuguinea", "Philippinen", "Samoa", "Saudi-Arabien",
                "Südkorea", "Thailand", "Vereinigte Arabische Emirate", "Vietnam",

                "Albanien", "Estland", "Kroatien", "Polen",
                "Rumänien", "Russland", "Ungarn", "Belarus",

                "Argentinien", "Brasilien", "Chile", "Costa Rica",
                "Dominikanische Republik", "Ecuador", "Haiti", "Jamaika",
                "Kuba", "Mexiko", "Trinidad und Tobago", "Uruguay",

                "Australien", "Deutschland", "Frankreich", "Italien",
                "Kanada", "Niederlande", "Norwegen", "Schweden",
                "Türkei", "Vereinigte Staaten", "Vereinigtes Königreich"
                );

            Assert.AreEqual(71, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-kk"));
        }

        [Test]
        [Order(508)]
        public void TestAddSeatsToMRR()
        {
            _context.Fluent.ForCommittee("munbw22-mrr").AddSeatsByCountryNames("Algerien", "Burkina Faso");

            Assert.AreEqual(2, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-mrr"));
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
            // Afrika
            _ = _context.Fluent.ForConference("munbw22")
                .AddDelegation(options => options.WithName("Ägypten").WithCountry("Ägybpten").InsideAnyCommittee());

            _ = _context.Fluent.ForConference("munbw22")
                .AddDelegation(del => del.WithName("Algerien").WithCountry("Algerien").InsideCommittee("munbw22-gv", "munbw22-wiso", "munbw22-iom", "munbw-kk"));

            _ = _context.Fluent.ForConference("munbw22")
                .AddDelegation(del => del.WithName("Algerien (online)").WithCountry("Algerien").InsideCommittee("munbw22-mrr"));

            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Angola");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Äthiopien");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Burkina Faso");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Elfenbeinküste");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Eritrea");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Gabun");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Ghana");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Kamerun");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Kenia");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Demokratische Republik Kongo");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Republik Kongo");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Libyen");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Madagaskar");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Mosambik");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Nigeria");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Ruanda");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Seychellen");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Südafrika");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Sudan");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Südsudan");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Tschad");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Tunesien");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Uganda");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Zentralafrikanische Republik");

            // Asien
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Bangladesch");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Volksrepublik China");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Fidschi");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Indien");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Indonesien");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Iran");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Japan");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Jemen");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Kasachstan");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Katar");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Myanmar");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Pakistan");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Palau");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Papua-Neuguinea");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Philippinen");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Samoa");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Saudi-Arabien");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Singapur");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Südkorea");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Syrien");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Thailand");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Usbekistan");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Vereinigte Arabische Emirate");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Vietnam");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Zypern");

            // Osteuropa
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Albanien");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Bosnien und Herzegowina");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Estland");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Kroatien");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Lettland");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Polen");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Rumänien");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Russland");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Ukraine");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Ungarn");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Belarus");

            // Lateinamerika
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Argentinien");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Brasilien");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Chile");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Costa Rica");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Dominikanische Republik");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Ecuador");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Haiti");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Jamaika");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Kolumbien");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Kuba");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Mexiko");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Nicaragua");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Peru");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Trinidad und Tobago");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Uruguay");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Venezuela");

            // Westeuropa
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Australien");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Deutschland");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Frankreich");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Irland");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Israel");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Italien");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Kanada");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Niederlande");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Norwegen");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Schweden");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Schweiz");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Türkei");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Vereinigte Staaten");
            _context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Vereinigtes Königreich");



            var allDelegations = _context.Delegations
                .Include(n => n.ChildDelegations)
                .Include(n => n.Roles)
                .Where(n => n.Conference.ConferenceId == "munbw22")
                .ToList();

            Assert.AreEqual(93, allDelegations.Count);

            var delegationDeutschland = allDelegations.FirstOrDefault(n => n.Name == "Deutschland");
            Assert.NotNull(delegationDeutschland, "Expected to find a Delegation named Deutschland but none was found!");
            // Vertreten in 5 Gremien
            // + 1 Lehrkraft kommt in einem späteren Test hinzu
            Assert.AreEqual(5, delegationDeutschland.Roles.Count);
            Assert.IsTrue(_context.Delegations.Any(n => n.DelegationId == "munbw22-deutschland"));
        }

        [Test]
        [Order(512)]
        public void TestAddCostRuleForMrr()
        {
            _context.Fluent.ForCommittee("munbw22-mrr").AddCostRule(50, "Preis für ein Online Gremium");
            Assert.AreEqual(1, _context.ConferenceParticipationCostRules.Count());
        }

        [Test]
        [Order(513)]
        public void TestAddTeacherToDelegationDeutschland()
        {
            var deutschland =
                _context.Delegations.FirstOrDefault(n =>
                    n.Name == "Deutschland" && n.Conference.ConferenceId == "munbw22");
            Assert.NotNull(deutschland);

            var teacher = new ConferenceDelegateRole()
            {
                Conference = _context.Conferences.Find("munbw22"),
                ApplicationState = EApplicationStates.Closed,
                Committee = null,
                DelegateCountry = null,
                DelegateType = "Lehrkraft",
                Delegation = deutschland,
                IsDelegationLeader = true,
                RoleName = "Lehrkraft",
                RoleFullName = "Lehrkraft"
            };

            _context.Delegates.Add(teacher);
            _context.SaveChanges();

            var hasLehrkraft = _context.Delegations.Any(n =>
                n.DelegationId == deutschland.DelegationId && n.Roles.Any(a => a.RoleName == "Lehrkraft"));
            Assert.IsTrue(hasLehrkraft);
        }

        [Test]
        [Order(514)]
        public void TestAddCostRuleForTeacher()
        {
            _context.Fluent.ForConference("munbw22").AddCostRuleForRolesOfSubType("Lehrkraft", 0, "Preis für Lehrkraft");
            var lehrkraftRole = _context.Delegates.FirstOrDefault(n => n.RoleName == "Lehrkraft");

            Assert.NotNull(lehrkraftRole);
            var costs = _context.Fluent.ForConference("munbw22").GetCostOfRole(lehrkraftRole.RoleId);
            Assert.AreEqual(0, costs);

        }

        #endregion

        // ID 600 Test Registration/Application
        #region Application Phase (Anmeldephase) - 600

        [Test]
        [Order(600)]
        [Description("This Test should show how the application phase is created within a conference.")]
        public void TestSetupTheApplicationPhase()
        {
            var conference = _context.Conferences.Find("munbw22");
            Assert.NotNull(conference);

            var options = new ConferenceApplicationOptions()
            {
                Conference = conference,
                AllowCountryWishApplication = false,
                AllowDelegationApplication = true,
                AllowDelegationWishApplication = true,
                AllowRoleApplication = true,
                ApplicationEndDate = new DateTime(2022, 2, 20, 12, 0, 0),
                ApplicationStartDate = new DateTime(2021, 12, 12, 12, 0, 0),
                Formulas = new List<ConferenceApplicationFormula>(),
                IsActive = true
            };

            var formula = new ConferenceApplicationFormula()
            {
                Options = options,
                Title = "Anmeldung",
                PostContent =
                    "Die Anmeldungen werden am 20.02.2022 abgeschlossen. Dann Erfahren Sie, welche Delegation und Rolle Sie bekommen",
                PreContent = "Die Anmeldung bei Model United Nations Baden-Würrtemberg...",
                RequiresAddress = true,
                RequiresName = true,
                MaxWishes = 3
            };

            formula.Fields = new List<ConferenceApplicationField>();

            formula.Fields.Add(new ConferenceApplicationField()
            {
                IsRequired = true,
                DefaultValue = null,
                FieldName = "Motivation",
                FieldDescription = "Tragen Sie hier ein, warum Sie unbedingt diese Rolle haben möchten.",
                FieldType = ConferenceApplicationFieldTypes.MultiLineText,
                Forumula = formula
            });

            formula.Fields.Add(new ConferenceApplicationField()
            {
                IsRequired = true,
                DefaultValue = null,
                FieldName = "Erfahrung",
                FieldDescription = "Tragen Sie hier Ihre Erfahrung rund um das Thema Model United Nations und anderweitig ein.",
                FieldType = ConferenceApplicationFieldTypes.MultiLineText,
                Forumula = formula
            });

            _context.ConferenceApplicationFormulas.Add(formula);
            _context.SaveChanges();
            Assert.IsTrue(_context.ConferenceApplicationFormulas.Any(n => n.Options.Conference.ConferenceId == "munbw22"));
            var formulaReload = _context.ConferenceApplicationFormulas
                .Include(n => n.Fields)
                .FirstOrDefault(n => n.Options.Conference.ConferenceId == "munbw22" &&
                n.FormulaType == ConferenceApplicationFormulaTypes.Delegation);

            Assert.NotNull(formulaReload);
            Assert.NotNull(formulaReload.Fields);
            Assert.AreEqual(2, formulaReload.Fields.Count);
            Assert.IsTrue(_context.ConferenceApplicationOptions.Any(n => n.ConferenceId == "munbw22"));
        }

        [Test]
        [Order(610)]
        [Description("This test sets the application phase on all the roles that are linked to a country inside the Delegation named Deutschland.")]
        public void TestSetApplicationStateOnDelegations()
        {
            var delegations = _context.Delegations
                .Include(n => n.Roles)
                .ThenInclude(n => n.DelegateCountry)
                .Where(n => n.Conference.ConferenceId == "munbw22");


            foreach(var delegation in delegations)
            {
                foreach (var role in delegation.Roles)
                {
                    if (role.DelegateCountry is not null)
                        role.ApplicationState = EApplicationStates.DelegationApplication;
                }
            }

            var result = _context.SaveChanges();
        }

        [Test]
        [Order(611)]
        [Description("This test show how a delegation Application can be created, that is targeting three different Delegations.")]
        public void TestCreateDelegationApplication()
        {

            var application = _context.Fluent
                .ForConference("munbw22")
                .CreateDelegationApplication()
                .WithAuthor("muricaboi")
                .WithPreferedDelegationByName("Deutschland")
                .WithPreferedDelegationByName("Frankreich")
                .WithPreferedDelegationByName("Vereinigte Staaten")
                .WithFieldInput("Motivation", "Wir sind sehr Motiviert bei dieser Konferenz dabei zu sein :)")
                .Submit();


            Assert.NotNull(application);
            Assert.AreEqual(1, _context.DelegationApplications.Count());

            var applications = _context.DelegationApplications
                .Where(n => n.DelegationWishes
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

            var priceDeutschland = _context.Fluent.ForConference("munbw22").CostsOfDelegationByName("Deutschland");

            // Deutschland ist in 5 Gremien, Preis sollte 5 * 50
            // Basispreis: 70 Euro
            // Preis für Online-Gremien: 50 Euro
            Assert.AreEqual(5 * 70, priceDeutschland.Costs.Sum(a => a.Cost));
        }

        

        #endregion

        // ID 700 Test Website

    }
}
