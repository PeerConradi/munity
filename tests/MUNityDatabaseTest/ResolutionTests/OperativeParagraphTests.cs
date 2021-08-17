using MUNity.Database.Models.Resolution;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNityDatabaseTest.ResolutionTests
{
    class OperativeParagraphTests : AbstractDatabaseTests
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
        public void TestCanAddAnOperativeParagraph()
        {
            var paragraph = new ResaOperativeParagraph()
            {
                Resolution = this.resolution
            };
            this._context.OperativeParagraphs.Add(paragraph);
            this._context.SaveChanges();
            Assert.AreEqual(1, this._context.OperativeParagraphs.Count(n => n.Resolution.ResaElementId == this.resolution.ResaElementId));
        }

        [Test]
        [Order(2)]
        public void TestParagraphHasId()
        {
            var paragraph = this._context.OperativeParagraphs.FirstOrDefault();
            Assert.IsFalse(string.IsNullOrEmpty(paragraph.ResaOperativeParagraphId));
        }

        [Test]
        [Order(3)]
        public void TestCanSetParagraphName()
        {
            var paragraph = this._context.OperativeParagraphs.FirstOrDefault();
            Assert.IsFalse(this._context.OperativeParagraphs.Any(n => n.Name == "Test"));
            paragraph.Name = "Test";
            _context.SaveChanges();
            Assert.IsTrue(this._context.OperativeParagraphs.Any(n => n.Name == "Test"));
        }

        [Test]
        [Order(4)]
        public void TestCatSetParagraphText()
        {
            var paragraph = this._context.OperativeParagraphs.FirstOrDefault();
            Assert.IsFalse(this._context.OperativeParagraphs.Any(n => n.Text == "Test"));
            paragraph.Text = "Test";
            _context.SaveChanges();
            Assert.IsTrue(this._context.OperativeParagraphs.Any(n => n.Text == "Test"));
        }

        [Test]
        [Order(5)]
        public void TestCanLockParagraph()
        {
            Assert.IsFalse(this._context.OperativeParagraphs.Any(n => n.IsLocked == true));
            var paragraph = this._context.OperativeParagraphs.FirstOrDefault();
            paragraph.IsLocked = true;
            _context.SaveChanges();
            Assert.IsTrue(this._context.OperativeParagraphs.Any(n => n.IsLocked == true));
        }

        [Test]
        [Order(6)]
        public void TestCanMakeParagraphVirtual()
        {
            Assert.IsFalse(this._context.OperativeParagraphs.Any(n => n.IsVirtual == true));
            var paragraph = this._context.OperativeParagraphs.FirstOrDefault();
            paragraph.IsVirtual = true;
            _context.SaveChanges();
            Assert.IsTrue(this._context.OperativeParagraphs.Any(n => n.IsVirtual == true));
        }

        [Test]
        [Order(7)]
        public void TestCanMakeParagraphInvisible()
        {
            Assert.IsTrue(this._context.OperativeParagraphs.Any(n => n.Visible == true));
            var paragraph = this._context.OperativeParagraphs.FirstOrDefault();
            paragraph.Visible = false;
            _context.SaveChanges();
            Assert.IsFalse(this._context.OperativeParagraphs.Any(n => n.Visible == true));
        }

        [Test]
        [Order(8)]
        public void TestCanMakeParagraphCorrected()
        {
            Assert.IsFalse(this._context.OperativeParagraphs.Any(n => n.Corrected == true));
            var paragraph = this._context.OperativeParagraphs.FirstOrDefault();
            paragraph.Corrected = true;
            _context.SaveChanges();
            Assert.IsTrue(this._context.OperativeParagraphs.Any(n => n.Corrected == true));
        }

        [Test]
        [Order(9)]
        public void TestCanSetParagraphComment()
        {
            Assert.IsFalse(this._context.OperativeParagraphs.Any(n => n.Comment == "Test"));
            var paragraph = this._context.OperativeParagraphs.FirstOrDefault();
            paragraph.Comment = "Test";
            _context.SaveChanges();
            Assert.IsTrue(this._context.OperativeParagraphs.Any(n => n.Comment == "Test"));
        }

        [Test]
        [Order(10)]
        public void TestCanSetOrderIndex()
        {
            Assert.IsTrue(this._context.OperativeParagraphs.Any(n => n.OrderIndex == 0));
            Assert.IsFalse(this._context.OperativeParagraphs.Any(n => n.OrderIndex == 1));
            var paragraph = this._context.OperativeParagraphs.FirstOrDefault();
            paragraph.OrderIndex = 1;
            _context.SaveChanges();
            Assert.IsFalse(this._context.OperativeParagraphs.Any(n => n.OrderIndex == 0));
            Assert.IsTrue(this._context.OperativeParagraphs.Any(n => n.OrderIndex == 1));
        }

        public OperativeParagraphTests() : base("operativeParagraph.db")
        {

        }
    }
}
