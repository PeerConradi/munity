using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MUNity.Database.Context;
using NUnit.Framework;
using MUNity.Database.Extensions;

namespace MUNity.Database.Test.MUNBW22Tests
{

    public class CompleteTestRunMUNBW22
    {
        private MunityContext _context;



        [OneTimeSetUp]
        public void SetupDatabase()
        {
            var tmpContext = MunityContext.FromSqlLite("testmunbw");
            tmpContext.Database.EnsureDeleted();
            tmpContext.Database.EnsureCreated();
        }

        [SetUp]
        public void ReloadContextToClearBuffer()
        {
            
            _context = MunityContext.FromSqlLite("testmunbw");
        }

        // Id 0-50 Tests
        #region Plattoform Setup

        [Test]
        [Order(1)]
        public void TestSetupCountries()
        {
            var recaff = _context.AddBaseCountries();
            Assert.NotZero(recaff);
            Assert.NotZero(_context.Countries.Count());
            Assert.NotZero(_context.CountryNameTranslations.Count());
        }

        #endregion

        // ID 51 - 99 Tests
        #region Account Setup

        #endregion

        #region Organization Setup
        [Test]
        [Order(100)]
        public void TestAddDMUNOrganiozation()
        {
            var orga = _context.AddOrganization(options =>
                options.WithName("Deutsche Model United Nations e.V.")
                    .WithShort("DMUN e.V."));
            Assert.NotNull(orga);
            Assert.AreEqual(1, _context.Organizations.Count());
            Assert.IsTrue(_context.Organizations.Any(n => n.OrganizationId == "dmunev"));
        }
        #endregion

        // Id 200 Tests
        #region Project MUNBW Setup
        [Test]
        [Order(200)]
        public void TestAddMUNBWProject()
        {
            var project = _context.AddProject(options =>
                options.WithShort("MUNBW")
                    .WithName("Model United Nations Baden-Würrtemberg")
                    .WithOrganization("dmunev"));
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

        // Id 300 Tests
        #region Basic Conference Setup
        [Test]
        [Order(300)]
        public void TestAddConference()
        {
            var conference = _context.AddConference(options =>
                options.WithShort("MUNBW 22")
                    .WithName("Model United Nations Baden-Würrtemberg 2022")
                    .WithFullName("Model United Nations Baden-Würrtemberg 2022")
                    .WithProject("munbw"));
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

            var committee = conference.AddCommittee(gv =>
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
            _context.SaveChanges();
            Assert.AreEqual(2, _context.Committees.Count());
            Assert.IsTrue(_context.Committees.Any(n => n.CommitteeId == "munbw22-gv"));
            Assert.IsTrue(_context.Committees.Any(n => n.CommitteeId == "munbw22-ha3"));
            Assert.AreEqual(5, _context.CommitteeTopics.Count());
        }
        #endregion

        // Id 400 Tests
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
            Assert.AreEqual(1, _context.TeamRoles.Count());
        }

        [Test]
        [Order(401)]
        public void TestAddErweiterteProjektleitung()
        {
            var conference = _context.Conferences.FirstOrDefault(n => n.ConferenceId == "munbw22");
            var projektleitung = _context.TeamRoles.FirstOrDefault(n =>
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
            Assert.AreEqual(13, _context.TeamRoles.Count());
        }

        [Test]
        [Order(402)]
        public void TestConferenceHas13Roles()
        {
            var conference = _context.Conferences.Include(n => n.Roles)
                .FirstOrDefault(n => n.ConferenceId == "munbw22");
            Assert.AreEqual(13, conference.Roles.Count);
        }

        #endregion

    }
}
