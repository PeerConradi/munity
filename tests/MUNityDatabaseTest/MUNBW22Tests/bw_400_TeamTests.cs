using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Database.Test.MUNBW22Tests;

public partial class FullDMUN2022Tests
{
    [Test]
    [Order(400)]
    public void TestAddProjektleitung()
    {
        var role = _context.Fluent.ForConference("munbw22").AddTeamRoleGroup(options =>
            options.WithShort("PL")
            .WithFullName("die  Projektleitung von MUNBW 2022")
            .WithName("Projektleitung")
            .WithRole(roleBuilder =>
                roleBuilder.WithShort("PL")
                    .WithFullName("Projektleiter")
                    .WithName("Projektleiter")
                    .WithLevel(1)));
        Assert.NotNull(role);
        Assert.AreEqual(1, _context.TeamRoleGroups.Count());
        Assert.AreEqual(1, _context.ConferenceTeamRoles.Count());
    }

    [Test]
    [Order(400)]
    public void TestAddTeamLeaderAuth()
    {
        var recaff = _context.Fluent.ForConference("munbw22").AddBasicAuthorizations();
        Assert.NotZero(recaff);
        Assert.NotZero(_context.ConferenceRoleAuthorizations.Count());
    }

    [Test]
    [Order(401)]
    public void TestAddErweiterteProjektleitung()
    {
        var projektleitung = _context.ConferenceTeamRoles.FirstOrDefault(n =>
            n.Conference.ConferenceId == "munbw22" && n.RoleName == "Projektleiter");

        Assert.NotNull(projektleitung);

        var group = _context.Fluent.ForConference("munbw22").AddTeamRoleGroup(options =>
            options.WithShort("EPL")
                .WithFullName("die  erweiterte Projektleitung von MUNBW 2022")
                .WithName("Erweiterte Projektleitung")
                .WithRole("Generalsekretär", "GS")
                .WithRole("Leitung Inhalt & Sekretariat", "Sek")
                .WithRole("Leitung Nichtstaatlicher Akteure", "NAB")
                .WithRole("Teilnehmendenwerbung", "TNW")
                .WithRole("Team- & Materialkoordination", "TMK")
                .WithRole("Chairbetreuung", "CB")
                .WithRole("Leitung Technik/Digitalisierung", "TECH")
                .WithRole("Leitung Medien")
                .WithRole("Leitung Bildungsprogramm")
                .WithRole("Teilnehmenden- & Kommservice-Betreuung", "TNB")
                .WithRole("Kassenwart", "CASH")
                .WithRole("Fundraising")
                .WithRoleLevel(2)
                .WithParentRole(projektleitung));

        Assert.AreEqual(2, _context.TeamRoleGroups.Count());
        Assert.AreEqual(13, _context.ConferenceTeamRoles.Count());
    }

    [Test]
    [Order(402)]
    public void TestConferenceHas13Roles()
    {
        var conference = _context.Conferences.Include(n => n.Roles)
            .FirstOrDefault(n => n.ConferenceId == "munbw22");
        Assert.AreEqual(13, conference.Roles.Count);
    }

    [Test]
    [Order(403)]
    public void TestMakeTonyStarkProjektleiter()
    {
        _context.Fluent.ForConference("munbw22").MakeUserParticipateInTeamRole("tonystark", "Projektleiter");
        Assert.IsTrue(_context.Participations.Any(n => n.Role.Conference.ConferenceId == "munbw22" && n.User.UserName == "tonystark"));
    }
}
