using Microsoft.EntityFrameworkCore;
using MUNity.Database.Models.Conference;
using MUNity.Database.Models.Conference.Roles;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Database.Test.ConferenceTests;

public class ParticipationTests : AbstractDatabaseTests
{

    private ConferenceTeamRole teamRole;

    [Test]
    [Order(0)]
    public void TestCreateARole()
    {
        teamRole = EnsureProjectOwnerRole();
    }

    [Test]
    [Order(1)]
    public void TestConferenceHasRole()
    {
        var conference = _context.Conferences.Include(n => n.Roles).FirstOrDefault();
        Assert.NotNull(conference);
        Assert.AreEqual(1, conference.Roles.Count);
    }

    [Test]
    [Order(2)]
    public void TestConferenceHasNotParticipations()
    {
        var conference = _context.Conferences.FirstOrDefault();
        var participations = _context.Participations.Where(n => n.Role.Conference.ConferenceId == conference.ConferenceId).ToList();
        Assert.AreEqual(0, participations.Count);
    }

    [Test]
    [Order(3)]
    public void TestCreateParticipation()
    {
        var user = CreateATestUser("tester");
        var participation = new Participation()
        {
            Cost = 50,
            IsMainRole = false,
            Paid = 0,
            Role = teamRole,
            User = user
        };
        _context.Participations.Add(participation);
        _context.SaveChanges();
        Assert.AreEqual(1, _context.Participations.Count());
    }

    [Test]
    [Order(4)]
    public void TestThatConferenceHasOneParticipationSlot()
    {
        var participationSlots = _context.Participations
            .Where(n => n.Role.Conference.ConferenceId == TestConference.ConferenceId);
        Assert.AreEqual(1, participationSlots.Count());
    }

    [Test]
    [Order(5)]
    public void TestGetUsersOfAConference()
    {
        var usersOfConference = _context.Participations
            .Where(n => n.Role.Conference.ConferenceId == TestConference.ConferenceId)
            .Select(n => n.User)
            .Distinct();
        var userList = usersOfConference.ToList();
        Assert.AreEqual(1, userList.Count);
    }

    [Test]
    [Order(6)]
    public void TestGetUsersThatAreInTheTeam()
    {
        var usersInTeam = _context.Participations
            .Where(n => n.Role.Conference.ConferenceId == TestConference.ConferenceId &&
            n.Role is ConferenceTeamRole)
            .Select(n => n.User)
            .Distinct();
        Assert.AreEqual(1, usersInTeam.Count());
    }

    [Test]
    [Order(7)]
    public void TestHasNoDelegateUsers()
    {
        var delegateUsers = _context.Participations
            .Where(n => n.Role.Conference.ConferenceId == TestConference.ConferenceId &&
            n.Role is ConferenceDelegateRole)
            .Count();
        Assert.AreEqual(0, delegateUsers);
    }


    public ParticipationTests() : base("participationTest.db")
    {

    }
}
