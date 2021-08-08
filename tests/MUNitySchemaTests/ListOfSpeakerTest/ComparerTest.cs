using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using MUNity.ViewModels.ListOfSpeakers;

namespace MunityNUnitTest.ListOfSpeakerTest
{
    public class ComparerTest
    {
        [Test]
        public void TestInitWithSameId()
        {
            var listOne = new ListOfSpeakersViewModel();
            var listTwo = new ListOfSpeakersViewModel();
            listTwo.ListOfSpeakersId = listOne.ListOfSpeakersId;
            var result = listOne.CompareTo(listTwo);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void TestWithDifferentIds()
        {
            var listOne = new ListOfSpeakersViewModel();
            var listTwo = new ListOfSpeakersViewModel();
            Assert.AreNotEqual(listOne.ListOfSpeakersId, listTwo.ListOfSpeakersId);
            var result = listOne.CompareTo(listTwo);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void TestOneHasCurrentQuestionTwoDoesnt()
        {
            var listOne = new ListOfSpeakersViewModel()
            {
                ListOfSpeakersId = "fixedId"
            };
            var listTwo = new ListOfSpeakersViewModel()
            {
                ListOfSpeakersId = "fixedId"
            };
            listOne.AddQuestion("Test");
            listOne.NextQuestion();
            var result = listOne.CompareTo(listTwo);
            Assert.NotNull(listOne.CurrentQuestion);
            Assert.IsNull(listTwo.CurrentQuestion);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void TestTwoHasCurrentQuestionOneDoesnt()
        {
            var listOne = new ListOfSpeakersViewModel()
            {
                ListOfSpeakersId = "fixedId"
            };
            var listTwo = new ListOfSpeakersViewModel()
            {
                ListOfSpeakersId = "fixedId"
            };
            listTwo.AddQuestion("Test");
            listTwo.NextQuestion();
            var result = listOne.CompareTo(listTwo);
            Assert.NotNull(listTwo.CurrentQuestion);
            Assert.IsNull(listOne.CurrentQuestion);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void TestBothHaveCurrentQuestionButNotTheSame()
        {
            var listOne = new ListOfSpeakersViewModel()
            {
                ListOfSpeakersId = "fixedId"
            };
            var listTwo = new ListOfSpeakersViewModel()
            {
                ListOfSpeakersId = "fixedId"
            };
            listOne.AddQuestion("Test");
            listOne.NextQuestion();
            listTwo.AddQuestion("Test 2");
            listTwo.NextQuestion();
            var result = listOne.CompareTo(listTwo);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void TestListOneHasCurrentSpeakerListTwoDoesnt()
        {
            var listOne = new ListOfSpeakersViewModel()
            {
                ListOfSpeakersId = "fixedId"
            };
            var listTwo = new ListOfSpeakersViewModel()
            {
                ListOfSpeakersId = "fixedId"
            };
            listOne.AddSpeaker("Test");
            listOne.NextSpeaker();
            var result = listOne.CompareTo(listTwo);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void TestListTwoHasCurrentSpeakerListOneDoenst()
        {
            var listOne = new ListOfSpeakersViewModel()
            {
                ListOfSpeakersId = "fixedId"
            };
            var listTwo = new ListOfSpeakersViewModel()
            {
                ListOfSpeakersId = "fixedId"
            };

            listTwo.AddSpeaker("Test");
            listTwo.NextSpeaker();

            var result = listOne.CompareTo(listTwo);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void TestBothHaveASpeakerButNotTheSame()
        {
            var listOne = new ListOfSpeakersViewModel()
            {
                ListOfSpeakersId = "fixedId"
            };
            var listTwo = new ListOfSpeakersViewModel()
            {
                ListOfSpeakersId = "fixedId"
            };

            listOne.AddSpeaker("Test");
            listOne.NextSpeaker();

            listTwo.AddSpeaker("Test");
            listTwo.NextSpeaker();

            var result = listOne.CompareTo(listTwo);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void TestListClosedStateDiffers()
        {
            var listOne = new ListOfSpeakersViewModel()
            {
                ListOfSpeakersId = "fixedId",
                ListClosed = true
            };
            var listTwo = new ListOfSpeakersViewModel()
            {
                ListOfSpeakersId = "fixedId",
                ListClosed = false
            };

            var result = listOne.CompareTo(listTwo);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void TestNameDiffers()
        {
            var listOne = new ListOfSpeakersViewModel()
            {
                ListOfSpeakersId = "fixedId",
                Name = "List One"
            };
            var listTwo = new ListOfSpeakersViewModel()
            {
                ListOfSpeakersId = "fixedId",
                Name = "List Two"
            };

            var result = listOne.CompareTo(listTwo);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void TestPausedQuestionTimeDiffers()
        {
            var listOne = new ListOfSpeakersViewModel()
            {
                ListOfSpeakersId = "fixedId",
                PausedQuestionTime = new TimeSpan(0, 0, 30)
            };
            var listTwo = new ListOfSpeakersViewModel()
            {
                ListOfSpeakersId = "fixedId",
                PausedQuestionTime = new TimeSpan(0, 0, 29)
            };

            var result = listOne.CompareTo(listTwo);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void TestPausedSpeakerTimeDiffers()
        {
            var listOne = new ListOfSpeakersViewModel()
            {
                ListOfSpeakersId = "fixedId",
                PausedSpeakerTime = new TimeSpan(0, 0, 30)
            };
            var listTwo = new ListOfSpeakersViewModel()
            {
                ListOfSpeakersId = "fixedId",
                PausedSpeakerTime = new TimeSpan(0, 0, 29)
            };

            var result = listOne.CompareTo(listTwo);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void TestPublicIdDiffers()
        {
            var listOne = new ListOfSpeakersViewModel()
            {
                ListOfSpeakersId = "fixedId",
                PublicId = "ListOne"
            };
            var listTwo = new ListOfSpeakersViewModel()
            {
                ListOfSpeakersId = "fixedId",
                PublicId = "ListTwo"
            };

            var result = listOne.CompareTo(listTwo);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void QuestionsClosedDiffers()
        {
            var listOne = new ListOfSpeakersViewModel()
            {
                ListOfSpeakersId = "fixedId",
                QuestionsClosed = true
            };
            var listTwo = new ListOfSpeakersViewModel()
            {
                ListOfSpeakersId = "fixedId",
                QuestionsClosed = false
            };

            var result = listOne.CompareTo(listTwo);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void QuestionTimeDiffers()
        {
            var listOne = new ListOfSpeakersViewModel()
            {
                ListOfSpeakersId = "fixedId",
                QuestionTime = new TimeSpan(0, 0, 30)
            };
            var listTwo = new ListOfSpeakersViewModel()
            {
                ListOfSpeakersId = "fixedId",
                QuestionTime = new TimeSpan(0, 0, 29)
            };

            var result = listOne.CompareTo(listTwo);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void SpeakerTimeDiffers()
        {
            var listOne = new ListOfSpeakersViewModel()
            {
                ListOfSpeakersId = "fixedId",
                SpeakerTime = new TimeSpan(0, 0, 30)
            };
            var listTwo = new ListOfSpeakersViewModel()
            {
                ListOfSpeakersId = "fixedId",
                SpeakerTime = new TimeSpan(0, 0, 29)
            };

            var result = listOne.CompareTo(listTwo);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void StartQuestimTimeDiffers()
        {
            var listOne = new ListOfSpeakersViewModel()
            {
                ListOfSpeakersId = "fixedId",
                StartQuestionTime = new DateTime(2021, 1,1,12,0,0)
            };
            var listTwo = new ListOfSpeakersViewModel()
            {
                ListOfSpeakersId = "fixedId",
                StartQuestionTime = new DateTime(2021, 1, 1, 12, 0, 1)
            };

            var result = listOne.CompareTo(listTwo);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void StartSpeakerTimeDiffers()
        {
            var listOne = new ListOfSpeakersViewModel()
            {
                ListOfSpeakersId = "fixedId",
                StartSpeakerTime = new DateTime(2021, 1, 1, 12, 0, 0)
            };
            var listTwo = new ListOfSpeakersViewModel()
            {
                ListOfSpeakersId = "fixedId",
                StartSpeakerTime = new DateTime(2021, 1, 1, 12, 0, 1)
            };

            var result = listOne.CompareTo(listTwo);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void StatusDiffers()
        {
            var listOne = new ListOfSpeakersViewModel()
            {
                ListOfSpeakersId = "fixedId",
                Status = MUNityBase.ESpeakerListStatus.Answer
            };
            var listTwo = new ListOfSpeakersViewModel()
            {
                ListOfSpeakersId = "fixedId",
                Status = MUNityBase.ESpeakerListStatus.Stopped
            };

            var result = listOne.CompareTo(listTwo);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void SpeakersDiffer()
        {
            var listOne = new ListOfSpeakersViewModel()
            {
                ListOfSpeakersId = "fixedId",
            };
            var listTwo = new ListOfSpeakersViewModel()
            {
                ListOfSpeakersId = "fixedId",
            };

            listOne.AddSpeaker("Speaker");
            listTwo.AddSpeaker("Speaker2");

            var result = listOne.CompareTo(listTwo);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void TestMoreSpeakersReturnsFalse()
        {
            var instanceOne = new ListOfSpeakersViewModel();
            var instanceTwo = new ListOfSpeakersViewModel();
            instanceTwo.ListOfSpeakersId = instanceOne.ListOfSpeakersId;
            instanceOne.AddSpeaker("Speaker 1");
            var result = instanceOne.CompareTo(instanceTwo);
            Assert.AreNotEqual(0, result);
        }
    }
}
