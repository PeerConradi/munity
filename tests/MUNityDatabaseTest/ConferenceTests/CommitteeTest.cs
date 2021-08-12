using Microsoft.EntityFrameworkCore;
using MUNity.Database.Models.Conference;
using MUNity.Database.Models.Session;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNityDatabaseTest.ConferenceTests
{
    public class CommitteeTest : AbstractDatabaseTests
    {
        Conference conference;

        [Order(0)]
        [Test]
        public void TestSetupTestEnvironment()
        {
            conference = new Conference()
            {
                ConferenceShort = "MUN-SH 2021",
                CreationDate = DateTime.Now,
                Name = "MUN Schleswig-Holstein 2021",
                FullName = "Model United Nations Schleswig-Holstein 2021"
            };
            _context.Conferences.Add(conference);
            _context.SaveChanges();
            Assert.AreEqual(1, _context.Conferences.Count());
        }

        [Test]
        [Order(1)]
        public void TestCreateACommittee()
        {
            var committee = new Committee()
            {
                Conference = conference,
                Article = "die",
                FullName = "Generalversammlung",
                Name = "Generalversammlung"
            };
            _context.Committees.Add(committee);
            _context.SaveChanges();
            Assert.AreEqual(1, _context.Committees.Count());
        }

        [Test]
        [Order(2)]
        public void TestCommitteeIsStoredCorrectly()
        {
            var committee = _context.Committees.FirstOrDefault();
            Assert.NotNull(committee);
            Assert.AreEqual("die", committee.Article);
            Assert.AreEqual("Generalversammlung", committee.Name);
            Assert.AreEqual("Generalversammlung", committee.FullName);
            var reloadConference = _context.Conferences.Include(n => n.Committees).FirstOrDefault();
            Assert.AreEqual(1, reloadConference.Committees.Count);
        }

        [Test]
        [Order(4)]
        public void TestCreateASubCommittee()
        {
            var parentCommittee = _context.Committees.FirstOrDefault();
            var subCommittee = new Committee()
            {
                Article = "die",
                Name = "Abrüstungskommission",
                FullName = "Abrüstungskommission",
                ResolutlyCommittee = parentCommittee
            };
            _context.Committees.Add(subCommittee);
            _context.SaveChanges();
            Assert.AreEqual(2, _context.Committees.Count());
            Assert.AreEqual(1, _context.Committees.Count(n => n.ResolutlyCommittee.CommitteeId == parentCommittee.CommitteeId));
            var reloadParent = _context.Committees.Include(n => n.ChildCommittees).FirstOrDefault(n => n.CommitteeId == parentCommittee.CommitteeId);
            Assert.AreEqual(1, reloadParent.ChildCommittees.Count);
        }

        [Test]
        [Order(5)]
        public void TestAddATopic()
        {
            var committee = _context.Committees.FirstOrDefault();
            var topic = new CommitteeTopic()
            {
                Committee = committee,
                TopicCode = "WF",
                TopicName = "Weltfrieden",
                TopicFullName = "Maßnahmen zum sicherstellen des Friedens auf der Welt",
                TopicDescription = "In diesem Topic wird der Frieden auf der Erde debattiert. Hoffen wir mal das klappt, Krieg ist voll nicht spaßig."
            };
            _context.CommitteeTopics.Add(topic);
            _context.SaveChanges();
            Assert.AreEqual(1, _context.CommitteeTopics.Count());
        }

        [Test]
        [Order(6)]
        public void TestCreateASession()
        {
            var committee = _context.Committees.FirstOrDefault();
            var session = new CommitteeSession()
            {
                Committee = committee,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddHours(1),
                Name = "Demo Session",
            };
            _context.CommitteeSessions.Add(session);
            _context.SaveChanges();
            Assert.AreEqual(1, _context.CommitteeSessions.Count());
            var committeeWithSessions = _context.Committees
                .Include(n => n.Sessions)
                .FirstOrDefault(n => n.CommitteeId == committee.CommitteeId);
            Assert.AreEqual(1, committeeWithSessions.Sessions.Count);
        }

        public CommitteeTest() : base ("committeeTest.db")
        {

        }
    }
}
