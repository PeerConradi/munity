using MUNity.Database.Models.Resolution;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Database.Test.ResolutionTests;

public class BasicResolutionsTests : AbstractDatabaseTests
{
    private ResaElement resolution;

    [Test]
    [Order(0)]
    public void TestCanCreateInstance()
    {
        resolution = new ResaElement();
        Assert.NotNull(resolution);
    }

    [Test]
    [Order(1)]
    public void TestResolutionHasId()
    {
        Assert.IsFalse(string.IsNullOrEmpty(resolution.ResaElementId));
    }

    [Test]
    [Order(2)]
    public void TestResolutionCanSetTopic()
    {
        resolution.Topic = "New Resolution";
        Assert.AreEqual("New Resolution", resolution.Topic);
    }

    [Test]
    [Order(3)]
    public void TestResolutionCanSetName()
    {
        resolution.Name = "New Name";
        Assert.AreEqual("New Name", resolution.Name);
    }

    [Test]
    [Order(4)]
    public void TestResolutionCanSetFullName()
    {
        resolution.FullName = "New FullName";
        Assert.AreEqual("New FullName", resolution.FullName);
    }

    [Test]
    [Order(5)]
    public void TestResolutionCanSetAgendaItem()
    {
        resolution.AgendaItem = "Tagesordnungspunkt 1";
        Assert.AreEqual("Tagesordnungspunkt 1", resolution.AgendaItem);
    }

    [Test]
    [Order(6)]
    public void TestResolutionCanSetSession()
    {
        resolution.Session = "MUN-SH21.GV.1.1";
        Assert.AreEqual("MUN-SH21.GV.1.1", resolution.Session);
    }

    [Test]
    [Order(7)]
    public void TestResolutionCanSetSubmitterName()
    {
        resolution.SubmitterName = "Deutschland";
        Assert.AreEqual("Deutschland", resolution.SubmitterName);
    }

    [Test]
    [Order(8)]
    public void TestResolutionCanSetCommitteeName()
    {
        resolution.CommitteeName = "Generalversammlung";
        Assert.AreEqual("Generalversammlung", resolution.CommitteeName);
    }

    [Test]
    [Order(9)]
    public void TestResolutionCanSetCreatedDate()
    {
        var date = new DateTime(2021, 1, 1, 12, 0, 0);
        resolution.CreatedDate = date;
        Assert.AreEqual(date, resolution.CreatedDate);
    }

    [Test]
    [Order(10)]
    public void TestCanSetSupporterNames()
    {
        resolution.SupporterNames = "Frankreich, Spanien";
        Assert.AreEqual("Frankreich, Spanien", resolution.SupporterNames);
    }

    [Test]
    [Order(11)]
    public void TestResolutionPreambleParagraphsIsNotNull()
    {
        Assert.NotNull(resolution.PreambleParagraphs);
    }

    [Test]
    [Order(12)]
    public void TestResolutionOperativeParagraphsIsNotNull()
    {
        Assert.NotNull(resolution.OperativeParagraphs);
    }

    [Test]
    [Order(13)]
    public void TestResolutionAmendmentsIsNotNull()
    {
        Assert.NotNull(resolution.AddAmendments);
    }

    [Test]
    [Order(14)]
    public void TestResolutionCanBeStored()
    {
        _context.Resolutions.Add(resolution);
        var remark = _context.SaveChanges();
        // Two because the Auth is now created with this Resolution
        Assert.AreEqual(2, remark);
    }

    [Test]
    [Order(15)]
    public void TestLoadingResolutionKeptValues()
    {
        var loadedResolution = _context.Resolutions.First();
        Assert.AreEqual(loadedResolution.ResaElementId, resolution.ResaElementId);
        Assert.AreEqual(loadedResolution.Topic, resolution.Topic);
        Assert.AreEqual(loadedResolution.Name, resolution.Name);
        Assert.AreEqual(loadedResolution.FullName, resolution.FullName);
        Assert.AreEqual(loadedResolution.AgendaItem, resolution.AgendaItem);
        Assert.AreEqual(loadedResolution.Session, resolution.Session);
        Assert.AreEqual(loadedResolution.SubmitterName, resolution.SubmitterName);
        Assert.AreEqual(loadedResolution.CommitteeName, resolution.CommitteeName);
        Assert.AreEqual(loadedResolution.CreatedDate, resolution.CreatedDate);
        Assert.AreEqual(loadedResolution.SupporterNames, resolution.SupporterNames);
    }

    public BasicResolutionsTests() : base("resolutions.db")
    {

    }
}
