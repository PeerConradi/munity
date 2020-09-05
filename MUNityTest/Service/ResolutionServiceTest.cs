using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Driver.Core.Operations;
using MUNityAngular.DataHandlers;
using MUNityAngular.DataHandlers.EntityFramework;
using MUNityAngular.Models.Resolution.V2;
using MUNityAngular.Services;
using NUnit.Framework;

namespace MUNityTest.Service
{
    [TestFixture]
    public class ResolutionServiceTest
    {
        private MunityContext _munityContext;

        private class MongoTestString : IMunityMongoDatabaseSettings
        {
            public string ResolutionCollectionName { get; set; }
            public string ConnectionString { get; set; }
            public string DatabaseName { get; set; }
            public string PresenceCollectionName { get; set; }

            public MongoTestString()
            {
                ResolutionCollectionName = "TestResolutions";
                ConnectionString = "mongodb://localhost:27017";
                DatabaseName = "Munity_Tests";
                PresenceCollectionName = "NotNeeded";
            }
        }

        [OneTimeSetUp]
        public void Setup()
        {
            // Datenbank für den Test erzeugen und falls vorhanden erst einmal leeren und neu erstellen!
            var optionsBuilder = new DbContextOptionsBuilder<MunityContext>();
            optionsBuilder.UseSqlite("Data Source=test_organisation.db");
            _munityContext = new MunityContext(optionsBuilder.Options);
            _munityContext.Database.EnsureDeleted();
            _munityContext.Database.EnsureCreated();

            // Clean the Test Database before doing anything
            var settings = new MongoTestString();
            var client = new MongoClient(settings.ConnectionString);
            client.DropDatabase(settings.DatabaseName);
        }

        [Test]
        public async Task TestCreateResolution()
        {
            var service = new NewResolutionService(_munityContext, new MongoTestString());
            var resolution = await service.CreateResolution("Test");
            Assert.NotNull(resolution);
            Assert.AreEqual("Test", resolution.Header.Topic);
        }

        [Test]
        public async Task TestGetResolution()
        {
            var service = new NewResolutionService(_munityContext, new MongoTestString());
            var resolution = await service.CreateResolution("Test2");
            Assert.NotNull(resolution);
            Assert.AreEqual("Test2", resolution.Header.Topic);
            var reloadResolution = await service.GetResolution(resolution.ResolutionId);
            Assert.NotNull(reloadResolution);
            Assert.AreEqual(resolution.ResolutionId, reloadResolution.ResolutionId);
            Assert.AreEqual(resolution.Header.Topic, reloadResolution.Header.Topic);
        }

        [Test]
        public async Task TestAddPreambleParagraph()
        {
            var service = new NewResolutionService(_munityContext, new MongoTestString());
            var resolution = await service.CreateResolution("Test2");
            var result = await service.AddPreambleParagraph(resolution);
            Assert.AreEqual(1, resolution.Preamble.Paragraphs.Count);
            Assert.NotNull(result.PreambleParagraphId);
            Assert.NotNull(result);
        }

        [Test]
        public async Task TestAddOperativeParagraph()
        {
            var service = new NewResolutionService(_munityContext, new MongoTestString());
            var resolution = await service.CreateResolution("Test2");
            var result = await service.AddOperativeParagraph(resolution);
            Assert.AreEqual(1, resolution.OperativeSection.Paragraphs.Count);
            Assert.NotNull(result);
        }

        [Test]
        public async Task TestDeleteResolution()
        {
            var service = new NewResolutionService(_munityContext, new MongoTestString());
            var resolution = await service.CreateResolution("Test2");
            var reloadResolution = await service.GetResolution(resolution.ResolutionId);
            Assert.NotNull(reloadResolution);
            var result = await service.DeleteResolution(reloadResolution);
            Assert.NotNull(result);
            reloadResolution = await service.GetResolution(resolution.ResolutionId);
            Assert.Null(reloadResolution);
        }

        [Test]
        public async Task TestRemovePreambleParagraph()
        {
            var service = new NewResolutionService(_munityContext, new MongoTestString());
            var resolution = await service.CreateResolution("Test Remove Preamble Paragraph");
            var result = await service.AddPreambleParagraph(resolution);
            var reloadResolution = await service.GetResolution(resolution.ResolutionId);
            Assert.AreEqual(1, reloadResolution.Preamble.Paragraphs.Count);
            Assert.AreEqual(1, resolution.Preamble.Paragraphs.Count);
            await service.RemovePreambleParagraph(resolution, result.PreambleParagraphId);
            Assert.AreEqual(0, resolution.Preamble.Paragraphs.Count);
            reloadResolution = await service.GetResolution(resolution.ResolutionId);
            Assert.AreEqual(0, reloadResolution.Preamble.Paragraphs.Count);
        }

        [Test]
        public async Task TestMovePreambleParagraphUp()
        {
            var service = new NewResolutionService(_munityContext, new MongoTestString());
            var resolution = await service.CreateResolution("Test Move Paragraph Up");
            var paragraphOne = await service.AddPreambleParagraph(resolution, "Paragraph One");
            var paragraphTwo = await service.AddPreambleParagraph(resolution, "Paragraph Two");

            Assert.AreEqual(2, resolution.Preamble.Paragraphs.Count);
            Assert.IsFalse(paragraphOne.PreambleParagraphId == paragraphTwo.PreambleParagraphId);
            Assert.IsTrue(paragraphOne.PreambleParagraphId == resolution.Preamble.Paragraphs[0].PreambleParagraphId);
            Assert.IsTrue(paragraphTwo.PreambleParagraphId == resolution.Preamble.Paragraphs[1].PreambleParagraphId);

            // Validate if everything is saved!
            var reloadResolution = await service.GetResolution(resolution.ResolutionId);
            Assert.AreEqual(2, reloadResolution.Preamble.Paragraphs.Count);
            Assert.IsTrue(paragraphOne.PreambleParagraphId == reloadResolution.Preamble.Paragraphs[0].PreambleParagraphId);
            Assert.IsTrue(paragraphTwo.PreambleParagraphId == reloadResolution.Preamble.Paragraphs[1].PreambleParagraphId);

            // Move the paragraph
            var saved = service.MovePreambleParagraphUp(resolution, paragraphTwo.PreambleParagraphId);

            Assert.IsTrue(paragraphTwo.PreambleParagraphId == resolution.Preamble.Paragraphs[0].PreambleParagraphId);
            Assert.IsTrue(paragraphOne.PreambleParagraphId == resolution.Preamble.Paragraphs[1].PreambleParagraphId);

            // Validate if everything has been saved!
            reloadResolution = await service.GetResolution(resolution.ResolutionId);
            Assert.IsTrue(paragraphTwo.PreambleParagraphId == reloadResolution.Preamble.Paragraphs[0].PreambleParagraphId, "The order is not as expected this element should be paragraphTwo");
            Assert.IsTrue(paragraphOne.PreambleParagraphId == reloadResolution.Preamble.Paragraphs[1].PreambleParagraphId);
        }

        [Test]
        public async Task TestMovePreambleParagraphDown()
        {
            var service = new NewResolutionService(_munityContext, new MongoTestString());
            var resolution = await service.CreateResolution("Test Move Paragraph Up");
            var paragraphOne = await service.AddPreambleParagraph(resolution, "Paragraph One");
            var paragraphTwo = await service.AddPreambleParagraph(resolution, "Paragraph Two");

            Assert.AreEqual(2, resolution.Preamble.Paragraphs.Count);
            Assert.IsFalse(paragraphOne.PreambleParagraphId == paragraphTwo.PreambleParagraphId);
            Assert.IsTrue(paragraphOne.PreambleParagraphId == resolution.Preamble.Paragraphs[0].PreambleParagraphId);
            Assert.IsTrue(paragraphTwo.PreambleParagraphId == resolution.Preamble.Paragraphs[1].PreambleParagraphId);

            // Validate if everything is saved!
            var reloadResolution = await service.GetResolution(resolution.ResolutionId);
            Assert.AreEqual(2, reloadResolution.Preamble.Paragraphs.Count);
            Assert.IsTrue(paragraphOne.PreambleParagraphId == reloadResolution.Preamble.Paragraphs[0].PreambleParagraphId);
            Assert.IsTrue(paragraphTwo.PreambleParagraphId == reloadResolution.Preamble.Paragraphs[1].PreambleParagraphId);

            // Move the paragraph
            var saved = service.MovePreambleParagraphDown(resolution, paragraphOne.PreambleParagraphId);

            Assert.IsTrue(paragraphTwo.PreambleParagraphId == resolution.Preamble.Paragraphs[0].PreambleParagraphId);
            Assert.IsTrue(paragraphOne.PreambleParagraphId == resolution.Preamble.Paragraphs[1].PreambleParagraphId);

            // Validate if everything has been saved!
            reloadResolution = await service.GetResolution(resolution.ResolutionId);
            Assert.IsTrue(paragraphTwo.PreambleParagraphId == reloadResolution.Preamble.Paragraphs[0].PreambleParagraphId, "The order is not as expected this element should be paragraphTwo");
            Assert.IsTrue(paragraphOne.PreambleParagraphId == reloadResolution.Preamble.Paragraphs[1].PreambleParagraphId);
        }

        [Test]
        [Author(("Peer Conradi"))]
        public async Task TestSaveResolutionThatsNotCreated()
        {
            var service = new NewResolutionService(_munityContext, new MongoTestString());
            var resolution = new ResolutionV2();
            var result = await service.SaveResolution(resolution);
            Assert.IsNull(result);
        }

    }
}