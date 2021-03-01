using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MUNityCore.Controllers.Resa;
using MUNityCore.DataHandlers.EntityFramework;
using NUnit.Framework;
using System.Net.Http;
using MUNity.Models.Resolution;
using Moq;
using Microsoft.AspNetCore.SignalR;
using MUNityCore.Hubs;
using MUNity.Models.Resolution.EventArguments;
using MUNity.Schema.Resolution;

namespace MUNityCoreTest.ResolutionTest.ControllerTests
{
    [Description("Tests the workflow of the different controllers and how well they can interact with one another.")]
    public class ControllerFlowTest
    {
        private MunityContext _context;

        private MUNityCore.Services.SqlResolutionService _service;

        private Mock<IHubContext<ResolutionHub, MUNity.Hubs.ITypedResolutionHub>> _mockHub;

        private string resolutionId;

        [OneTimeSetUp]
        public void Setup()
        {
            // Datenbank für den Test erzeugen und falls vorhanden erst einmal leeren und neu erstellen!
            var optionsBuilder = new DbContextOptionsBuilder<MunityContext>();
            _mockHub = new Mock<IHubContext<ResolutionHub, MUNity.Hubs.ITypedResolutionHub>>();
            optionsBuilder.UseSqlite("Data Source=test_resolutions.db");
            _context = new MunityContext(optionsBuilder.Options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            _service = new MUNityCore.Services.SqlResolutionService(_context);
        }

        [Test]
        [Order(0)]
        public async Task CreatePublic()
        {
            var controller = new MainResaController(this._service);
            var result = await controller.Public();
            Assert.NotNull(result);
            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);
            var id = okResult.Value as string;
            Assert.NotNull(id);
            this.resolutionId = id;
        }

        [Test]
        [Order(1)]
        public async Task GetResolution()
        {
            var resolution = await GetTestResolution();
            Assert.NotNull(resolution);
        }

        [Test]
        [Order(2)]
        public async Task UpdateName()
        {
            var headerController = new HeaderController(_mockHub.Object, _service);
            var body = new HeaderStringPropChangedEventArgs(resolutionId, "New Name")
            {
                Tan = "12345",
            };
            var result = await headerController.Name(body);
            Assert.NotNull(result);
            Assert.IsTrue(result is OkResult);
            var resolution = await GetTestResolution();
            Assert.AreEqual("New Name", resolution.Header.Name);
        }

        [Test]
        [Order(3)]
        public async Task UpdateFullName()
        {
            var headerController = new HeaderController(_mockHub.Object, _service);
            var body = new HeaderStringPropChangedEventArgs(resolutionId, "New FullName")
            {
                Tan = "12345",
            };
            var result = await headerController.FullName(body);
            Assert.NotNull(result);
            Assert.IsTrue(result is OkResult);
            var resolution = await GetTestResolution();
            Assert.AreEqual("New FullName", resolution.Header.FullName);
        }

        [Test]
        [Order(4)]
        public async Task UpdateTopic()
        {
            var headerController = new HeaderController(_mockHub.Object, _service);
            var body = new HeaderStringPropChangedEventArgs(resolutionId, "New Topic")
            {
                Tan = "12345",
            };
            var result = await headerController.Topic(body);
            Assert.NotNull(result);
            Assert.IsTrue(result is OkResult);
            var resolution = await GetTestResolution();
            Assert.AreEqual("New Topic", resolution.Header.Topic);
        }

        [Test]
        [Order(5)]
        public async Task UpdateAgendaItem()
        {
            var headerController = new HeaderController(_mockHub.Object, _service);
            var body = new HeaderStringPropChangedEventArgs(resolutionId, "New AgendaItem")
            {
                Tan = "12345",
            };
            var result = await headerController.AgendaItem(body);
            Assert.NotNull(result);
            Assert.IsTrue(result is OkResult);
            var resolution = await GetTestResolution();
            Assert.AreEqual("New AgendaItem", resolution.Header.AgendaItem);
        }

        [Test]
        [Order(6)]
        public async Task UpdateSession()
        {
            var headerController = new HeaderController(_mockHub.Object, _service);
            var body = new HeaderStringPropChangedEventArgs(resolutionId, "New Session")
            {
                Tan = "12345",
            };
            var result = await headerController.Session(body);
            Assert.NotNull(result);
            Assert.IsTrue(result is OkResult);
            var resolution = await GetTestResolution();
            Assert.AreEqual("New Session", resolution.Header.Session);
        }

        [Test]
        [Order(7)]
        public async Task UpdateSubmitterName()
        {
            var headerController = new HeaderController(_mockHub.Object, _service);
            var body = new HeaderStringPropChangedEventArgs(resolutionId, "New SubmitterName")
            {
                Tan = "12345",
            };
            var result = await headerController.SubmitterName(body);
            Assert.NotNull(result);
            Assert.IsTrue(result is OkResult);
            var resolution = await GetTestResolution();
            Assert.AreEqual("New SubmitterName", resolution.Header.SubmitterName);
        }

        [Test]
        [Order(8)]
        public async Task UpdateCommitteeName()
        {
            var headerController = new HeaderController(_mockHub.Object, _service);
            var body = new HeaderStringPropChangedEventArgs(resolutionId, "New CommitteeName")
            {
                Tan = "12345",
            };
            var result = await headerController.CommitteeName(body);
            Assert.NotNull(result);
            Assert.IsTrue(result is OkResult);
            var resolution = await GetTestResolution();
            Assert.AreEqual("New CommitteeName", resolution.Header.CommitteeName);
        }

        [Test]
        [Order(9)]
        public async Task CreatePreambleParagraph()
        {
            var preambleController = new PreambleController(_mockHub.Object, _service);
            var body = new AddPreambleParagraphRequest()
            {
                ResolutionId = resolutionId
            };
            var result = preambleController.AddParagraph(body);
            Assert.NotNull(result);
            var okObjectResult = result.Result as OkObjectResult;
            Assert.NotNull(okObjectResult, $"Expected an ok Object result but got {result.Result.GetType()}");
            var dto = okObjectResult.Value as PreambleParagraph;
            Assert.NotNull(dto, $"Expected an PreambleParagraph dto but got: {okObjectResult.Value.GetType()}");
            var resolution = await GetTestResolution();
            Assert.AreEqual(1, resolution.Preamble.Paragraphs.Count);
        }

        [Test]
        [Order(10)]
        public async Task ChangePreambleText()
        {
            var resolution = await GetTestResolution();

            var preambleController = new PreambleController(_mockHub.Object, _service);
            var body = new ChangePreambleParagraphTextRequest()
            {
                NewText = "New Preamble Text",
                PreambleParagraphId = resolution.Preamble.Paragraphs.First().PreambleParagraphId,
                ResolutionId = resolution.ResolutionId
            };
            var result = preambleController.Text(body);
            Assert.IsTrue(result is OkResult);
            var reloadResolution = await GetTestResolution();
            Assert.AreEqual("New Preamble Text", reloadResolution.Preamble.Paragraphs[0].Text);
        }

        [Test]
        [Order(11)]
        public async Task ChangePreambleComment()
        {
            var resolution = await GetTestResolution();

            var preambleController = new PreambleController(_mockHub.Object, _service);
            var body = new ChangePreambleParagraphTextRequest()
            {
                NewText = "New Comment Text",
                PreambleParagraphId = resolution.Preamble.Paragraphs.First().PreambleParagraphId,
                ResolutionId = resolution.ResolutionId
            };
            var result = preambleController.Comment(body);
            Assert.IsTrue(result is OkResult);
        }

        [Test]
        [Order(12)]
        public async Task RemovePreambleParagraph()
        {
            var resolution = await GetTestResolution();

            var preambleController = new PreambleController(_mockHub.Object, _service);
            var body = new RemovePreambleParagraphRequest()
            {
                PreambleParagraphId = resolution.Preamble.Paragraphs.First().PreambleParagraphId,
                ResolutionId = resolution.ResolutionId
            };
            var result = preambleController.Remove(body);
            Assert.IsTrue(result is OkResult);

            var reloadResolution = await GetTestResolution();
            Assert.IsFalse(reloadResolution.Preamble.Paragraphs.Any());
        }

        [Test]
        [Order(13)]
        public async Task CreateTwoParagraphsAndSwap()
        {
            var controller = new PreambleController(_mockHub.Object, _service);
            var paragraphOne = CreateParagraph();
            var paragraphTwo = CreateParagraph();
            Assert.NotNull(paragraphOne);
            Assert.NotNull(paragraphTwo);
            var changeTextOne = SetTestResolutionPreambleParagraphText(paragraphOne.PreambleParagraphId, "One");
            var changeTextTwo = SetTestResolutionPreambleParagraphText(paragraphTwo.PreambleParagraphId, "Two");
            Assert.True(changeTextOne);
            Assert.True(changeTextTwo);

            var list = new List<string>();
            list.Add(paragraphTwo.PreambleParagraphId);
            list.Add(paragraphOne.PreambleParagraphId);

            var body = new ReorderPreambleRequest()
            {
                NewOrder = list,
                ResolutionId = resolutionId
            };

            var result = controller.Reorder(body);
            Assert.IsTrue(result is OkResult);

            var resolution = await GetTestResolution();
            Assert.AreEqual("Two", resolution.Preamble.Paragraphs[0].Text);
            Assert.AreEqual("One", resolution.Preamble.Paragraphs[1].Text);
        }

        [Test]
        [Order(14)]
        public async Task CreateOperativeParagraph()
        {
            var controller = new OperativeParagraphController(_mockHub.Object, _service);
            var body = new AddOperativeParagraphRequest()
            {
                ResolutionId = resolutionId
            };
            var response = controller.AddParagraph(body);
            Assert.NotNull(response);
            var okObjectResult = response.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var paragraph = okObjectResult.Value as OperativeParagraph;
            Assert.NotNull(paragraph);

            var resolution = await GetTestResolution();
            Assert.AreEqual(1, resolution.OperativeSection.Paragraphs.Count);
        }

        [Test]
        [Order(15)]
        public async Task ChangeOperativeParagraphText()
        {
            var resolution = await GetTestResolution();
            var paragraph = resolution.OperativeSection.Paragraphs.First();
            Assert.NotNull(paragraph);

            var controller = new OperativeParagraphController(_mockHub.Object, _service);
            var body = new ChangeOperativeParagraphTestRequest()
            {
                NewText = "New Paragraph Text",
                OperativeParagraphId = paragraph.OperativeParagraphId,
                ResolutionId = resolutionId
            };

            var response = controller.Text(body);
            Assert.NotNull(response);
            Assert.IsTrue(response is OkResult);
        }

        private bool SetTestResolutionPreambleParagraphText(string paragraphId, string text)
        {
            var preambleController = new PreambleController(_mockHub.Object, _service);
            var body = new ChangePreambleParagraphTextRequest()
            {
                NewText = text,
                PreambleParagraphId = paragraphId,
                ResolutionId = resolutionId
            };
            var result = preambleController.Text(body);
            return result is OkResult;
        }

        private PreambleParagraph CreateParagraph()
        {
            var preambleController = new PreambleController(_mockHub.Object, _service);
            var body = new AddPreambleParagraphRequest()
            {
                ResolutionId = resolutionId
            };
            var result = preambleController.AddParagraph(body);
            Assert.NotNull(result);
            var okObjectResult = result.Result as OkObjectResult;
            Assert.NotNull(okObjectResult, $"Expected an ok Object result but got {result.Result.GetType()}");
            var dto = okObjectResult.Value as PreambleParagraph;
            return dto;
        }

        private async Task<Resolution> GetTestResolution()
        {
            var controller = new MainResaController(this._service);
            var result = await controller.Public(resolutionId);
            Assert.NotNull(result);
            var okObjectResult = result.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var resolution = okObjectResult.Value as Resolution;
            return resolution;
        }
    }
}
