using Microsoft.EntityFrameworkCore;
using MUNity.Database.Context;
using MUNity.Database.Models.Conference;
using MUNity.Database.Models.Organization;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNityDatabaseTest.ConferenceTests
{
    [TestFixture]
    public class ConferenceTest
    {

        private MunityContext _context;

        [OneTimeSetUp]
        public void Setup()
        {
            // Datenbank für den Test erzeugen und falls vorhanden erst einmal leeren und neu erstellen!
            var optionsBuilder = new DbContextOptionsBuilder<MunityContext>();
            optionsBuilder.UseSqlite("Data Source=test_conference.db");
            _context = new MunityContext(optionsBuilder.Options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        [Test]
        [Order(1)]
        public void TestCreateOrganization()
        {
            var organization = new Organization()
            {
                OrganizationName = "Deutsche Model United Nations e.V.",
                OrganizationShort = "DMUN e.V."
            };
            _context.Organizations.Add(organization);
            _context.SaveChanges();
            Assert.AreEqual(1, _context.Organizations.Count());
        }

        [Test]
        [Order(2)]
        public void TestOrganizationIsNotNull()
        {
            var orga = _context.Organizations.FirstOrDefault();
            Assert.NotNull(orga);
        }

        [Test]
        [Order(3)]
        public void TestOrganizationHasId()
        {
            var orga = _context.Organizations.FirstOrDefault();
            Assert.IsFalse(string.IsNullOrEmpty(orga.OrganizationId));
            //Assert.IsNull(orga.OrganizationTimestamp);
        }

        [Test]
        [Order(4)]
        public void TestCreateProject()
        {
            var orga = _context.Organizations.FirstOrDefault();
            var project = new Project()
            {
                ProjectName = "Model United Nations Schleswig-Holstein",
                ProjectOrganization = orga,
                ProjectShort = "MUN-SH",
            };
            _context.Projects.Add(project);
            _context.SaveChanges();
        }

        [Test]
        [Order(5)]
        public void TestProjectHasId()
        {
            var project = _context.Projects.FirstOrDefault();
            Assert.IsFalse(string.IsNullOrEmpty(project.ProjectId));
        }

        [Test]
        [Order(6)]
        public void TestOrganizationHoldsProject()
        {
            var organization = _context.Organizations.Include(n => n.Projects).FirstOrDefault();
            Assert.AreEqual(1, organization.Projects.Count);
        }

        [Test]
        [Order(7)]
        public void TestCreateConference()
        {
            var project = _context.Projects.FirstOrDefault();
            var conference = new Conference()
            {
                ConferenceProject = project,
                ConferenceShort = "MUN-SH 2022",
                Name = "MUN Schleswig-Holstein 2022",
                FullName = "Model United Nations Schleswig-Holstein 2022"
            };
            _context.Conferences.Add(conference);
            _context.SaveChanges();
            Assert.AreEqual(1, _context.Conferences.Count());
        }

        [Test]
        [Order(8)]
        public void TestProjectContainsConference()
        {
            var project = _context.Projects.Include(n => n.Conferences).FirstOrDefault();
            Assert.AreEqual(1, project.Conferences.Count);
        }

        [Test]
        [Order(9)]
        public void TestConferenceHasAnId()
        {
            var conference = _context.Conferences.FirstOrDefault();
            Assert.IsFalse(string.IsNullOrEmpty(conference.ConferenceId));
        }

        [Test]
        [Order(10)]
        public void TestAddACommittee()
        {
            var conference = _context.Conferences.FirstOrDefault();
            var committee = new Committee()
            {
                Article = "die",
                CommitteeShort = "GV",
                Conference = conference,
                FullName = "Generalversammlung",
                Name = "Generalversammlung"
            };
            _context.Committees.Add(committee);
            _context.SaveChanges();
        }

        [Test]
        [Order(11)]
        public void TestConferenceContainsCommittee()
        {
            var conference = _context.Conferences.Include(n => n.Committees).FirstOrDefault();
            Assert.AreEqual(1, conference.Committees.Count);
        }

        [Test]
        [Order(12)]
        public void TestConferenceHasId()
        {
            var conference = _context.Conferences.FirstOrDefault();
            Assert.IsFalse(string.IsNullOrEmpty(conference.ConferenceId));
        }
    }
}
