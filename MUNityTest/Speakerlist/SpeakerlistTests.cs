using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using MUNityAngular.Models;
using MUNityAngular.Models.Conference;
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
        public void TestNextQuestionsResetsTime()
        {
            var instance = new SpeakerlistModel();
            var delegationOne = new DelegationModel();
            var delegationTwo = new DelegationModel();
            instance.NextQuestion();
            Assert.AreEqual(instance.Questiontime.TotalSeconds, instance.RemainingQuestionTime.TotalSeconds);
            instance.RemainingQuestionTime = new TimeSpan(0, 0, 2);
            Assert.AreEqual(2, instance.RemainingQuestionTime.TotalSeconds);
            instance.NextQuestion();
            Assert.AreEqual(instance.Questiontime.TotalSeconds, instance.RemainingQuestionTime.TotalSeconds);

        }

    }
}
