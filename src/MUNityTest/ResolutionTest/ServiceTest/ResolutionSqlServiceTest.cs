using MUNityCore.DataHandlers.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using MUNityCore.Models.Resolution.SqlResa;

namespace MUNityCoreTest.ResolutionTest.ServiceTest
{
    public class ResolutionSqlServiceTest
    {
        private MunityContext _context;

        private MUNityCore.Services.SqlResolutionService _service;

        private string resolutionId;

        [OneTimeSetUp]
        public void Setup()
        {
            // Datenbank für den Test erzeugen und falls vorhanden erst einmal leeren und neu erstellen!
            var optionsBuilder = new DbContextOptionsBuilder<MunityContext>();
            optionsBuilder.UseSqlite("Data Source=test_resolutions.db");
            _context = new MunityContext(optionsBuilder.Options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            _service = new MUNityCore.Services.SqlResolutionService(_context);
        }

        [Test]
        [Order(0)]
        public async Task TestCreateNew()
        {
            await _service.CreatePublicResolutionAsync("Test");
            Assert.IsTrue(_context.Resolutions.Any());
            Assert.IsTrue(_context.Resolutions.Any(n => n.Topic == "Test"));
            this.resolutionId = _context.Resolutions.FirstOrDefault()?.ResaElementId;
            Assert.NotNull(resolutionId);
        }

        [Test]
        [Order(1)]
        public async Task SetSetTopic()
        {
            var success = await _service.ChangeTopicAsync(resolutionId, "New Topic");
            Assert.IsTrue(success);
            var resolution = this._context.Resolutions.FirstOrDefault(n => n.ResaElementId == resolutionId);
            Assert.NotNull(resolution);
            Assert.AreEqual("New Topic", resolution.Topic);
        }

        [Test]
        [Order(2)]
        public async Task SetSubmitterName()
        {
            bool success = await _service.SetSubmitterNameAsync(resolutionId, "New Submitter");
            Assert.IsTrue(success);
            var resolution = this._context.Resolutions.FirstOrDefault(n => n.ResaElementId == resolutionId);
            Assert.NotNull(resolution);
            Assert.AreEqual("New Submitter", resolution.SubmitterName);
        }

        [Test]
        [Order(3)]
        public async Task SetCommitteeName()
        {
            bool success = await _service.SetCommitteeNameAsync(resolutionId, "Committee");
            Assert.IsTrue(success);
            var resolution = this._context.Resolutions.Find(resolutionId);
            Assert.NotNull(resolution);
            Assert.AreEqual("Committee", resolution.CommitteeName);
        }

        [Test]
        [Order(4)]
        public async Task AddSupporterOne()
        {
            bool success = await _service.AddSupporterAsync(resolutionId, "Supporter One");
            Assert.IsTrue(success);
            var resolution = this._context.Resolutions.Find(resolutionId);
            Assert.NotNull(resolution);
            Assert.IsTrue(resolution.Supporters.Any(n => n.Name == "Supporter One"));
            Assert.NotNull(resolution.Supporters[0]);
            Assert.NotNull(resolution.Supporters[0].ResaSupporterId);
            Assert.Greater(resolution.Supporters[0].ResaSupporterId.Length, 0);
        }

        [Test]
        [Order(5)]
        public async Task AddSupporterTwo()
        {
            bool success = await _service.AddSupporterAsync(resolutionId, "Supporter Two");
            Assert.IsTrue(success);
            var resolution = this._context.Resolutions.Find(resolutionId);
            Assert.AreEqual(2, resolution.Supporters.Count);
        }

        [Test]
        [Order(6)]
        public async Task RemoveSupporterTwo()
        {
            var supporter = this._context.ResolutionSupporters.FirstOrDefault(n => n.Name == "Supporter Two");
            Assert.NotNull(supporter);
            bool success = await _service.RemoveSupporterAsync(supporter.ResaSupporterId);
            Assert.IsTrue(success);
            var resolution = this._context.Resolutions.Find(resolutionId);
            Assert.AreEqual(1, resolution.Supporters.Count);
        }

        [Test]
        [Order(7)]
        public void AddPreambleParagraph()
        {
            ResaPreambleParagraph paragraph = _service.CreatePreambleParagraph(resolutionId);
            Assert.NotNull(paragraph);
            Assert.NotNull(paragraph.ResaPreambleParagraphId);
            var resolution = _context.Resolutions.Find(resolutionId);
            Assert.AreEqual(1, resolution.PreambleParagraphs.Count);
            Assert.NotNull(resolution.PreambleParagraphs[0]);
            Assert.NotNull(resolution.PreambleParagraphs[0].ResaPreambleParagraphId);
        }

        [Test]
        [Order(8)]
        public void SetPreambleParagraphText()
        {
            var paragraph = _context.PreambleParagraphs.FirstOrDefault(n => n.ResaElement.ResaElementId == resolutionId);
            Assert.NotNull(paragraph);
            bool success = this._service.SetPreambleParagraphText(paragraph.ResaPreambleParagraphId, "New Text");
            Assert.IsTrue(success);
            var resolution = _context.Resolutions.Find(resolutionId);
            Assert.AreEqual("New Text", resolution.PreambleParagraphs[0].Text);
        }

        [Test]
        [Order(9)]
        public void SetPreambleParagraphComment()
        {
            var paragraph = _context.PreambleParagraphs.FirstOrDefault(n => n.ResaElement.ResaElementId == resolutionId);
            Assert.NotNull(paragraph);
            bool success = this._service.SetPreambleParagraphComment(paragraph.ResaPreambleParagraphId, "New Comment");
            Assert.IsTrue(success);
            var resolution = _context.Resolutions.Find(resolutionId);
            Assert.AreEqual("New Comment", resolution.PreambleParagraphs[0].Comment);
        }

        [Test]
        [Order(10)]
        public void RemovePreambleParagraph()
        {
            var paragraph = _context.PreambleParagraphs.FirstOrDefault(n => n.ResaElement.ResaElementId == resolutionId);
            Assert.NotNull(paragraph);
            bool success = this._service.RemovePreambleParagraph(paragraph.ResaPreambleParagraphId);
            Assert.IsTrue(success);
            var resolution = _context.Resolutions.Find(resolutionId);
            Assert.IsFalse(resolution.PreambleParagraphs.Any());
        }

        [Test]
        [Order(11)]
        public void CreatePreambleParagraphOne()
        {
            ResaPreambleParagraph paragraph = _service.CreatePreambleParagraph(resolutionId, "One");
            Assert.NotNull(paragraph);
            Assert.AreEqual("One", paragraph.Text);
            Assert.NotNull(paragraph.ResaPreambleParagraphId);
            Assert.AreEqual(0, paragraph.OrderIndex);
        }

        [Test]
        [Order(12)]
        public void CreatePreambleParagraphTwo()
        {
            ResaPreambleParagraph paragraph = _service.CreatePreambleParagraph(resolutionId, "Two");
            Assert.NotNull(paragraph);
            Assert.AreEqual("Two", paragraph.Text);
            Assert.NotNull(paragraph.ResaPreambleParagraphId);
            Assert.AreEqual(1, paragraph.OrderIndex);
        }

        [Test]
        [Order(13)]
        public void ReorderPreambleParagraphs()
        {
            var firstId = this._context.PreambleParagraphs.FirstOrDefault(n => n.ResaElement.ResaElementId == resolutionId && n.OrderIndex == 0)?.ResaPreambleParagraphId;
            var secondId = this._context.PreambleParagraphs.FirstOrDefault(n => n.ResaElement.ResaElementId == resolutionId && n.OrderIndex == 1)?.ResaPreambleParagraphId;
            Assert.NotNull(firstId);
            Assert.NotNull(secondId);
            var list = new List<string>();
            list.Add(secondId);
            list.Add(firstId);
            bool success = this._service.ReorderPreamble(resolutionId, list);
            var resolution = this._context.Resolutions.Find(resolutionId);
            Assert.IsTrue(success);
            Assert.AreEqual("Two", resolution.PreambleParagraphs.OrderBy(n => n.OrderIndex).ElementAt(0).Text);
            Assert.AreEqual(secondId, resolution.PreambleParagraphs.OrderBy(n => n.OrderIndex).ElementAt(0).ResaPreambleParagraphId);
            Assert.AreEqual("One", resolution.PreambleParagraphs.OrderBy(n => n.OrderIndex).ElementAt(1).Text);
            Assert.AreEqual(firstId, resolution.PreambleParagraphs.OrderBy(n => n.OrderIndex).ElementAt(1).ResaPreambleParagraphId);
        }

        [Test]
        [Order(14)]
        public void AddOperativeParagraph()
        {
            var paragraph = this._service.CreateOperativeParagraph(resolutionId);
            Assert.NotNull(paragraph);
            Assert.AreEqual(0, paragraph.OrderIndex);
            var resolution = this._context.Resolutions.Find(resolutionId);
            Assert.AreEqual(1, resolution.OperativeParagraphs.Count);
        }

        [Test]
        [Order(15)]
        public void SetOperativeParagraphText()
        {
            var paragraph = this._context.OperativeParagraphs.FirstOrDefault(n => n.Resolution.ResaElementId == resolutionId);
            Assert.NotNull(paragraph);
            bool success = this._service.SetOperativeParagraphText(paragraph.ResaOperativeParagraphId, "New Text");
            Assert.IsTrue(success);
            var resolution = this._context.Resolutions.Find(resolutionId);
            Assert.AreEqual("New Text", resolution.OperativeParagraphs[0].Text);
        }

        [Test]
        [Order(16)]
        public void SetOperativeParagraphComment()
        {
            var paragraph = this._context.OperativeParagraphs.FirstOrDefault(n => n.Resolution.ResaElementId == resolutionId);
            Assert.NotNull(paragraph);
            bool success = this._service.SetOperativeParagraphComment(paragraph.ResaOperativeParagraphId, "New Comment");
            Assert.IsTrue(success);
            var resolution = this._context.Resolutions.Find(resolutionId);
            Assert.AreEqual("New Comment", resolution.OperativeParagraphs[0].Comment);
        }

        [Test]
        [Order(17)]
        public void AddAnotherTopLevelParagraph()
        {
            var paragraph = this._service.CreateOperativeParagraph(resolutionId, "Two");
            Assert.NotNull(paragraph);
            Assert.AreEqual(1, paragraph.OrderIndex);
            Assert.NotNull(paragraph.ResaOperativeParagraphId);
            Assert.IsNull(paragraph.Parent);
        }

        [Test]
        [Order(18)]
        public void AddSubOperativeParagraph()
        {
            var resolution = this._context.Resolutions.Find(resolutionId);
            var paragraph = resolution.OperativeParagraphs.First();
            Assert.NotNull(paragraph);
            ResaOperativeParagraph childParagraph = this._service.CreateSubOperativeParagraph(paragraph.ResaOperativeParagraphId);
            Assert.AreEqual(resolutionId, childParagraph.Resolution.ResaElementId);
            Assert.NotNull(childParagraph.Parent);
            var refetchResolution = this._context.Resolutions.Find(resolutionId);
            Assert.AreEqual(3, refetchResolution.OperativeParagraphs.Count);
        }

        [Test]
        [Order(19)]
        public void AddDeleteAmendment()
        {
            var resolution = this._context.Resolutions.Find(resolutionId);
            var paragraph = resolution.OperativeParagraphs.First();
            Assert.NotNull(paragraph);
            ResaDeleteAmendment amendment = this._service.CreateDeleteAmendment(paragraph.ResaOperativeParagraphId, "submitter");
            Assert.NotNull(amendment);
            Assert.NotNull(amendment.ResaAmendmentId);
            Assert.NotNull(paragraph.DeleteAmendments);
            Assert.AreEqual(1, paragraph.DeleteAmendments.Count);
            Assert.AreEqual(1, resolution.Amendments.Count);
        }

        [Test]
        [Order(20)]
        public void AddChangeAmendment()
        {
            var resolution = this._context.Resolutions.Find(resolutionId);
            var paragraph = resolution.OperativeParagraphs.First();
            Assert.NotNull(paragraph);
            ResaChangeAmendment changeAmendment = this._service.CreateChangeAmendment(paragraph.ResaOperativeParagraphId, "submitter", "new text");
            Assert.NotNull(changeAmendment);
            Assert.AreEqual("new text", changeAmendment.NewText);
            Assert.AreEqual("submitter", changeAmendment.SubmitterName);
            Assert.AreEqual(2, resolution.Amendments.Count);
            Assert.AreEqual(1, paragraph.ChangeAmendments.Count);
            Assert.AreEqual(1, this._context.DeleteAmendments.Count(n => n.Resolution.ResaElementId == resolutionId));
            Assert.AreEqual(1, this._context.ChangeAmendments.Count(n => n.Resolution.ResaElementId == resolutionId));
        }

        [Test]
        [Order(21)]
        public void AddMoveAmendment()
        {
            var resolution = this._context.Resolutions.Find(resolutionId);
            var paragraph = resolution.OperativeParagraphs.First();
            Assert.NotNull(paragraph);
            ResaMoveAmendment moveAmendment = this._service.CreateMoveAmendment(paragraph.ResaOperativeParagraphId, 1);
            Assert.NotNull(moveAmendment);
            Assert.AreEqual(4, resolution.OperativeParagraphs.Count);
            Assert.AreEqual(1, resolution.OperativeParagraphs.Count(n => n.IsVirtual));
            var topLevelParagraphs = resolution.OperativeParagraphs.Where(n => n.Parent == null).OrderBy(n => n.OrderIndex);
            Assert.AreEqual(3, topLevelParagraphs.Count());
            Assert.AreEqual(paragraph.ResaOperativeParagraphId, topLevelParagraphs.ElementAt(0).ResaOperativeParagraphId);
            Assert.AreNotEqual(paragraph.ResaOperativeParagraphId, topLevelParagraphs.ElementAt(1).ResaOperativeParagraphId);
            Assert.AreNotEqual(moveAmendment.VirtualParagraph.ResaOperativeParagraphId, topLevelParagraphs.ElementAt(1).ResaOperativeParagraphId);
            Assert.AreEqual(moveAmendment.VirtualParagraph.ResaOperativeParagraphId, topLevelParagraphs.ElementAt(2).ResaOperativeParagraphId);
            Assert.AreEqual(3, resolution.Amendments.Count);
        }

        [Test]
        [Order(22)]
        public void AddAddAmendment()
        {
            var resolution = this._context.Resolutions.Find(resolutionId);
            Assert.NotNull(resolution);
            ResaAddAmendment addAmendment = this._service.CreateAddAmendment(resolutionId, 0, "submitter", "new Add Amendment");
            Assert.NotNull(addAmendment);
            Assert.AreEqual(5, resolution.OperativeParagraphs.Count);
            var topLevelParagraphs = resolution.OperativeParagraphs.Where(n => n.Parent == null).OrderBy(n => n.OrderIndex);
            Assert.IsTrue(topLevelParagraphs.ElementAt(0).IsVirtual);
            Assert.AreEqual("new Add Amendment", topLevelParagraphs.ElementAt(0).Text);
            Assert.AreEqual("submitter", addAmendment.SubmitterName);
            Assert.AreEqual(4, resolution.Amendments.Count);
            Assert.AreEqual(1, resolution.Amendments.OfType<ResaAddAmendment>().Count());
        }

        [Test]
        [Order(23)]
        public async Task GetDto()
        {
            var resolutionDb = this._context.Resolutions.Find(resolutionId);
            MUNity.Models.Resolution.Resolution resolution = await _service.GetResolutionDtoAsync(resolutionId);
            Assert.NotNull(resolution);
            Assert.AreEqual(resolutionDb.Topic, resolution.Header.Topic);
            Assert.AreEqual(resolutionDb.SubmitterName, resolution.Header.SubmitterName);
            Assert.AreEqual(resolutionDb.Supporters.Count, resolution.Header.Supporters.Count);

            Assert.NotNull(resolution.Preamble);
            Assert.AreEqual(2, resolution.Preamble.Paragraphs.Count);
            // we swopped them in an earlier test so now they are swapped :P
            Assert.AreEqual("Two", resolution.Preamble.Paragraphs[0].Text);
            Assert.AreEqual("One", resolution.Preamble.Paragraphs[1].Text);

            Assert.NotNull(resolution.OperativeSection);
            Assert.AreEqual(1, resolution.OperativeSection.AddAmendments.Count);
            Assert.AreEqual(1, resolution.OperativeSection.ChangeAmendments.Count);
            Assert.AreEqual(1, resolution.OperativeSection.DeleteAmendments.Count);
            Assert.AreEqual(1, resolution.OperativeSection.MoveAmendments.Count);
            Assert.NotNull(resolution.OperativeSection.Paragraphs);
            Assert.AreEqual(4, resolution.OperativeSection.Paragraphs.Count);

        }
    }
}
