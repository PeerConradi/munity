using MUNity.Models.ListOfSpeakers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using MUNity.Extensions.LoSExtensions;
using System.Linq;
using System.Threading.Tasks;

namespace MunityNUnitTest.ListOfSpeakerTest
{
    /// <summary>
    /// Basic test cases for the list of speakers.
    /// </summary>
    public class BaseListOfSpeakerTest
    {
        /// <summary>
        /// Test creating an instance of a list of speakers.
        /// </summary>
        [Test]
        public void TestCreate()
        {
            var instance = new ListOfSpeakers();
            Assert.NotNull(instance);
            Assert.NotNull(instance.Speakers);
            Assert.NotNull(instance.Questions);
            Assert.IsNull(instance.CurrentQuestion);
            Assert.IsNull(instance.CurrentSpeaker);
        }

        /// <summary>
        /// Test add a speaker to the list of speakers.
        /// </summary>
        [Test]
        public void TestAddSpeaker()
        {
            var instance = new ListOfSpeakers();
            var speaker = instance.AddSpeaker("Speaker 1");
            Assert.NotNull(speaker);
            Assert.IsTrue(instance.Speakers.Any(n => n.Name == "Speaker 1"));
        }

        /// <summary>
        /// Test that calling the next speaker removes the speaker from the list.
        /// </summary>
        [Test]
        public void TestNextSpeakerRemovesFromList()
        {
            var instance = new ListOfSpeakers();
            var speaker = instance.AddSpeaker("Speaker 1");
            instance.NextSpeaker();
            Assert.IsFalse(instance.Speakers.Any());

        }

        /// <summary>
        /// Test that calling the next speaker sets the current speaker to the first speaker of the list.
        /// </summary>
        [Test]
        public void TestNextSpeakerSetsCurrentSpeaker()
        {
            var instance = new ListOfSpeakers();
            var speaker = instance.AddSpeaker("Speaker 1");
            instance.NextSpeaker();
            Assert.AreEqual(speaker, instance.CurrentSpeaker);
        }

        /// <summary>
        /// Test that when a Next speaker is set the Reamining Speaker time whill reset.
        /// In this case the list of speakers should be stopped and the SpeakerTime should be returned!
        /// </summary>
        [Test]
        public void TestNextSpeakerSettingTime()
        {
            var instance = new ListOfSpeakers();
            instance.SpeakerTime = new TimeSpan(0, 0, 0, 30);
            var speaker = instance.AddSpeaker("Speaker 1");
            instance.NextSpeaker();
            instance.StartSpeaker();
            Assert.IsTrue(instance.RemainingSpeakerTime.TotalSeconds >= 29 && instance.RemainingSpeakerTime.TotalSeconds < 31);
        }

        /// <summary>
        /// Test with a delay that the RemainingSpeakerTime is actually counting down.
        /// </summary>
        [Test]
        public async Task TestSpeakerListSpeakerCountDown()
        {
            var instance = new ListOfSpeakers();
            instance.SpeakerTime = new TimeSpan(0, 0, 0, 30);
            var speaker = instance.AddSpeaker("Speaker 1");
            instance.NextSpeaker();
            instance.StartSpeaker();
            await Task.Delay(5000);
            Assert.IsTrue(instance.RemainingSpeakerTime.TotalSeconds >= 24 && instance.RemainingSpeakerTime.TotalSeconds < 25);
        }

        [Test]
        public void TestAddQuestion()
        {
            var myListOfSpeakers = new ListOfSpeakers();
            var question = myListOfSpeakers.AddQuestion("Question 1");
            Assert.IsTrue(myListOfSpeakers.Questions.Any());
            Assert.IsTrue(myListOfSpeakers.Questions.Contains(question));
        }

        [Test]
        public void TestNextQuestionsSetsCurrentQuestion()
        {
            var instance = new ListOfSpeakers();
            var question = instance.AddQuestion("Question");
            instance.NextQuestion();
            Assert.IsTrue(instance.CurrentQuestion == question);
        }

        [Test]
        public void TestNextQuestionRemovesQuestionFromList()
        {
            var instance = new ListOfSpeakers();
            var question = instance.AddQuestion("Question");
            instance.NextQuestion();
            Assert.IsFalse(instance.Questions.Any());
        }

        [Test]
        public void TestStartQuestionFromStopResetsTime()
        {
            var instance = new ListOfSpeakers();
            instance.QuestionTime = new TimeSpan(0, 0, 30);
            var question = instance.AddQuestion("Question");
            instance.NextQuestion();
            instance.StartQuestion();
            // 29 to have a little puffer but this should compute instant
            Assert.IsTrue(instance.RemainingQuestionTime.TotalSeconds >= 29);
        }

        [Test]
        public async Task TestPausedQuestionRemainingTime()
        {
            var instance = new ListOfSpeakers();
            instance.QuestionTime = new TimeSpan(0, 0, 30);
            var question = instance.AddQuestion("Question");
            instance.NextQuestion();
            instance.StartQuestion();
            await Task.Delay(2000);
            instance.PauseQuestion();
            await Task.Delay(2000);
            // Remaining time should be 28 seconds still
            // we accept the values 27, 28 and 29
            Console.WriteLine(instance.RemainingQuestionTime.TotalSeconds);
            Assert.IsTrue(instance.RemainingQuestionTime.TotalSeconds >= 27 && instance.RemainingQuestionTime.TotalSeconds <= 29);
        }

        [Test]
        public void TestAnswerSetsTimeToQuestionTime()
        {
            var instance = new ListOfSpeakers();
            instance.QuestionTime = new TimeSpan(0, 0, 30);
            instance.AddSpeaker("Speaker");
            instance.NextSpeaker();
            instance.StartAnswer();
            Assert.IsTrue(instance.RemainingSpeakerTime.TotalSeconds >= 29 && instance.RemainingSpeakerTime.TotalSeconds <= 30);
        }

        [Test]
        public async Task TestPauseAnswer()
        {
            var instance = new ListOfSpeakers();
            instance.QuestionTime = new TimeSpan(0, 0, 30);
            instance.AddSpeaker("Speaker");
            instance.NextSpeaker();
            instance.StartAnswer();
            await Task.Delay(5000);
            instance.PauseSpeaker();
            await Task.Delay(2000);
            Assert.IsTrue(instance.RemainingSpeakerTime.TotalSeconds >= 24 && instance.RemainingSpeakerTime.TotalSeconds <= 26);
        }

        [Test]
        public async Task TestResumePausedSpeaker()
        {
            var instance = new ListOfSpeakers();
            instance.QuestionTime = new TimeSpan(0, 0, 30);
            instance.AddSpeaker("Speaker");
            instance.NextSpeaker();
            instance.StartAnswer();
            await Task.Delay(5000);
            instance.PauseSpeaker();
            await Task.Delay(2000);
            instance.ResumeSpeaker();
            await Task.Delay(3000);
            Assert.IsTrue(instance.RemainingSpeakerTime.TotalSeconds >= 21 && instance.RemainingSpeakerTime.TotalSeconds <= 23, "Remaining Speaker time should have been between 21 and 23 seconds but was {0}", instance.RemainingSpeakerTime.TotalSeconds);
            
        }

        [Test]
        public async Task TestResumeSpeaker()
        {
            var instance = new ListOfSpeakers();
            instance.SpeakerTime = new TimeSpan(0, 1, 0);
            instance.AddSpeaker("Speaker");
            instance.NextSpeaker();
            instance.StartSpeaker();
            await Task.Delay(3000);
            instance.PauseSpeaker();
            await Task.Delay(3000);
            instance.ResumeSpeaker();
            await Task.Delay(3000);
            Assert.IsTrue(instance.RemainingSpeakerTime.TotalSeconds >= 53 && instance.RemainingSpeakerTime.TotalSeconds <= 55, "Remaining Speaker time should have been between 53 and 55 seconds but was {0}", instance.RemainingSpeakerTime.TotalSeconds);
        }
    }
}
