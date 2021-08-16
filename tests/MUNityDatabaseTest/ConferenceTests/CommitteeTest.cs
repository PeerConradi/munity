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
                CommitteeId = "MUN-SH2021-GV",
                Conference = conference,
                Article = "die",
                FullName = "Generalversammlung",
                Name = "Generalversammlung",
                CommitteeShort = "GV"
            };
            _context.Committees.Add(committee);
            _context.SaveChanges();
            Assert.AreEqual(1, _context.Committees.Count());
            Assert.AreEqual("die", committee.Article);
            Assert.AreEqual("Generalversammlung", committee.FullName);
            Assert.AreEqual("Generalversammlung", committee.Name);
            Assert.AreEqual("GV", committee.CommitteeShort);
        }

        [Test]
        [Order(2)]
        public void TestCommitteeIsStoredCorrectly()
        {
            var committee = _context.Committees
                .Include(n => n.ResolutlyCommittee)
                .Include(n => n.ChildCommittees)
                .Include(n => n.Conference)
                .FirstOrDefault(n => n.CommitteeId == "MUN-SH2021-GV");
            Assert.NotNull(committee);
            Assert.AreEqual("GV", committee.CommitteeShort);
            Assert.AreEqual("die", committee.Article);
            Assert.AreEqual("Generalversammlung", committee.Name);
            Assert.AreEqual("Generalversammlung", committee.FullName);
            var reloadConference = _context.Conferences.Include(n => n.Committees).FirstOrDefault();
            Assert.AreEqual(1, reloadConference.Committees.Count);
            Assert.IsNull(committee.ResolutlyCommittee);
            Assert.AreEqual(0, committee.ChildCommittees.Count);
            Assert.NotNull(committee.Conference);
        }

        [Test]
        [Order(4)]
        public void TestCreateASubCommittee()
        {
            var parentCommittee = _context.Committees.FirstOrDefault(n => n.CommitteeId == "MUN-SH2021-GV");
            var subCommittee = new Committee()
            {
                CommitteeId = "MUN-SH2021-AK",
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

            var rehashParent = _context.Committees
                .Include(n => n.ChildCommittees)
                .FirstOrDefault(n => n.CommitteeId == parentCommittee.CommitteeId);
            Assert.AreEqual(1, rehashParent.ChildCommittees.Count);
        }

        [Test]
        [Order(5)]
        public void TestAddATopic()
        {
            var committee = _context.Committees.FirstOrDefault(n => n.CommitteeId == "MUN-SH2021-GV");
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
            var committee = _context.Committees.FirstOrDefault(n => n.CommitteeId == "MUN-SH2021-GV");
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

        [Test]
        [Order(7)]
        public void TestGVHasTopics()
        {
            var gv = _context.Committees
                .Include(n => n.Topics)
                .FirstOrDefault(n => n.CommitteeId == "MUN-SH2021-GV");
            Assert.AreEqual(1, gv.Topics.Count);
        }

        [Test]
        [Order(8)]
        public void TestGVHasSessions()
        {
            var gv = _context.Committees
                .Include(n => n.Sessions)
                .FirstOrDefault(n => n.CommitteeId == "MUN-SH2021-GV");
            Assert.AreEqual(1, gv.Sessions.Count);
        }

        [Test]
        [Order(9)]
        public void TestHasNoResolutions()
        {
            var committee = _context.Committees
                .Include(n => n.Resolutions)
                .FirstOrDefault(n => n.CommitteeId == "MUN-SH2021-GV");
            Assert.AreEqual(0, committee.Resolutions.Count);
        }

        [Test]
        [Order(10)]
        public void TestTopicHasValues()
        {
            var topic = _context.CommitteeTopics
                .Include(n => n.Committee)
                .FirstOrDefault();
            Assert.NotNull(topic);
            Assert.AreEqual(1, topic.CommitteeTopicId);
            Assert.AreEqual("Weltfrieden", topic.TopicName);
            Assert.AreEqual("Maßnahmen zum sicherstellen des Friedens auf der Welt", topic.TopicFullName);
            Assert.AreEqual("In diesem Topic wird der Frieden auf der Erde debattiert. Hoffen wir mal das klappt, Krieg ist voll nicht spaßig.", topic.TopicDescription);
            Assert.AreEqual("WF", topic.TopicCode);
            Assert.NotNull(topic.Committee);
            Assert.AreEqual("MUN-SH2021-GV", topic.Committee.CommitteeId);
        }


        public CommitteeTest() : base ("committeeTest.db")
        {

        }
    }
}
