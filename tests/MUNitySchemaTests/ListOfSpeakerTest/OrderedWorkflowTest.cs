using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using MUNityBase;
using MUNity.ViewModels.ListOfSpeakers;

namespace MunityNUnitTest.ListOfSpeakerTest
{
    public class OrderedWorkflowTest
    {
        private ListOfSpeakersViewModel _instance;

        [Test]
        [Order(0)]
        public void TestCreateInstance()
        {
            this._instance = new ListOfSpeakersViewModel();
            this._instance.SpeakerTime = new TimeSpan(0, 3, 0);
            this._instance.QuestionTime = new TimeSpan(0, 0, 30);
            Assert.NotNull(_instance);
        }

        [Test]
        [Order(1)]
        public void TestAddFirstSpeaker()
        {
            var firstSpeaker = this._instance.AddSpeaker("First Speaker");
            Assert.NotNull(firstSpeaker);
            Assert.IsTrue(_instance.Speakers.Any());
            Assert.IsTrue(_instance.Speakers.Contains(firstSpeaker));
            Assert.AreEqual("First Speaker", firstSpeaker.Name);
        }

        [Test]
        [Order(2)]
        public void TestAddAnotherSpeaker()
        {
            var secondSpeaker = this._instance.AddSpeaker("Second Speaker");
            Assert.NotNull(secondSpeaker);
            Assert.IsTrue(_instance.Speakers.Count() == 2);
            Assert.AreEqual(secondSpeaker, _instance.Speakers.ElementAt(1));
        }

        [Test]
        [Order(3)]
        public void TestNextSpeaker()
        {
            _instance.NextSpeaker();
            Assert.AreEqual(1, _instance.Speakers.Count());
            Assert.NotNull(_instance.CurrentSpeaker);
            Assert.AreEqual("First Speaker", _instance.CurrentSpeaker.Name);
        }

        [Test]
        [Order(4)]
        public async Task TestStartFirstSpeaker()
        {
            _instance.ResumeSpeaker();
            Assert.IsTrue(_instance.Status == ESpeakerListStatus.Speaking);
            await Task.Delay(3000);
            Assert.AreEqual(177, (int)Math.Round(_instance.RemainingSpeakerTime.TotalSeconds));
        }

        [Test]
        [Order(5)]
        public void TestAddFirstQuestion()
        {
            var question = _instance.AddQuestion("First Question");
            Assert.NotNull(question);
            Assert.IsTrue(_instance.Questions.Any());
            Assert.IsTrue(_instance.Questions.Contains(question));
        }

        [Test]
        [Order(6)]
        public void TestAddAnotherQuestion()
        {
            var question = _instance.AddQuestion("Second Question");
            Assert.NotNull(question);
            Assert.IsTrue(_instance.Questions.Any());
            Assert.IsTrue(_instance.Questions.Contains(question));
            Assert.AreEqual(question, _instance.Questions.ElementAt(1));
        }

        [Test]
        [Order(7)]
        public void TestNextQuestion()
        {
            _instance.NextQuestion();
            Assert.IsTrue(_instance.Status == ESpeakerListStatus.SpeakerPaused);
            Assert.NotNull(_instance.CurrentSpeaker);
            Assert.AreEqual(1, _instance.Questions.Count());
            Assert.AreEqual(30, _instance.RemainingQuestionTime.TotalSeconds);
        }

        [Test]
        [Order(8)]
        public async Task TestStartQuestion()
        {
            _instance.ResumeQuestion();
            Assert.IsTrue(_instance.Status == ESpeakerListStatus.Question);
            await Task.Delay(3000);
            Assert.AreEqual(27, (int)Math.Round(_instance.RemainingQuestionTime.TotalSeconds));
        }

        [Test]
        [Order(9)]
        public async Task TestPauseQuestion()
        {
            _instance.Pause();
            var remainingSeconds = (int)Math.Round(_instance.RemainingQuestionTime.TotalSeconds);
            await Task.Delay(3000);
            Assert.AreEqual(remainingSeconds, (int)Math.Round(_instance.RemainingQuestionTime.TotalSeconds));
        }

        [Test]
        [Order(10)]
        public void TestNextSpeakerWhilePausedQuestion()
        {
            _instance.NextSpeaker();
            Assert.IsFalse(_instance.Speakers.Any());
            Assert.NotNull(_instance.CurrentSpeaker);
            Assert.IsTrue(_instance.Status == ESpeakerListStatus.Stopped);
            Assert.IsNull(_instance.CurrentQuestion);
            Assert.IsFalse(_instance.Questions.Any());
            Assert.AreEqual(180, (int)_instance.RemainingSpeakerTime.TotalSeconds);
            Assert.AreEqual(30, (int)_instance.RemainingQuestionTime.TotalSeconds);
        }
    }
}
