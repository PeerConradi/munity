using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using MUNity.Models.Resolution;
using MUNity.Extensions.ResolutionExtensions;
using System.Reflection;

namespace MunityNUnitTest.ResolutionWorkerTest
{
    public class ResolutionObserverTest
    {
        [Test]
        public void TestCreateInstance()
        {
            var resolution = new Resolution();
            var worker = new MUNity.Observers.ResolutionObserver(resolution);
            Assert.NotNull(worker);
        }

        [Test]
        public void TestChangeNameCallsNameChangedEvent()
        {
            var resolution = new Resolution();
            var observer = new MUNity.Observers.ResolutionObserver(resolution);
            bool wasRaised = false;
            observer.HeaderNameChanged += delegate { wasRaised = true; };
            resolution.Header.Name = "Neuer Name";
            Assert.IsTrue(wasRaised);
        }

        [Test]
        public void TestChangeFullNameCallsFullNameChangedEvent()
        {
            var resolution = new Resolution();
            var observer = new MUNity.Observers.ResolutionObserver(resolution);
            bool wasRaised = false;
            observer.HeaderFullNameChanged += delegate { wasRaised = true; };
            resolution.Header.FullName = "Neuer Name";
            Assert.IsTrue(wasRaised);
        }

        [Test]
        public void TestChangeTopicCallsFullNameChangedEvent()
        {
            var resolution = new Resolution();
            var observer = new MUNity.Observers.ResolutionObserver(resolution);
            bool wasRaised = false;
            observer.HeaderTopicChanged += delegate { wasRaised = true; };
            resolution.Header.Topic = "Neuer Name";
            Assert.IsTrue(wasRaised);
        }

        [Test]
        public void TestChangeAgendaItemCallsAgendaItemChangedEvent()
        {
            var resolution = new Resolution();
            var observer = new MUNity.Observers.ResolutionObserver(resolution);
            bool wasRaised = false;
            observer.HeaderAgendaItemChanged += delegate { wasRaised = true; };
            resolution.Header.AgendaItem = "Neuer Name";
            Assert.IsTrue(wasRaised);
        }

        [Test]
        public void TestEventChangeSession()
        {
            var resolution = new Resolution();
            var observer = new MUNity.Observers.ResolutionObserver(resolution);
            bool wasRaised = false;
            observer.HeaderSessionChanged += delegate { wasRaised = true; };
            resolution.Header.Session = "Neuer Name";
            Assert.IsTrue(wasRaised);
        }

        [Test]
        public void TestEventSubmitterName()
        {
            var resolution = new Resolution();
            var observer = new MUNity.Observers.ResolutionObserver(resolution);
            bool wasRaised = false;
            observer.HeaderSubmitterChanged += delegate { wasRaised = true; };
            resolution.Header.SubmitterName = "Neuer Name";
            Assert.IsTrue(wasRaised);
        }

        [Test]
        public void TestEventCommitteeName()
        {
            var resolution = new Resolution();
            var observer = new MUNity.Observers.ResolutionObserver(resolution);
            bool wasRaised = false;
            observer.HeaderCommitteeChanged += delegate { wasRaised = true; };
            resolution.Header.CommitteeName = "Neuer Name";
            Assert.IsTrue(wasRaised);
        }
    }
}
