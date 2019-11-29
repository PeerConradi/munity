using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using MUNityAngular.Models;
using MUNityAngular.DataHandlers.Database;

namespace MUNityTest.Speakerlist
{
    class SpeakerlistTests
    {
        [Test]
        public void TestCreateSpeakerlist()
        {
            var instance = new SpeakerlistModel();
            Assert.IsNotNull(instance);
        }

        [Test]
        public void TestAddSpeaker()
        {
            var instance = new SpeakerlistModel();
            instance.AddSpeaker(DelegationHandler.AllDefaultDelegations()[0]);
            Assert.AreEqual(1, instance.Speakers.Count);
            Assert.AreEqual(DelegationHandler.AllDefaultDelegations()[0], instance.Speakers[0]);
        }

        [Test]
        public void TestAddQuestion()
        {
            var instance = new SpeakerlistModel();
            instance.AddQuestion(DelegationHandler.AllDefaultDelegations()[0]);
            Assert.AreEqual(1, instance.Questions.Count);
            Assert.AreEqual(DelegationHandler.AllDefaultDelegations()[0], instance.Questions[0]);
        }

        [Test]
        public void TestRemoveSpeaker()
        {
            var instance = new SpeakerlistModel();
            instance.AddSpeaker(DelegationHandler.AllDefaultDelegations()[0]);
            instance.RemoveSpeaker(DelegationHandler.AllDefaultDelegations()[0]);
            Assert.AreEqual(0, instance.Speakers.Count);
        }

    }
}
