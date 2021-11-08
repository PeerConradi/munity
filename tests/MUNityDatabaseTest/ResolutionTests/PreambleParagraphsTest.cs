using Microsoft.EntityFrameworkCore;
using MUNity.Database.Models.Resolution;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNityDatabaseTest.ResolutionTests
{
    public class PreambleParagraphsTest : AbstractDatabaseTests
    {
        private ResaElement resolution;

        [Test]
        [Order(0)]
        public void TestCreateResolution()
        {
            this.resolution = new ResaElement();
            this._context.Resolutions.Add(this.resolution);
            this._context.SaveChanges();
            Assert.AreEqual(1, this._context.Resolutions.Count());
        }

        [Test]
        [Order(1)]
        public void TestAddPreambleParagraph()
        {
            var tmpResolution = _context.Resolutions
                .FirstOrDefault();
            var newPreambleParagraph = new ResaPreambleParagraph()
            {
                Comment = "",
                IsCorrected = false,
                IsLocked = false,
                OrderIndex = 0,
                ResaElement = tmpResolution,
                Text = "New Paragraph"
            };
            _context.PreambleParagraphs.Add(newPreambleParagraph);
            _context.SaveChanges();
            Assert.AreEqual(1, _context.PreambleParagraphs.Count());
            var reloadParagraph = _context.PreambleParagraphs
                .Include(n => n.ResaElement)
                .FirstOrDefault(n => n.ResaPreambleParagraphId == newPreambleParagraph.ResaPreambleParagraphId);
            Assert.NotNull(reloadParagraph.ResaElement);
        }

        [Test]
        [Order(2)]
        public void TestParagraphHasAnId()
        {
            var paragraph = _context.PreambleParagraphs.FirstOrDefault();
            Assert.IsFalse(string.IsNullOrEmpty(paragraph.ResaPreambleParagraphId));
        }

        [Test]
        [Order(3)]
        public void TestParagraphIsNotCorrected()
        {
            var paragraph = _context.PreambleParagraphs.FirstOrDefault();
            Assert.IsFalse(paragraph.IsCorrected);
        }

        [Test]
        [Order(4)]
        public void TestParagraphIsNotLocked()
        {
            var paragraph = _context.PreambleParagraphs.FirstOrDefault();
            Assert.IsFalse(paragraph.IsLocked);
        }

        [Test]
        [Order(5)]
        public void TestParagraphOrderIndexIsZero()
        {
            var paragraph = _context.PreambleParagraphs.FirstOrDefault();
            Assert.AreEqual(0, paragraph.OrderIndex);
        }

        [Test]
        [Order(6)]
        public void TestParagraphCommentIsEmptyString()
        {
            var paragraph = _context.PreambleParagraphs.FirstOrDefault();
            Assert.AreEqual(string.Empty, paragraph.Comment);
        }

        [Test]
        [Order(7)]
        public void TestParagraphHasText()
        {
            var paragraph = _context.PreambleParagraphs.FirstOrDefault();
            Assert.AreEqual("New Paragraph", paragraph.Text);
        }

        public PreambleParagraphsTest() : base("operativeparagraphs.db")
        {

        }
    }
}
