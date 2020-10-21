using System;
using Microsoft.EntityFrameworkCore;
using MUNityCore.DataHandlers.EntityFramework;
using MUNityCore.Models.Conference;
using MUNityCore.Models.Organisation;
using MUNityCore.Services;
using NUnit.Framework;

namespace MUNityTest.Service
{

    /// <summary>
    /// A lot of this TestCases are doublicated inside WorkflowTests/ConferenceWorkflowTest
    /// </summary>
    [TestFixture]
    public class ConferenceServiceTest
    {
        private static MunCoreContext _context;

        private Organisation _organisation;
        private Project _project;
        private Conference _conference;

        [OneTimeSetUp]
        public void Setup()
        {
            Console.WriteLine("Setup Test Environment");
            // Datenbank für den Test erzeugen und falls vorhanden erst einmal leeren und neu erstellen!
            var optionsBuilder = new DbContextOptionsBuilder<MunCoreContext>();
            optionsBuilder.UseSqlite("Data Source=test_conferenceservice.db");
            _context = new MunCoreContext(optionsBuilder.Options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        [Test]
        [Order(1)]
        [Author("Peer Conradi")]
        [Description("Test initializing ConferenceService implementation")]
        public void TestInitializingConferenceService()
        {
            var service = new ConferenceService(_context);
            Assert.NotNull(service);
            Assert.NotNull(service as IConferenceService);
        }

        [Test]
        [Order(2)]
        [Author("Peer Conradi")]
        [Description("First create an organisation to be able to create a project")]
        public void TestCreateOrganisation()
        {
            var organisationService = new OrganisationService(_context);
            var organisation = organisationService.CreateOrganisation("Deutsche Model United Nation e.V.", "dmun e.V.");
            Assert.NotNull(organisation);
            _organisation = organisation;
        }

        [Test]
        [Order(3)]
        [Author("Peer Conradi")]
        [Description("Test creating a project")]
        public void TestCreateProject()
        {
            var service = new ConferenceService(_context);
            var project = service.CreateProject("Model United Nations Baden-Würrtemberg", "MUNBW", _organisation);
            Assert.NotNull(project);
            _project = project;
        }

        [Test]
        [Order(4)]
        [Author("Peer Conradi")]
        [Description("Test Creating a conference.")]
        public void TestCreateConference()
        {
            var service = new ConferenceService(_context);
            var conference = service.CreateConference("MUN Baden-Würrtemberg 2021",
                "Model United Nations Baden-Würrtemberg 2021",
                "MUNBW 2021", _project);
            Assert.NotNull(conference);
            _conference = conference;
        }

        [Test]
        [Order(5)]
        [Author("Peer Conradi")]
        [Description("Add a committee to the conference")]
        public void TestCreateCommittee()
        {
            var service = new ConferenceService(_context);
            var committee = service.CreateCommittee(_conference, "Generalversammlung", "Generalversammlung", "GV");
            Assert.NotNull(committee);
            Assert.AreEqual(1, _conference.Committees.Count);
        }



    }
}