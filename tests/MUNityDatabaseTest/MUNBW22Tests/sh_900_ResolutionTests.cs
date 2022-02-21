using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUNity.Database.Models.Resolution;
using NUnit.Framework;

namespace MUNity.Database.Test.MUNBW22Tests;

public partial class FullDMUN2022Tests
{
    [Test]
    [Order(2900)]
    public void TestCreateAResolution()
    {
        var resolution = new ResaElement();
        var auth = new ResolutionAuth();
        auth.Resolution = resolution;
        auth.Conference = _context.Conferences.Find("munsh22");
        auth.Committee = _context.Committees.Find("munsh22-gv");
        _context.Resolutions.Add(resolution);
        _context.ResolutionAuths.Add(auth);
        _context.SaveChanges();

        Assert.AreEqual(1, _context.ResolutionAuths.Count());
        Assert.AreEqual(1, _context.Resolutions.Count());
    }

    [Test]
    [Order(2901)]
    public void TestCreatePreambleParagraph()
    {
        var resolution = _context.Resolutions.FirstOrDefault();
        var paragraph = new ResaPreambleParagraph();
        paragraph.ResaElement = resolution;
        paragraph.OrderIndex = _context.PreambleParagraphs.Count(n => n.ResaElement.ResaElementId == resolution.ResaElementId);
        _context.PreambleParagraphs.Add(paragraph);
        _context.SaveChanges();
        Assert.AreEqual(1, _context.PreambleParagraphs.Count());
    }


}
