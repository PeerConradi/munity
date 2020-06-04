using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MUNityAngular.DataHandlers.EntityFramework;
using MUNityAngular.Models.Conference;
using MUNityAngular.Models.Organisation;
using NUnit.Framework;
using MUNityAngular.Services;

namespace MUNityTest.Service
{
    [TestFixture]
    [Author("Peer Conradi")]
    [Description("Test the Conference Service implementation with a SqLite Database")]
    public class ConferenceServiceTest
    {
        private static ConferenceContext _context;
        private static Organisation _organisation;
        private static Project _project;
        private static Conference _conference;

        [SetUp]
        public void Setup()
        {
            // Datenbank für den Test erzeugen und falls vorhanden erst einmal leeren und neu erstellen!
            var optionsBuilder = new DbContextOptionsBuilder<ConferenceContext>();
            optionsBuilder.UseSqlite("Data Source=test.db");
            _context = new ConferenceContext(optionsBuilder.Options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        [Test]
        [Order(1)]
        public async Task TestCreateAndGetOrganisation()
        {
            var service = new ConferenceService(_context);
            var result = service.CreateOrganisation("Test Organisations", "TO");
            var dbResult = await service.GetOrganisation(result.OrganisationId);
            Assert.AreEqual("Test Organisations", dbResult.OrganisationName);
            _organisation = dbResult;
        }

        [Test]
        [Order(2)]
        public async Task TestCreateAndGetProject()
        {
            if (_organisation == null)
                Assert.Fail("This test needs an organisation created before it can run!");

            var service = new ConferenceService(_context);
            var result = service.CreateProject("Test Konferenz", "TK", _organisation);
            var dbResult = await service.GetProject(result.ProjectId);
            Assert.AreEqual("Test Konferenz", dbResult.ProjectName);
            _project = dbResult;
        }

        [Test]
        [Order(3)]
        [Author("Peer Conradi")]
        [Description("Test that a conference can be created")]
        public async Task TestCreateAndGetConference()
        {
            if (_project == null)
            {
                Assert.Fail("This test needs a created project to run!");
            }
            var service = new ConferenceService(_context);
            var conference = service.CreateConference("Test Konferenz 2021", 
                "Konferenz zum Testen 2021" , "TK 2021", _project);
            var result = await service.GetConference(conference.ConferenceId);
            Assert.NotNull(result);
            _conference = conference;
        }

        [Test]
        [Order(4)]
        [Author("Peer Conradi")]
        [Description("Test that a committee can be added to a Conference")]
        public async Task TestCreateAndGetCommittee()
        {
            if (_conference == null)
                Assert.Fail("This test needs a conference before it can run!");

            var service = new ConferenceService(_context);
            var committee = service.CreateCommittee(_conference, "Generalversammlung",
                "Generalversammlung", "GV");
            var result = await service.GetCommittee(committee.CommitteeId);
            Assert.NotNull(committee);
            var conference = await service.GetConference(_conference.ConferenceId);
            Assert.AreEqual(1, conference.Committees.Count);
        }

        
    }
}