using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using MUNity.ViewModels.ListOfSpeakers;
using MUNityBase;

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
            var instance = new ListOfSpeakersViewModel();
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
            var instance = new ListOfSpeakersViewModel();
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
            var instance = new ListOfSpeakersViewModel();
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
            var instance = new ListOfSpeakersViewModel();
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
            var instance = new ListOfSpeakersViewModel();
            instance.SpeakerTime = new TimeSpan(0, 0, 0, 30);
            var speaker = instance.AddSpeaker("Speaker 1");
            instance.NextSpeaker();
            instance.ResumeSpeaker();
            Assert.IsTrue(instance.RemainingSpeakerTime.TotalSeconds >= 29 && instance.RemainingSpeakerTime.TotalSeconds < 31);
        }

        [Test]
        public void TestAddQuestion()
        {
            var myListOfSpeakers = new ListOfSpeakersViewModel();
            var question = myListOfSpeakers.AddQuestion("Question 1");
            Assert.IsTrue(myListOfSpeakers.Questions.Any());
            Assert.IsTrue(myListOfSpeakers.Questions.Contains(question));
        }

        [Test]
        public void TestNextQuestionsSetsCurrentQuestion()
        {
            var instance = new ListOfSpeakersViewModel();
            var question = instance.AddQuestion("Question");
            instance.NextQuestion();
            Assert.IsTrue(instance.CurrentQuestion == question);
        }

        [Test]
        public void TestNextQuestionRemovesQuestionFromList()
        {
            var instance = new ListOfSpeakersViewModel();
            var question = instance.AddQuestion("Question");
            instance.NextQuestion();
            Assert.IsFalse(instance.Questions.Any());
        }

        [Test]
        public void TestStartQuestionFromStopResetsTime()
        {
            var instance = new ListOfSpeakersViewModel();
            instance.QuestionTime = new TimeSpan(0, 0, 30);
            var question = instance.AddQuestion("Question");
            instance.NextQuestion();
            instance.ResumeQuestion();
            // 29 to have a little puffer but this should compute instant
            Assert.IsTrue(instance.RemainingQuestionTime.TotalSeconds >= 29);
        }


        [Test]
        public void TestAnswerSetsTimeToQuestionTime()
        {
            var instance = new ListOfSpeakersViewModel();
            instance.QuestionTime = new TimeSpan(0, 0, 30);
            instance.AddSpeaker("Speaker");
            instance.NextSpeaker();
            instance.StartAnswer();
            Assert.IsTrue(instance.RemainingSpeakerTime.TotalSeconds >= 29 && instance.RemainingSpeakerTime.TotalSeconds <= 30);
        }



        [Test]
        public void TestNextSpeakerEmptyListCearsCurrent()
        {
            var instance = new ListOfSpeakersViewModel();
            instance.AddSpeaker("Speaker");
            instance.NextSpeaker();
            instance.NextSpeaker();
            Assert.IsNull(instance.CurrentSpeaker);
            Assert.IsFalse(instance.Speakers.Any());
        }

        [Test]
        public void TestNextQuestionEmptyListClearsCurrent()
        {
            var instance = new ListOfSpeakersViewModel();
            instance.AddQuestion("Question");
            instance.NextQuestion();
            instance.NextQuestion();
            Assert.IsNull(instance.CurrentQuestion);
            Assert.IsFalse(instance.Questions.Any());
        }

        [Test]
        public void TestNextSpeakerWhileActiveSpeakerPauses()
        {
            var instance = new ListOfSpeakersViewModel();
            instance.AddSpeaker("Speaker");
            instance.NextSpeaker();
            instance.ResumeSpeaker();
            instance.AddQuestion("New Question");
            instance.NextQuestion();
            Assert.AreEqual(ESpeakerListStatus.SpeakerPaused, instance.Status);
        }

        [Test]
        public void TestStartSpeakerWithoutCurrentStopsList()
        {
            var instance = new ListOfSpeakersViewModel();
            instance.ResumeSpeaker();
            Assert.AreEqual(ESpeakerListStatus.Stopped, instance.Status);
        }

        [Test]
        public void TestStartAnswerWithoutCurrentSpeakerStopsList()
        {
            var instance = new ListOfSpeakersViewModel();
            instance.StartAnswer();
            Assert.AreEqual(ESpeakerListStatus.Stopped, instance.Status);
        }

        [Test]
        public void TestStartQuestionWithoutCurrentStopsList()
        {
            var instance = new ListOfSpeakersViewModel();
            instance.ResumeQuestion();
            Assert.AreEqual(ESpeakerListStatus.Stopped, instance.Status);
        }

        [Test]
        public void TestPauseWhileSpeakingPausesSpeaker()
        {
            var instance = new ListOfSpeakersViewModel();
            instance.AddSpeaker("Speaker");
            instance.NextSpeaker();
            instance.ResumeSpeaker();
            Assert.AreEqual(ESpeakerListStatus.Speaking, instance.Status);
            instance.Pause();
            Assert.AreEqual(ESpeakerListStatus.SpeakerPaused, instance.Status);
        }

        [Test]
        public void TestPauseWHileQUestionPausesQuestion()
        {
            var instance = new ListOfSpeakersViewModel();
            instance.AddQuestion("Question");
            instance.NextQuestion();
            instance.ResumeQuestion();
            Assert.AreEqual(ESpeakerListStatus.Question, instance.Status);
            instance.Pause();
            Assert.AreEqual(ESpeakerListStatus.QuestionPaused, instance.Status);
        }

        [Test]
        public void TestPauseWhileAnswerPausesAnswer()
        {
            var instance = new ListOfSpeakersViewModel();
            instance.AddSpeaker("Speaker");
            instance.NextSpeaker();
            instance.StartAnswer();
            Assert.AreEqual(ESpeakerListStatus.Answer, instance.Status);
            instance.Pause();
            Assert.AreEqual(ESpeakerListStatus.AnswerPaused, instance.Status);
        }

        [Test]
        public void TestResumeSpeakerAsStartResetsTime()
        {
            var instance = new ListOfSpeakersViewModel();
            instance.SpeakerTime = new TimeSpan(0, 0, 30);
            instance.AddSpeaker("Speaker One");
            instance.NextSpeaker();
            instance.ResumeSpeaker();
            instance.StartSpeakerTime = DateTime.Now.ToUniversalTime().AddSeconds(-10);   // Faking that the last speaker already removed 10 secs
            Assert.AreEqual(20, Math.Round(instance.RemainingSpeakerTime.TotalSeconds));
            instance.AddSpeaker("Next Speaker");
            instance.NextSpeaker();
            instance.ResumeSpeaker();
            Assert.AreEqual(30, Math.Round(instance.RemainingSpeakerTime.TotalSeconds));
        }

        [Test]
        public void TestAddedQuestionKeepsOrder()
        {
            var instance = new ListOfSpeakersViewModel();
            instance.AddQuestion("Question 1");
            instance.AddQuestion("Question 2");
            Assert.AreEqual(0, instance.Questions.ElementAt(0).OrdnerIndex);
            Assert.AreEqual(1, instance.Questions.ElementAt(1).OrdnerIndex);
            Assert.AreEqual("Question 1", instance.Questions.ElementAt(0).Name);
            Assert.AreEqual("Question 2", instance.Questions.ElementAt(1).Name);
        }

        [Test]
        public void TestAddedSpeakerKeepsOrder()
        {
            var instance = new ListOfSpeakersViewModel();
            instance.AddSpeaker("Speaker 1");
            instance.AddSpeaker("Speaker 2");
            Assert.AreEqual(0, instance.Speakers.ElementAt(0).OrdnerIndex);
            Assert.AreEqual(1, instance.Speakers.ElementAt(1).OrdnerIndex);
            Assert.AreEqual("Speaker 1", instance.Speakers.ElementAt(0).Name);
            Assert.AreEqual("Speaker 2", instance.Speakers.ElementAt(1).Name);
        }

        [Test]
        public void TestResetSpeakerTime()
        {
            var instance = new ListOfSpeakersViewModel();
            instance.SpeakerTime = new TimeSpan(0, 0, 30);
            instance.AddSpeaker("Speaker 1");
            instance.NextSpeaker();
            instance.ResumeSpeaker();
            instance.AddSpeakerSeconds(-10);
            Assert.IsTrue(instance.RemainingSpeakerTime.TotalSeconds <= 20);
            instance.ResetSpeakerTime();
            Assert.AreEqual(30, instance.RemainingSpeakerTime.TotalSeconds, 0.5);
        }

        [Test]
        public void TestResetQuestionTIme()
        {
            var instance = new ListOfSpeakersViewModel();
            instance.AddQuestion("Question 1");
            instance.NextQuestion();
            instance.ResumeQuestion();
            instance.AddQuestionSeconds(-10);
            Assert.AreEqual(20, instance.RemainingQuestionTime.TotalSeconds, 0.5);
            instance.ResetQuestionTime();
            Assert.AreEqual(30, instance.RemainingQuestionTime.TotalSeconds, 0.5);
        }

        [Test]
        public void TestResumePausedSpeaker()
        {
            var instance = new ListOfSpeakersViewModel();
            instance.SpeakerTime = new TimeSpan(0, 0, 30);
            instance.AddSpeaker("Speaker");
            instance.NextSpeaker();
            instance.ResumeSpeaker();
            instance.AddSpeakerSeconds(-10);
            instance.Pause();
            Assert.AreEqual(20, instance.RemainingSpeakerTime.TotalSeconds, 0.5);
            instance.ResumeSpeaker();
            Assert.AreEqual(20, instance.RemainingSpeakerTime.TotalSeconds, 0.5);
        }

        [Test]
        public void TestResumePausedAnswer()
        {
            var instance = new ListOfSpeakersViewModel();
            instance.QuestionTime = new TimeSpan(0, 0, 30);
            instance.AddSpeaker("Speaker");
            instance.NextSpeaker();
            instance.StartAnswer();
            instance.AddSpeakerSeconds(-10);
            instance.Pause();
            Assert.AreEqual(20, instance.RemainingSpeakerTime.TotalSeconds, 0.5);
            instance.ResumeSpeaker();
            Assert.AreEqual(20, instance.RemainingSpeakerTime.TotalSeconds, 0.5);
        }

        [Test]
        public void TestResumePausedQuestion()
        {
            var instance = new ListOfSpeakersViewModel();
            instance.QuestionTime = new TimeSpan(0, 0, 30);
            instance.AddQuestion("Speaker");
            instance.NextQuestion();
            instance.ResumeQuestion();
            instance.AddQuestionSeconds(-10);
            instance.Pause();
            Assert.AreEqual(20, instance.RemainingQuestionTime.TotalSeconds, 0.5);
            instance.ResumeQuestion();
            Assert.AreEqual(20, instance.RemainingQuestionTime.TotalSeconds, 0.5);
        }

        [Test]
        public void TestClearCurrentSpeakerWhileSpeaking()
        {
            var instance = new ListOfSpeakersViewModel();
            instance.SpeakerTime = new TimeSpan(0, 0, 30);
            instance.AddSpeaker("Speaker");
            instance.NextSpeaker();
            instance.ResumeSpeaker();
            instance.ClearCurrentSpeaker();
            Assert.AreEqual(ESpeakerListStatus.Stopped, instance.Status);
        }

        [Test]
        public void TestClearCurrentQuestionWhileQuestionActive()
        {
            var instance = new ListOfSpeakersViewModel();
            instance.AddQuestion("Question");
            instance.NextQuestion();
            instance.ResumeQuestion();
            instance.ClearCurrentQuestion();
            Assert.AreEqual(ESpeakerListStatus.Stopped, instance.Status);
        }
    }
}
