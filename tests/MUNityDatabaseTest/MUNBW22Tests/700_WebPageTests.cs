using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MUNity.Database.Models.Website;
using NUnit.Framework;

namespace MUNity.Database.Test.MUNBW22Tests;

public partial class FullMUNBW22Tests
{
    [Test]
    [Order(700)]
    public void TestCreateAStartPage()
    {
        var page = new ConferenceWebPage()
        {
            Components = new List<AbstractConferenceWebPageElement>(),
            Conference = _context.Conferences.Find("munbw22"),
            CreatedUser = TestUsers.Avangers.FoundingMembers.TonyStark,
            CreationDate = DateTime.Now,
            LastUpdateDate = DateTime.Now,
            IsIndexPage = true,
            Title = "Start"
        };
        _context.ConferenceWebPages.Add(page);
        _context.SaveChanges();
        Assert.AreEqual(1, _context.ConferenceWebPages.Count());
    }

    [Test]
    [Order(701)]
    public void TestCreateATextComponentOnStartPage()
    {

        var indexPage = _context.ConferenceWebPages.FirstOrDefault(n => n.Conference.ConferenceId == "munbw22" && n.IsIndexPage);
        Assert.NotNull(indexPage);
        var component = new WebPageTextElement()
        {
            Content = "Willkommen bei <b>MUNBW 2022</b>",
            NormalizedTextRaw = "WILLKOMMEN BEI MUNBW 2022",
            TextRaw = "Willkommen bei MUNBW 2022",
            SortOrder = 0,
            Page = indexPage
        };

        indexPage.Components.Add(component);
        var changes = _context.SaveChanges();
        Assert.AreEqual(1, changes);
    }

    [Test]
    [Order(702)]
    public void TestCreateGVPage()
    {
        var gvPage = new ConferenceWebPage()
        {
            Components = new List<AbstractConferenceWebPageElement>(),
            Conference = _context.Conferences.Find("munbw22"),
            CreatedUser = TestUsers.Avangers.FoundingMembers.TonyStark,
            CreationDate = DateTime.Now,
            IsIndexPage = false,
            LastUpdateDate = DateTime.Now,
            Title = "Generalversammlung"
        };

        var textComponent = new WebPageTextElement()
        {
            Content = "Die Generalversammlung ist das größte Gremium auf der Konferenz",
            NormalizedTextRaw = "DIE GENERALVERSAMMLUNG IST DAS GROßTE GREMIUM AUF DER KONFERENZ",
            TextRaw = "Die Generalversammlung ist das größte Gremium auf der Konferenz",
            Page = gvPage,
            SortOrder = 0
        };
        gvPage.Components.Add(textComponent);

        var topicsComponent = new CommitteeTopicsElement()
        {
            Committee = _context.Committees.Find("munbw22-gv"),
            Page = gvPage,
            SortOrder = 1
        };
        gvPage.Components.Add(topicsComponent);

        var delegationsComponent = new CommitteeDelegatesElement()
        {
            Committee = _context.Committees.Find("munbw22-gv"),
            Page = gvPage,
            SortOrder = 2
        };

        gvPage.Components.Add(delegationsComponent);
        _context.ConferenceWebPages.Add(gvPage);
        var changeCount = _context.SaveChanges();
        Assert.AreEqual(4, changeCount);
    }

    [Test]
    [Order(703)]
    public void TestCreateMenuItemForIndexPage()
    {
        var indexPage = _context.ConferenceWebPages.FirstOrDefault(n => n.Conference.ConferenceId == "munbw22" &&
            n.IsIndexPage);

        var indexEntry = new ConferenceWebMenuEntry()
        {
            ChildEntries = new List<ConferenceWebMenuEntry>(),
            Conference = _context.Conferences.Find("munbw22"),
            Parent = null,
            TargetedPage = indexPage,
            Title = "Start"
        };

        _context.ConferenceWebMenuEntries.Add(indexEntry);
        var changes = _context.SaveChanges();
        Assert.AreEqual(1, changes);

        Assert.True(_context.ConferenceWebMenuEntries.Any(n => n.Conference.ConferenceId == "munbw22" && n.Title == "Start" && n.TargetedPage != null));
    }

    [Test]
    [Order(704)]
    public void TestCreateMenuItemForCommittees()
    {
        var gvPage = _context.ConferenceWebPages.FirstOrDefault(n => n.Conference.ConferenceId == "munbw22" &&
        n.Title == "Generalversammlung");
        Assert.NotNull(gvPage);

        var committeesEntry = new ConferenceWebMenuEntry()
        {
            ChildEntries = new List<ConferenceWebMenuEntry>(),
            Conference = _context.Conferences.Find("munbw22"),
            Parent = null,
            TargetedPage = null,
            Title = "Gremien"
        };

        var gvEntry = new ConferenceWebMenuEntry()
        {
            Conference = _context.Conferences.Find("munbw22"),
            Parent = committeesEntry,
            TargetedPage = gvPage,
            Title = "Generalversammlung"
        };
        committeesEntry.ChildEntries.Add(gvEntry);

        _context.ConferenceWebMenuEntries.Add(committeesEntry);
        var changes = _context.SaveChanges();
        Assert.AreEqual(2, changes);

        var recallCommitteesEntry = _context.ConferenceWebMenuEntries
            .Include(n => n.ChildEntries)
            .ThenInclude(n => n.ChildEntries)
            .FirstOrDefault(n => n.Conference.ConferenceId == "munbw22" &&
            n.Title == "Gremien");
        Assert.NotNull(recallCommitteesEntry);
        Assert.AreEqual(1, recallCommitteesEntry.ChildEntries.Count);
    }
}
