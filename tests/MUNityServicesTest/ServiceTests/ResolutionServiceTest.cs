using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using MUNity.Database.Context;
using NUnit.Framework;

namespace MUNity.Services.Test.ServiceTests
{
    public class ResolutionServiceTest
    {
        private MunityContext dbContext;

        private ResolutionService service;

        [SetUp]
        public void SetupTests()
        {
            var loggerMock = new Mock<ILogger<ResolutionService>>();
            dbContext = TestHelpers.CreateSQLiteContext("resolutiontests");
            service = new ResolutionService(dbContext, loggerMock.Object);
        }

        [Test]
        public void TestCreateResolution()
        {
            var resolution = service.CreateResolution();
            Assert.IsNotNull(resolution);
            Assert.IsFalse(string.IsNullOrEmpty(resolution.ResaElementId));
            Assert.AreEqual(1, dbContext.Resolutions.Count());
        }

        [Test]
        public void TestCreatePreambleParagraph()
        {
            var resolution = service.CreateResolution();
            var paragraph = service.CreatePreambleParagraph(resolution);
            Assert.NotNull(paragraph);
            Assert.AreEqual(1, dbContext.PreambleParagraphs.Count());
        }

        [Test]
        public void TestRemovePreambleParagraph()
        {
            var resolution = service.CreateResolution();
            var paragraph = service.CreatePreambleParagraph(resolution);
            var result = service.RemovePreambleParagraph(paragraph);
            Assert.IsTrue(result);
        }

        [Test]
        public void TestUpdatePreambleParagraph()
        {
            var resolution = service.CreateResolution();
            var paragraph = service.CreatePreambleParagraph(resolution);
            paragraph.Text = "Hallo Welt";
            var result = service.UpdatePreambleParagraph(paragraph);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void TestMovePreambleParagraphUp()
        {
            var resolution = service.CreateResolution();
            var paragraphOne = service.CreatePreambleParagraph(resolution, "Text1");
            var paragraphTwo = service.CreatePreambleParagraph(resolution, "Text2");
            var result = service.MovePreambleParagraphUp(paragraphTwo);
            var paragraphs = service.GetPreambleParagraphs(resolution.ResaElementId);
            Assert.AreEqual("Text2", paragraphs[0].Text);
            Assert.AreEqual("Text1", paragraphs[1].Text);
        }

        [Test]
        public void TestMovePreambleParagraphDown()
        {
            var resolution = service.CreateResolution();
            var paragraphOne = service.CreatePreambleParagraph(resolution, "Text1");
            var paragraphTwo = service.CreatePreambleParagraph(resolution, "Text2");
            var result = service.MovePreambleParagraphDown(paragraphOne);
            var paragraphs = service.GetPreambleParagraphs(resolution.ResaElementId);
            Assert.AreEqual("Text2", paragraphs[0].Text);
            Assert.AreEqual("Text1", paragraphs[1].Text);
        }

        [Test]
        public void TestUpdateResolutionTopic()
        {
            var resolution = service.CreateResolution();
            resolution.Topic = "Hello World";
            var result = service.UpdateResaElement(resolution);
            Assert.AreEqual(1, result);
            Assert.IsTrue(dbContext.Resolutions.Any(n => n.Topic == "Hello World"));
        }

    }
}
