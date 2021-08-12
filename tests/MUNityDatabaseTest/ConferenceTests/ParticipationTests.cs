using MUNity.Database.Models.Conference;
using MUNity.Database.Models.Conference.Roles;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNityDatabaseTest.ConferenceTests
{
    public class ParticipationTests : AbstractDatabaseTests
    {

        private ConferenceTeamRole teamRole;

        [Test]
        [Order(0)]
        public void TestCreateARole()
        {
            var leaderAuth = new RoleAuth()
            {
                Conference = TestConference,
                CanEditConferenceSettings = true,
                CanEditParticipations = true,
                CanSeeApplications = true,
                PowerLevel = 1,
                RoleAuthName = "Project-Owner"
            };
            _context.RoleAuths.Add(leaderAuth);

            var leaderRoleGroup = new TeamRoleGroup()
            {
                FullName = "die Projektleitung",
                Name = "Projektleitung",
                TeamRoleGroupShort = "PL",
                GroupLevel = 1
            };
            _context.TeamRoleGroups.Add(leaderRoleGroup);

            var leaderRole = new ConferenceTeamRole()
            {
                Conference = TestConference,
                IconName = "pl",
                RoleFullName = "Projektleiter",
                RoleAuth = leaderAuth,
                RoleName = "Projektleiter",
                RoleShort = "PL",
                TeamRoleGroup = leaderRoleGroup,
                TeamRoleLevel = 1,
            };
            this.teamRole = leaderRole;
            _context.TeamRoles.Add(leaderRole);
            _context.SaveChanges();
        }

        [Test]
        [Order(1)]
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
        [Order(2)]
        public void TestThatConferenceHasOneParticipationSlot()
        {
            var participationSlots = _context.Participations
                .Where(n => n.Role.Conference.ConferenceId == TestConference.ConferenceId);
            Assert.AreEqual(1, participationSlots.Count());
        }

        [Test]
        [Order(3)]
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
        [Order(4)]
        public void TestGetUsersThatAreInTheTeam()
        {
            var usersInTeam = _context.Participations
                .Where(n => n.Role.Conference.ConferenceId == TestConference.ConferenceId &&
                n.Role is ConferenceTeamRole)
                .Select(n => n.User)
                .Distinct();
            Assert.AreEqual(1, usersInTeam.Count());
        }

        public ParticipationTests() : base ("participationTest.db")
        {

        }
    }
}
