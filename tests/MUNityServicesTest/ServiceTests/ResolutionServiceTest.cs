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

        [Test]
        public void TestResolutionMoveAmendmentFirstToSecond()
        {
            var resolution = service.CreateResolution();
            var oaOne = service.CreateOperativeParagraph(resolution, "OA1");
            var oaTwo = service.CreateOperativeParagraph(resolution, "OA2");
            var oaThree = service.CreateOperativeParagraph(resolution, "OA3");
            var oaFour = service.CreateOperativeParagraph(resolution, "OA4");

            Assert.AreEqual(4, resolution.OperativeParagraphs.Count);
            service.AddMoveAmendment(oaOne, 0, 2);

            Assert.AreEqual(1, oaOne.MoveAmendments.Count);
            Assert.AreEqual(5, resolution.OperativeParagraphs.Count);

            Assert.AreSame(oaOne, resolution.OperativeParagraphs[0]);
            Assert.AreSame(oaTwo, resolution.OperativeParagraphs[1]);
            Assert.IsTrue(resolution.OperativeParagraphs[2].IsVirtual);
            Assert.AreSame(oaThree, resolution.OperativeParagraphs[3]);
            Assert.AreSame(oaFour, resolution.OperativeParagraphs[4]);
        }

        [Test]
        public void TestResolutionMoveAmendmentFirstToThird()
        {
            var resolution = service.CreateResolution();
            var oaOne = service.CreateOperativeParagraph(resolution, "OA1");
            var oaTwo = service.CreateOperativeParagraph(resolution, "OA2");
            var oaThree = service.CreateOperativeParagraph(resolution, "OA3");
            var oaFour = service.CreateOperativeParagraph(resolution, "OA4");

            Assert.AreEqual(4, resolution.OperativeParagraphs.Count);
            service.AddMoveAmendment(oaOne, 0, 3);

            Assert.AreEqual(1, oaOne.MoveAmendments.Count);
            Assert.AreEqual(5, resolution.OperativeParagraphs.Count);

            Assert.AreSame(oaOne, resolution.OperativeParagraphs[0]);
            Assert.AreSame(oaTwo, resolution.OperativeParagraphs[1]);
            Assert.AreSame(oaThree, resolution.OperativeParagraphs[2]);
            Assert.IsTrue(resolution.OperativeParagraphs[3].IsVirtual);
            Assert.AreSame(oaFour, resolution.OperativeParagraphs[4]);

            Assert.AreEqual(0, resolution.OperativeParagraphs[0].OrderIndex);
            Assert.AreEqual(1, resolution.OperativeParagraphs[1].OrderIndex);
            Assert.AreEqual(2, resolution.OperativeParagraphs[2].OrderIndex);
            Assert.AreEqual(3, resolution.OperativeParagraphs[3].OrderIndex);
            Assert.AreEqual(4, resolution.OperativeParagraphs[4].OrderIndex);
        }

        [Test]
        public void TestResolutionMoveAmendmentLastToFirst()
        {
            var resolution = service.CreateResolution();
            var oaOne = service.CreateOperativeParagraph(resolution, "OA1");
            var oaTwo = service.CreateOperativeParagraph(resolution, "OA2");
            var oaThree = service.CreateOperativeParagraph(resolution, "OA3");
            var oaFour = service.CreateOperativeParagraph(resolution, "OA4");

            Assert.AreEqual(4, resolution.OperativeParagraphs.Count);
            service.AddMoveAmendment(oaFour, 0, 0);

            Assert.AreEqual(1, oaFour.MoveAmendments.Count);
            Assert.AreEqual(5, resolution.OperativeParagraphs.Count);
            Assert.IsTrue(resolution.OperativeParagraphs[0].IsVirtual);
            Assert.AreSame(oaOne, resolution.OperativeParagraphs[1]);
            Assert.AreSame(oaTwo, resolution.OperativeParagraphs[2]);
            Assert.AreSame(oaThree, resolution.OperativeParagraphs[3]);
            Assert.AreSame(oaFour, resolution.OperativeParagraphs[4]);
        }

        [Test]
        public void TestResolutionMoveAmendmentLasToSecond()
        {
            var resolution = service.CreateResolution();
            var oaOne = service.CreateOperativeParagraph(resolution, "OA1");
            var oaTwo = service.CreateOperativeParagraph(resolution, "OA2");
            var oaThree = service.CreateOperativeParagraph(resolution, "OA3");
            var oaFour = service.CreateOperativeParagraph(resolution, "OA4");

            Assert.AreEqual(4, resolution.OperativeParagraphs.Count);
            service.AddMoveAmendment(oaFour, 0, 1);

            Assert.AreEqual(1, oaFour.MoveAmendments.Count);
            Assert.AreEqual(5, resolution.OperativeParagraphs.Count);
            Assert.AreSame(oaOne, resolution.OperativeParagraphs[0]);
            Assert.IsTrue(resolution.OperativeParagraphs[1].IsVirtual);
            Assert.AreSame(oaTwo, resolution.OperativeParagraphs[2]);
            Assert.AreSame(oaThree, resolution.OperativeParagraphs[3]);
            Assert.AreSame(oaFour, resolution.OperativeParagraphs[4]);
        }

        [Test]
        public void TestAcceptMoveAmendmentFourToOne()
        {
            var resolution = service.CreateResolution();
            var oaOne = service.CreateOperativeParagraph(resolution, "OA1");
            var oaTwo = service.CreateOperativeParagraph(resolution, "OA2");
            var oaThree = service.CreateOperativeParagraph(resolution, "OA3");
            var oaFour = service.CreateOperativeParagraph(resolution, "OA4");

            Assert.AreEqual(4, resolution.OperativeParagraphs.Count);
            service.AddMoveAmendment(oaFour, 0, 0);

            var amendment = oaFour.MoveAmendments.FirstOrDefault();
            Assert.IsNotNull(amendment);
            service.SubmitMoveAmendment(amendment);

            Assert.AreEqual(4, resolution.OperativeParagraphs.Count);

            Assert.AreEqual("OA4", resolution.OperativeParagraphs[0].Text);
            Assert.AreEqual(oaOne, resolution.OperativeParagraphs[1]);
            Assert.AreEqual(oaTwo, resolution.OperativeParagraphs[2]);
            Assert.AreEqual(oaThree, resolution.OperativeParagraphs[3]);

            Assert.AreEqual(0, resolution.OperativeParagraphs[0].OrderIndex);
            Assert.AreEqual(1, resolution.OperativeParagraphs[1].OrderIndex);
            Assert.AreEqual(2, resolution.OperativeParagraphs[2].OrderIndex);
            Assert.AreEqual(3, resolution.OperativeParagraphs[3].OrderIndex);
        }

        [Test]
        public void TestAcceptMoveAmendmentOneToThree()
        {
            var resolution = service.CreateResolution();
            var oaOne = service.CreateOperativeParagraph(resolution, "OA1");
            var oaTwo = service.CreateOperativeParagraph(resolution, "OA2");
            var oaThree = service.CreateOperativeParagraph(resolution, "OA3");
            var oaFour = service.CreateOperativeParagraph(resolution, "OA4");

            Assert.AreEqual(4, resolution.OperativeParagraphs.Count);
            service.AddMoveAmendment(oaOne, 0, 3);

            var amendment = oaOne.MoveAmendments.FirstOrDefault();
            Assert.IsNotNull(amendment);
            service.SubmitMoveAmendment(amendment);

            Assert.AreEqual(4, resolution.OperativeParagraphs.Count);

            Assert.AreEqual("OA2", resolution.OperativeParagraphs[0].Text);
            Assert.AreEqual("OA3", resolution.OperativeParagraphs[1].Text);
            Assert.AreEqual("OA1", resolution.OperativeParagraphs[2].Text);
            Assert.AreEqual("OA4", resolution.OperativeParagraphs[3].Text);

            Assert.AreEqual(0, resolution.OperativeParagraphs[0].OrderIndex);
            Assert.AreEqual(1, resolution.OperativeParagraphs[1].OrderIndex);
            Assert.AreEqual(2, resolution.OperativeParagraphs[2].OrderIndex);
            Assert.AreEqual(3, resolution.OperativeParagraphs[3].OrderIndex);
        }

        [Test]
        public void TestCreateAddAmendment()
        {
            var resolution = service.CreateResolution();
            var oaOne = service.CreateOperativeParagraph(resolution, "OA1");
            var oaTwo = service.CreateOperativeParagraph(resolution, "OA2");
            var oaThree = service.CreateOperativeParagraph(resolution, "OA3");

            service.AddAddAmendment(resolution, 0, "New Text");
            Assert.AreEqual("OA1", resolution.OperativeParagraphs[0].Text);
            Assert.AreEqual("OA2", resolution.OperativeParagraphs[1].Text);
            Assert.AreEqual("OA3", resolution.OperativeParagraphs[2].Text);
            Assert.AreEqual("New Text", resolution.OperativeParagraphs[3].Text);

            Assert.AreEqual(0, resolution.OperativeParagraphs[0].OrderIndex);
            Assert.AreEqual(1, resolution.OperativeParagraphs[1].OrderIndex);
            Assert.AreEqual(2, resolution.OperativeParagraphs[2].OrderIndex);
            Assert.AreEqual(3, resolution.OperativeParagraphs[3].OrderIndex);
        }

    }
}
