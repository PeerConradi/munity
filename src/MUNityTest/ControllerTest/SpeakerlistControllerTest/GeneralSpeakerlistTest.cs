using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Moq;
using MUNity.Models.ListOfSpeakers;
using MUNity.Schema.ListOfSpeakers;
using MUNityCore.Controllers;
using MUNityCore.DataHandlers.EntityFramework;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNityCoreTest.ControllerTest.SpeakerlistControllerTest
{
    public class GeneralSpeakerlistTest
    {
        private static MunityContext _context;

        private Mock<IHubContext<MUNityCore.Hubs.SpeakerListHub, MUNity.Hubs.ITypedListOfSpeakerHub>> mockHub;

        private MUNityCore.Services.SpeakerlistService service;

        private SpeakerlistController controller;

        private string listId;

        [OneTimeSetUp]
        public void SetupBefore()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MunityContext>();
            optionsBuilder.UseSqlite("Data Source=test_conferenceservice.db");
            _context = new MunityContext(optionsBuilder.Options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            mockHub = new Mock<IHubContext<MUNityCore.Hubs.SpeakerListHub, MUNity.Hubs.ITypedListOfSpeakerHub>>();
            service = new MUNityCore.Services.SpeakerlistService(_context);
            controller = new SpeakerlistController(mockHub.Object, service);
        }

        [Test]
        [Order(1)]
        public void TestCreateList()
        {
            
            var result = controller.CreateListOfSpeaker();
            var resultType = result.Result as OkObjectResult;
            Assert.NotNull(resultType);
            var dto = resultType.Value as CreatedResponse;
            Assert.IsTrue(result.Result is OkObjectResult);
            Assert.NotNull(dto);
            Assert.IsFalse(string.IsNullOrEmpty(dto.ListOfSpeakersId));
            Assert.IsFalse(string.IsNullOrEmpty(dto.AccessToken));
            this.listId = dto.ListOfSpeakersId;
            Console.WriteLine($"Created a list of speakers with id: {this.listId}");
        }

        [Test]
        [Order(2)]
        public void TestGetList()
        {
            var result = controller.GetSpeakerlist(listId);
            var resultType = result.Result as OkObjectResult;
            Assert.NotNull(resultType, $"Should not have been null because it should be an OkObjectResult but was {result.Result.GetType()}");
            var list = resultType.Value as ListOfSpeakers;
            Assert.NotNull(list);
        }

        [Test]
        [Order(3)]
        public void TestSetListSettings()
        {
            var dto = new SetListSettingsBody()
            {
                ListOfSpeakersId = listId,
                Token = "test",
                SpeakerTime = new TimeSpan(0, 2, 0),
                QuestionTime = new TimeSpan(0, 0, 20)
            };
            var result = controller.SetSettings(dto);
            Assert.IsTrue(result is OkResult);

            var listResponse = controller.GetSpeakerlist(listId);
            var listObjectResult = listResponse.Result as OkObjectResult;
            Assert.NotNull(listObjectResult);
            var list = listObjectResult.Value as ListOfSpeakers;
            Assert.NotNull(list);
            Assert.AreEqual(2, list.SpeakerTime.TotalMinutes);
            Assert.AreEqual(20, list.QuestionTime.TotalSeconds);
        }

        [Test]
        [Order(4)]
        public async Task TestAddSpeaker()
        {
            var dto = new AddSpeakerBody()
            {
                ListOfSpeakersId = listId,
                Token = "test",
                Name = "Germany",
                Iso = "DE"
            };
            var result = await controller.AddSpeaker(dto);
            Assert.IsTrue(result is OkResult);

            var list = GetTestSpeakerlist();
            Assert.NotNull(list);
            Assert.IsTrue(list.Speakers.Any(n => n.Name == "Germany"));
            Assert.AreEqual(1, list.Speakers.Count());
        }

        [Test]
        [Order(5)]
        public async Task TestNextSpeaker()
        {
            var dto = new ListOfSpeakersRequest()
            {
                ListOfSpeakersId = listId,
                Token = "test"
            };
            var result = await controller.NextSpeaker(dto);
            Assert.IsTrue(result is OkResult, $"Expected OkResult but got {result.GetType()}");

            var testList = GetTestSpeakerlist();
            Assert.NotNull(testList);
            Assert.NotNull(testList.CurrentSpeaker);
            Assert.AreEqual(0, testList.Speakers.Count());
        }

        [Test]
        [Order(6)]
        public void TestStartSpeaker()
        {
            var dto = new ListOfSpeakersRequest()
            {
                ListOfSpeakersId = listId,
                Token = "test"
            };

            var result = controller.StartSpeaker(dto);
            Assert.IsTrue(result is OkResult);
            var list = GetTestSpeakerlist();
            Assert.NotNull(list);
            Assert.IsTrue(list.Status == ListOfSpeakers.EStatus.Speaking);
            Assert.Greater((int)list.RemainingSpeakerTime.TotalSeconds, 118);
            Assert.Less((int)list.RemainingSpeakerTime.TotalSeconds, 121);
        }

        [Test]
        [Order(7)]
        public async Task TestAddQuestionOne()
        {
            var dto = new AddSpeakerBody()
            {
                ListOfSpeakersId = listId,
                Token = "test",
                Name = "Question 1",
                Iso = "Q1"
            };
            var result = await controller.AddQuestion(dto);
            Assert.IsTrue(result is OkResult);

            var list = GetTestSpeakerlist();
            Assert.NotNull(list);
            Assert.IsTrue(list.Questions.Any(n => n.Name == "Question 1"));
            Assert.AreEqual(1, list.Questions.Count());
        }

        [Test]
        [Order(8)]
        public async Task TestAddQuestionTwo()
        {
            var dto = new AddSpeakerBody()
            {
                ListOfSpeakersId = listId,
                Token = "test",
                Name = "Question 2",
                Iso = "Q2"
            };
            var result = await controller.AddQuestion(dto);
            Assert.IsTrue(result is OkResult);

            var list = GetTestSpeakerlist();
            Assert.NotNull(list);
            Assert.IsTrue(list.Questions.Any(n => n.Name == "Question 2"));
            Assert.AreEqual(2, list.Questions.Count());
        }

        [Test]
        [Order(9)]
        public async Task TestNextQuestion()
        {
            var dto = new ListOfSpeakersRequest()
            {
                ListOfSpeakersId = listId,
                Token = "test"
            };
            var result = await controller.NextQuestion(dto);
            Assert.IsTrue(result is OkResult, $"Expected an ok result but got: {result.GetType()}");
            var list = GetTestSpeakerlist();
            Assert.NotNull(list.CurrentQuestion);
            Assert.NotNull(list.CurrentSpeaker);
            Assert.IsTrue(list.Status == ListOfSpeakers.EStatus.SpeakerPaused, $"Expected the list of speakers to be pause the speaker after next question but was: {list.Status}");
            Assert.AreEqual(1, list.Questions.Count());
        }

        [Test]
        [Order(10)]
        public void TestStartQuestion()
        {
            var dto = new ListOfSpeakersRequest()
            {
                ListOfSpeakersId = listId,
                Token = "test"
            };
            var result = controller.StartQuestion(dto);
            Assert.IsTrue(result is OkResult);

            var list = GetTestSpeakerlist();
            Assert.NotNull(list);
            Assert.AreEqual(ListOfSpeakers.EStatus.Question, list.Status);
            Assert.GreaterOrEqual(list.RemainingQuestionTime.TotalSeconds, 19);
            Assert.LessOrEqual(list.RemainingQuestionTime.TotalSeconds, 20);
        }

        [Test]
        [Order(11)]
        public async Task TestRemoveQuestionTwo()
        {
            var list = GetTestSpeakerlist();
            Assert.AreEqual(1, list.Questions.Count());
            var speakerTwo = list.Questions.First();
            var dto = new RemoveSpeakerBody()
            {
                ListOfSpeakersId = listId,
                SpeakerId = speakerTwo.Id,
                Token = "test"
            };
            var result = await controller.RemoveSpeakerFromList(dto);
            Assert.IsTrue(result is OkResult);
            // Reload list
            list = GetTestSpeakerlist();
            Assert.AreEqual(0, list.Questions.Count());
        }

        [Test]
        [Order(12)]
        public void TestGiveQuestionExtraTime()
        {
            var list = GetTestSpeakerlist();
            // Prerequirements
            Assert.NotNull(list);
            Assert.NotNull(list.CurrentQuestion);

            var startRemainingSeconds = list.RemainingQuestionTime.TotalSeconds;
            var dto = new AddSpeakerSeconds()
            {
                ListOfSpeakersId = list.ListOfSpeakersId,
                Seconds = 10,
                Token = "test"
            };

            var result = controller.AddQuestionSeconds(dto);
            Assert.IsTrue(result is OkResult);
            list = GetTestSpeakerlist();
            Assert.GreaterOrEqual(list.RemainingQuestionTime.TotalSeconds, startRemainingSeconds + 9);
        }

        [Test]
        [Order(13)]
        public void TestStartAnswer()
        {
            var dto = new ListOfSpeakersRequest()
            {
                ListOfSpeakersId = listId,
                Token = "test"
            };
            var result = controller.StartAnswer(dto);
            Assert.IsTrue(result is OkResult);
            var list = GetTestSpeakerlist();
            Assert.AreEqual(ListOfSpeakers.EStatus.Answer, list.Status);
            Assert.GreaterOrEqual(list.RemainingSpeakerTime.TotalSeconds, 19);
            Assert.LessOrEqual(list.RemainingSpeakerTime.TotalSeconds, 20);
        }

        [Test]
        [Order(14)]
        public void TestGiveSpeakerExtraTime()
        {
            var list = GetTestSpeakerlist();
            // Prerequirements
            Assert.NotNull(list);
            Assert.NotNull(list.CurrentQuestion);

            var startRemainingSeconds = list.RemainingSpeakerTime.TotalSeconds;
            var dto = new AddSpeakerSeconds()
            {
                ListOfSpeakersId = list.ListOfSpeakersId,
                Seconds = 10,
                Token = "test"
            };

            var result = controller.AddSpeakerSeconds(dto);
            Assert.IsTrue(result is OkResult);
            list = GetTestSpeakerlist();
            Assert.GreaterOrEqual(list.RemainingSpeakerTime.TotalSeconds, startRemainingSeconds + 9);
        }

        [Test]
        public void TestClearSpeaker()
        {
            var list = GetTestSpeakerlist();
            Assert.NotNull(list);
            Assert.NotNull(list.CurrentSpeaker);
            var dto = new ListOfSpeakersRequest()
            {
                ListOfSpeakersId = listId,
                Token = "test"
            };
            var result = controller.ClearSpeaker(dto);
            Assert.IsTrue(result is OkResult);
            list = GetTestSpeakerlist();
            Assert.IsNull(list.CurrentSpeaker);
            Assert.AreNotEqual(ListOfSpeakers.EStatus.Speaking, list.Status);
            Assert.AreNotEqual(ListOfSpeakers.EStatus.SpeakerPaused, list.Status);
        }

        [Test]
        public void TestClearQuestion()
        {
            var list = GetTestSpeakerlist();
            Assert.NotNull(list);
            Assert.NotNull(list.CurrentQuestion);
            var dto = new ListOfSpeakersRequest()
            {
                ListOfSpeakersId = listId,
                Token = "test"
            };
            var result = controller.ClearQuestion(dto);
            Assert.IsTrue(result is OkResult);
            list = GetTestSpeakerlist();
            Assert.IsNull(list.CurrentQuestion);
            Assert.AreNotEqual(ListOfSpeakers.EStatus.Question, list.Status);
            Assert.AreNotEqual(ListOfSpeakers.EStatus.QuestionPaused, list.Status);
        }

        private ListOfSpeakers GetTestSpeakerlist()
        {
            var response = controller.GetSpeakerlist(listId);
            if (response.Result is OkObjectResult result)
            {
                return result.Value as ListOfSpeakers;
            }
            return null;
        }
    }
}
