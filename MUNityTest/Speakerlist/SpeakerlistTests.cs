﻿using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using MUNityCore.Models;
using MUNityCore.Util.Extensions;
using MUNityCore.Models.Conference;
using MUNityCore.Models.ListOfSpeakers;

namespace MUNityTest.Speakerlist
{
    class SpeakerlistTests
    {
        [Test]
        public void TestCreateSpeakerlist()
        {
            var instance = new ListOfSpeakers();
            Assert.IsNotNull(instance);
        }

        [Test]
        public void TestNextQuestionsResetsTime()
        {
            var instance = new ListOfSpeakers();
            var delegationOne = new Speaker();
            var delegationTwo = new Speaker();
            instance.AddQuestion(delegationOne);
            instance.AddQuestion(delegationTwo);
            instance.NextQuestion();
            Assert.AreEqual(instance.QuestionTime.TotalSeconds, instance.RemainingQuestionTime.TotalSeconds);
            instance.RemainingQuestionTime = new TimeSpan(0, 0, 2);
            Assert.AreEqual(2, instance.RemainingQuestionTime.TotalSeconds);
            instance.NextQuestion();
            Assert.AreEqual(instance.QuestionTime.TotalSeconds, instance.RemainingQuestionTime.TotalSeconds);
        }

        [Test]
        public void TestNextSpeakerResetsTime()
        {
            var instance = new ListOfSpeakers();
            var delegationOne = new Speaker();
            var delegationTwo = new Speaker();
            instance.AddSpeaker(delegationOne);
            instance.AddSpeaker(delegationTwo);
            instance.NextSpeaker();
            Assert.AreEqual(instance.SpeakerTime.TotalSeconds, instance.RemainingSpeakerTime.TotalSeconds);
            instance.RemainingSpeakerTime = new TimeSpan(0, 0, 2);
            Assert.AreEqual(2, instance.RemainingSpeakerTime.TotalSeconds);
            instance.NextSpeaker();
            Assert.AreEqual(instance.SpeakerTime.TotalSeconds, instance.RemainingSpeakerTime.TotalSeconds);

        }

        [Test]
        public void TestTimeSpanFromString()
        {
            var ts_string = "00:01:00";
            var dt = ts_string.ToTimeSpan();
            Assert.IsTrue(dt.HasValue);
            Assert.AreEqual(60, (int)dt.Value.TotalSeconds);
        }

        [Test]
        public void RemainSpeakerTimeDecrisingTest()
        {
            var speakerlist = new ListOfSpeakers { SpeakerTime = new TimeSpan(0, 1, 0)};
            var delegation = new Speaker {Name = "Test"};
            speakerlist.AddSpeaker(delegation);
            speakerlist.NextSpeaker();
            speakerlist.StartSpeaker();
            System.Threading.Thread.Sleep(5000);
            Console.WriteLine(speakerlist.RemainingSpeakerTime);
            Assert.IsTrue(speakerlist.RemainingSpeakerTime < new TimeSpan(0, 0, 57));
            Assert.IsTrue(speakerlist.RemainingSpeakerTime > new TimeSpan(0, 0, 50));
        }

        [Test]
        public void StartStopShouldContinueTest()
        {
            var listOfSpeakers = new ListOfSpeakers { SpeakerTime = new TimeSpan(0, 1, 0)};
            var delegation = new Speaker {Name = "Test"};
            listOfSpeakers.AddSpeaker(delegation);
            listOfSpeakers.NextSpeaker();
            listOfSpeakers.StartSpeaker();
            System.Threading.Thread.Sleep(5000);
            listOfSpeakers.PauseSpeaker();
            System.Threading.Thread.Sleep(5000);
            listOfSpeakers.StartSpeaker();
            Assert.IsTrue(listOfSpeakers.RemainingSpeakerTime < new TimeSpan(0, 0, 57));
            Assert.IsTrue(listOfSpeakers.RemainingSpeakerTime > new TimeSpan(0, 0, 50));
        }

        [Test]
        public void RemainingQuestionTimeDecreasingTest()
        {
            var listOfSpeakers = new ListOfSpeakers { QuestionTime = new TimeSpan(0, 1, 0)};
            var delegation = new Speaker {Name = "Test"};
            listOfSpeakers.AddQuestion(delegation);
            listOfSpeakers.NextQuestion();
            listOfSpeakers.StartQuestion();
            System.Threading.Thread.Sleep(5000);
            listOfSpeakers.PauseSpeaker();
            System.Threading.Thread.Sleep(5000);
            listOfSpeakers.StartQuestion();
            Console.WriteLine(listOfSpeakers.RemainingQuestionTime);
            Assert.IsTrue(listOfSpeakers.RemainingQuestionTime < new TimeSpan(0, 0, 57));
            Assert.IsTrue(listOfSpeakers.RemainingQuestionTime > new TimeSpan(0, 0, 50));
        }

    }
}
