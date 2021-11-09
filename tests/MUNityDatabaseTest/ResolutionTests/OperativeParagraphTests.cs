using MUNity.Database.Models.Resolution;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Database.Test.ResolutionTests;

class OperativeParagraphTests : AbstractDatabaseTests
{

    private ResaElement resolution;

    [Test]
    [Order(0)]
    public void TestCreateResolution()
    {
        resolution = new ResaElement();
        _context.Resolutions.Add(resolution);
        _context.SaveChanges();
        Assert.AreEqual(1, _context.Resolutions.Count());
    }

    [Test]
    [Order(1)]
    public void TestCanAddAnOperativeParagraph()
    {
        var paragraph = new ResaOperativeParagraph()
        {
            Resolution = resolution
        };
        _context.OperativeParagraphs.Add(paragraph);
        _context.SaveChanges();
        Assert.AreEqual(1, _context.OperativeParagraphs.Count(n => n.Resolution.ResaElementId == resolution.ResaElementId));
    }

    [Test]
    [Order(2)]
    public void TestParagraphHasId()
    {
        var paragraph = _context.OperativeParagraphs.FirstOrDefault();
        Assert.IsFalse(string.IsNullOrEmpty(paragraph.ResaOperativeParagraphId));
    }

    [Test]
    [Order(3)]
    public void TestCanSetParagraphName()
    {
        var paragraph = _context.OperativeParagraphs.FirstOrDefault();
        Assert.IsFalse(_context.OperativeParagraphs.Any(n => n.Name == "Test"));
        paragraph.Name = "Test";
        _context.SaveChanges();
        Assert.IsTrue(_context.OperativeParagraphs.Any(n => n.Name == "Test"));
    }

    [Test]
    [Order(4)]
    public void TestCatSetParagraphText()
    {
        var paragraph = _context.OperativeParagraphs.FirstOrDefault();
        Assert.IsFalse(_context.OperativeParagraphs.Any(n => n.Text == "Test"));
        paragraph.Text = "Test";
        _context.SaveChanges();
        Assert.IsTrue(_context.OperativeParagraphs.Any(n => n.Text == "Test"));
    }

    [Test]
    [Order(5)]
    public void TestCanLockParagraph()
    {
        Assert.IsFalse(_context.OperativeParagraphs.Any(n => n.IsLocked == true));
        var paragraph = _context.OperativeParagraphs.FirstOrDefault();
        paragraph.IsLocked = true;
        _context.SaveChanges();
        Assert.IsTrue(_context.OperativeParagraphs.Any(n => n.IsLocked == true));
    }

    [Test]
    [Order(6)]
    public void TestCanMakeParagraphVirtual()
    {
        Assert.IsFalse(_context.OperativeParagraphs.Any(n => n.IsVirtual == true));
        var paragraph = _context.OperativeParagraphs.FirstOrDefault();
        paragraph.IsVirtual = true;
        _context.SaveChanges();
        Assert.IsTrue(_context.OperativeParagraphs.Any(n => n.IsVirtual == true));
    }

    [Test]
    [Order(7)]
    public void TestCanMakeParagraphInvisible()
    {
        Assert.IsTrue(_context.OperativeParagraphs.Any(n => n.Visible == true));
        var paragraph = _context.OperativeParagraphs.FirstOrDefault();
        paragraph.Visible = false;
        _context.SaveChanges();
        Assert.IsFalse(_context.OperativeParagraphs.Any(n => n.Visible == true));
    }

    [Test]
    [Order(8)]
    public void TestCanMakeParagraphCorrected()
    {
        Assert.IsFalse(_context.OperativeParagraphs.Any(n => n.Corrected == true));
        var paragraph = _context.OperativeParagraphs.FirstOrDefault();
        paragraph.Corrected = true;
        _context.SaveChanges();
        Assert.IsTrue(_context.OperativeParagraphs.Any(n => n.Corrected == true));
    }

    [Test]
    [Order(9)]
    public void TestCanSetParagraphComment()
    {
        Assert.IsFalse(_context.OperativeParagraphs.Any(n => n.Comment == "Test"));
        var paragraph = _context.OperativeParagraphs.FirstOrDefault();
        paragraph.Comment = "Test";
        _context.SaveChanges();
        Assert.IsTrue(_context.OperativeParagraphs.Any(n => n.Comment == "Test"));
    }

    [Test]
    [Order(10)]
    public void TestCanSetOrderIndex()
    {
        Assert.IsTrue(_context.OperativeParagraphs.Any(n => n.OrderIndex == 0));
        Assert.IsFalse(_context.OperativeParagraphs.Any(n => n.OrderIndex == 1));
        var paragraph = _context.OperativeParagraphs.FirstOrDefault();
        paragraph.OrderIndex = 1;
        _context.SaveChanges();
        Assert.IsFalse(_context.OperativeParagraphs.Any(n => n.OrderIndex == 0));
        Assert.IsTrue(_context.OperativeParagraphs.Any(n => n.OrderIndex == 1));
    }

    public OperativeParagraphTests() : base("operativeParagraph.db")
    {

    }
}
