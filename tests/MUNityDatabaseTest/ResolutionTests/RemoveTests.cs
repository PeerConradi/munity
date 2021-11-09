using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUNity.Database.Models.Resolution;
using NUnit.Framework;

namespace MUNity.Database.Test.ResolutionTests;

public class RemoveTests : AbstractDatabaseTests
{
    public RemoveTests() : base("deleteTests")
    {

    }

    [SetUp]
    public void ResetDatabaseBeforeTest()
    {
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
    }

    [Test]
    [Description("This Test will create a new auth for the resolution manually. Normaly this will be done by default!")]
    public void TestRemovingResolutionRemovesAuth()
    {
        var resolution = new ResaElement();
        var auth = new ResolutionAuth();
        resolution.Authorization = auth;
        _context.Resolutions.Add(resolution);
        _context.SaveChanges();
        Assert.AreEqual(1, _context.Resolutions.Count());
        Assert.AreEqual(1, _context.ResolutionAuths.Count(), "Expected to have a resolution auth, but there were none...");

        Assert.AreEqual(_context.Resolutions.First().ResaElementId, _context.ResolutionAuths.First().ResolutionId);

        // Perform deletion
        _context.Resolutions.Remove(resolution);
        _context.SaveChanges();
        Assert.AreEqual(0, _context.Resolutions.Count());
        Assert.AreEqual(0, _context.ResolutionAuths.Count());

    }

    [Test]
    public void TestRemoveResolutionDeletePreambleParagraphs()
    {
        var resolution = new ResaElement();
        var preambleParagraph = new ResaPreambleParagraph();
        resolution.PreambleParagraphs.Add(preambleParagraph);
        _context.Resolutions.Add(resolution);
        _context.SaveChanges();

        Assert.AreEqual(1, _context.Resolutions.Count());
        Assert.AreEqual(1, _context.PreambleParagraphs.Count());

        _context.Resolutions.Remove(resolution);
        _context.SaveChanges();
        Assert.AreEqual(0, _context.Resolutions.Count());
        Assert.AreEqual(0, _context.PreambleParagraphs.Count());
    }

    [Test]
    public void TestRemoveResolutionDeletesOperativeParagraphs()
    {
        var resolution = new ResaElement();
        var operativeParagraph = new ResaOperativeParagraph();
        resolution.OperativeParagraphs.Add(operativeParagraph);
        _context.Resolutions.Add(resolution);
        _context.SaveChanges();

        Assert.AreEqual(1, _context.Resolutions.Count());
        Assert.AreEqual(1, _context.OperativeParagraphs.Count());

        _context.Resolutions.Remove(resolution);
        _context.SaveChanges();
        Assert.AreEqual(0, _context.Resolutions.Count());
        Assert.AreEqual(0, _context.OperativeParagraphs.Count());
    }

    [Test]
    public void TestRemoveParentOperativeParagraphRemovesChildren()
    {
        var resolution = new ResaElement();
        var operativeParagraph = new ResaOperativeParagraph();
        var subParagraph = new ResaOperativeParagraph();
        subParagraph.Resolution = resolution;
        operativeParagraph.Children.Add(subParagraph);
        resolution.OperativeParagraphs.Add(operativeParagraph);

        _context.Resolutions.Add(resolution);
        _context.SaveChanges();

        Assert.AreEqual(1, _context.Resolutions.Count());
        Assert.AreEqual(2, _context.OperativeParagraphs.Count());

        _context.OperativeParagraphs.Remove(operativeParagraph);
        _context.SaveChanges();
        Assert.AreEqual(0, _context.OperativeParagraphs.Count());
    }

    [Test]
    public void TestRemoveOperativeParagraphRemovesDeleteAmendments()
    {
        var resolution = new ResaElement();
        var operativeParaph = new ResaOperativeParagraph();
        resolution.OperativeParagraphs.Add(operativeParaph);
        var deleteAmendments = new ResaDeleteAmendment();
        operativeParaph.DeleteAmendments.Add(deleteAmendments);
        _context.Resolutions.Add(resolution);
        _context.SaveChanges();

        Assert.AreEqual(1, _context.Resolutions.Count());
        Assert.AreEqual(1, _context.OperativeParagraphs.Count());
        Assert.AreEqual(1, _context.ResolutionDeleteAmendments.Count());

        _context.OperativeParagraphs.Remove(operativeParaph);
        _context.SaveChanges();

        Assert.AreEqual(1, _context.Resolutions.Count());
        Assert.AreEqual(0, _context.OperativeParagraphs.Count());
        Assert.AreEqual(0, _context.ResolutionDeleteAmendments.Count());
    }

    [Test]
    public void TestRemoveOperativeParagraphRemovesMoveAmendments()
    {
        var resolution = new ResaElement();
        var operativeParaph = new ResaOperativeParagraph();
        resolution.OperativeParagraphs.Add(operativeParaph);
        var moveAmendment = new ResaMoveAmendment();
        moveAmendment.SourceParagraph = operativeParaph;
        operativeParaph.MoveAmendments.Add(moveAmendment);
        _context.Resolutions.Add(resolution);
        _context.SaveChanges();

        Assert.AreEqual(1, _context.Resolutions.Count());
        Assert.AreEqual(1, _context.ResolutionMoveAmendments.Count());
        Assert.AreEqual(1, _context.OperativeParagraphs.Count());

        _context.OperativeParagraphs.Remove(operativeParaph);
        _context.SaveChanges();

        Assert.AreEqual(1, _context.Resolutions.Count());
        Assert.AreEqual(0, _context.ResolutionMoveAmendments.Count());
        Assert.AreEqual(0, _context.ResolutionDeleteAmendments.Count());
    }

    [Test]
    public void TestRemoveOperativeParagraphRemovesChangeAmendment()
    {
        var resolution = new ResaElement();
        var operativeParagraph = new ResaOperativeParagraph();
        resolution.OperativeParagraphs.Add(operativeParagraph);
        var changeAmendment = new ResaChangeAmendment();
        operativeParagraph.ChangeAmendments.Add(changeAmendment);

        _context.Resolutions.Add(resolution);
        _context.SaveChanges();

        Assert.AreEqual(1, _context.Resolutions.Count());
        Assert.AreEqual(1, _context.OperativeParagraphs.Count());
        Assert.AreEqual(1, _context.ResolutionChangeAmendments.Count());

        _context.OperativeParagraphs.Remove(operativeParagraph);
        _context.SaveChanges();

        Assert.AreEqual(1, _context.Resolutions.Count());
        Assert.AreEqual(0, _context.OperativeParagraphs.Count());
        Assert.AreEqual(0, _context.ResolutionChangeAmendments.Count());

    }
}
