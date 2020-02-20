using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using MUNityAngular.Models;
using MUNityAngular.Models.Conference;
using MUNityAngular.DataHandlers.Database;
using MUNityAngular.Util.Extenstions;

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

        [Test]
        public void TestTimeSpanFromString()
        {
            var ts_string = "00:01:00";
            var dt = ts_string.ToTimeSpan();
            Assert.IsTrue(dt.HasValue);
            Assert.AreEqual(60, (int)dt.Value.TotalSeconds);
        }

    }
}
