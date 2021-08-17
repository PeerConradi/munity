using MUNity.Database.Models.Resolution;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNityDatabaseTest.ResolutionTests
{
    public class BasicResolutionsTests : AbstractDatabaseTests
    {
        private ResaElement resolution;

        [Test]
        [Order(0)]
        public void TestCanCreateInstance()
        {
            resolution = new ResaElement();
            Assert.NotNull(this.resolution);   
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
            this.resolution.Topic = "New Resolution";
            Assert.AreEqual("New Resolution", this.resolution.Topic);
        }

        [Test]
        [Order(3)]
        public void TestResolutionCanSetName()
        {
            this.resolution.Name = "New Name";
            Assert.AreEqual("New Name", this.resolution.Name);
        }

        [Test]
        [Order(4)]
        public void TestResolutionCanSetFullName()
        {
            this.resolution.FullName = "New FullName";
            Assert.AreEqual("New FullName", this.resolution.FullName);
        }

        [Test]
        [Order(5)]
        public void TestResolutionCanSetAgendaItem()
        {
            this.resolution.AgendaItem = "Tagesordnungspunkt 1";
            Assert.AreEqual("Tagesordnungspunkt 1", this.resolution.AgendaItem);
        }

        [Test]
        [Order(6)]
        public void TestResolutionCanSetSession()
        {
            this.resolution.Session = "MUN-SH21.GV.1.1";
            Assert.AreEqual("MUN-SH21.GV.1.1", this.resolution.Session);
        }

        [Test]
        [Order(7)]
        public void TestResolutionCanSetSubmitterName()
        {
            this.resolution.SubmitterName = "Deutschland";
            Assert.AreEqual("Deutschland", this.resolution.SubmitterName);
        }

        [Test]
        [Order(8)]
        public void TestResolutionCanSetCommitteeName()
        {
            this.resolution.CommitteeName = "Generalversammlung";
            Assert.AreEqual("Generalversammlung", this.resolution.CommitteeName);
        }

        [Test]
        [Order(9)]
        public void TestResolutionCanSetCreatedDate()
        {
            var date = new DateTime(2021, 1, 1, 12, 0, 0);
            this.resolution.CreatedDate = date;
            Assert.AreEqual(date, this.resolution.CreatedDate);
        }

        [Test]
        [Order(10)]
        public void TestCanSetSupporterNames()
        {
            this.resolution.SupporterNames = "Frankreich, Spanien";
            Assert.AreEqual("Frankreich, Spanien", this.resolution.SupporterNames);
        }

        [Test]
        [Order(11)]
        public void TestResolutionPreambleParagraphsIsNotNull()
        {
            Assert.NotNull(this.resolution.PreambleParagraphs);
        }

        [Test]
        [Order(12)]
        public void TestResolutionOperativeParagraphsIsNotNull()
        {
            Assert.NotNull(this.resolution.OperativeParagraphs);
        }

        [Test]
        [Order(13)]
        public void TestResolutionAmendmentsIsNotNull()
        {
            Assert.NotNull(this.resolution.AddAmendments);
        }

        [Test]
        [Order(14)]
        public void TestResolutionCanBeStored()
        {
            _context.Resolutions.Add(resolution);
            var remark = _context.SaveChanges();
            Assert.AreEqual(1, remark);
        }

        [Test]
        [Order(15)]
        public void TestLoadingResolutionKeptValues()
        {
            var loadedResolution = _context.Resolutions.FirstOrDefault();
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
}
