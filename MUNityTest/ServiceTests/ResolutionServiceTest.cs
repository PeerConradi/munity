using MongoDB.Driver;
using MUNityAngular.Services;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MUNityAngular.DataHandlers.Database;

namespace MUNityTest.ServiceTests
{
    class ResolutionServiceTest
    {
        private ConnectionInfo _env;

        [SetUp]
        public void Setup()
        {
            //Testinformationen laden
            var TestSettingsPath = Path.Combine(Environment.CurrentDirectory, "NeededFiles/testsettings.json");

            if (!File.Exists(TestSettingsPath))
                Assert.Fail("The Test configuration is missing!");
            var settingsTest = File.ReadAllText(TestSettingsPath);
            _env = Newtonsoft.Json.JsonConvert.DeserializeObject<ConnectionInfo>(settingsTest);

            //Create the empty Database
            using (var connection = new MySqlConnection(_env.ConnectionString))
            {

                connection.Open();
                //Vorhandene Datenbank entfernen
                var cmdStr = "DROP DATABASE IF EXISTS `" + _env.DatabaseName + "`";
                var cmdDrop = new MySqlCommand(cmdStr, connection);
                cmdDrop.ExecuteNonQuery();

                cmdStr = "CREATE DATABASE `" + _env.DatabaseName + "`";
                var cmdCreate = new MySqlCommand(cmdStr, connection);
                cmdCreate.ExecuteNonQuery();

                //Wechseln auf die neue Datenbank
                connection.ChangeDatabase(_env.DatabaseName);
                this._env.ConnectionString += ";database=" + _env.DatabaseName;

                //Schaue wo die munitysql hin kopiert wird...
                var path = Path.Combine(Environment.CurrentDirectory, "NeededFiles/munity.sql");
                var sql = File.ReadAllText(path);
                cmdStr = sql;
                var cmdFill = new MySqlCommand(sql, connection);
                var filled = cmdFill.ExecuteNonQuery();
                Console.WriteLine(filled + " Test Data added");
            }

            //Create MongoDB
            var client = new MongoClient(_env.MunityMongoDatabaseSettings.ConnectionString);
            client.DropDatabase(_env.MunityMongoDatabaseSettings.DatabaseName);
            client.GetDatabase(_env.MunityMongoDatabaseSettings.DatabaseName);
        }

        [Test]
        public void CreateResolutionTest()
        {
            Console.WriteLine("This test is calling a method that is using Task.Run. This may causes it to fail!");
            var service = new ResolutionService(_env.ConnectionString, _env.MunityMongoDatabaseSettings.ConnectionString, _env.MunityMongoDatabaseSettings.DatabaseName);
            var userService = new AuthService(_env.ConnectionString);
            userService.Register("test", "test", "test@mail.de");
            var user = userService.GetUserByUsername("test");
            var resolution = service.CreateResolution(false, false, user.Id);
            resolution.Name = "Name";
            resolution.Topic = "Topic";
            service.RequestSave(resolution);
            Assert.NotNull(service.GetResolution(resolution.ID));
            var info = service.GetResolutionInfoForId(resolution.ID);
            Assert.AreEqual("Topic", info.Name);
            Assert.AreEqual(user.Id, info.UserId);
        }

        [Test]
        public void SetResolutionToPublicReadTest()
        {
            Console.WriteLine("This test is calling a method that is using Task.Run. This may causes it to fail!");
            var service = new ResolutionService(_env.ConnectionString, _env.MunityMongoDatabaseSettings.ConnectionString, _env.MunityMongoDatabaseSettings.DatabaseName);
            var userService = new AuthService(_env.ConnectionString);
            userService.Register("test", "test", "test@mail.de");
            var user = userService.GetUserByUsername("test");
            var resolution = service.CreateResolution(false, false, user.Id);
            resolution.Name = "Name";
            resolution.Topic = "Topic";
            service.RequestSave(resolution);
            service.ActivatePublicReadMode(resolution.ID);
            var info = service.GetResolutionInfoForId(resolution.ID);
            Assert.IsTrue(info.PublicRead);
            Assert.IsFalse(string.IsNullOrEmpty(info.OnlineCode));
        }

        [Test]
        public void GiveUserEditRightsTest()
        {
            var service = new ResolutionService(_env.ConnectionString, _env.MunityMongoDatabaseSettings.ConnectionString, _env.MunityMongoDatabaseSettings.DatabaseName);
            var userService = new AuthService(_env.ConnectionString);
            userService.Register("test", "test", "test@mail.de");
            userService.Register("test2", "test", "test2@mail.de");
            var user = userService.GetUserByUsername("test");
            var user2 = userService.GetUserByUsername("test2");
            var resolution = service.CreateResolution(false, false, user.Id);
            resolution.Name = "Name";
            resolution.Topic = "Topic";
            service.RequestSave(resolution);
            service.GrandUserRights(resolution.ID, user2.Id, true, true);
            var userTwoResolutions = service.GetResolutionsUserCanEdit(user2.Id);
            Assert.NotZero(userTwoResolutions.Count);
            Assert.NotNull(userTwoResolutions.Find(n => n.ID == resolution.ID));
        }

        [Test]
        public void LinkResolutionToConferenceTest()
        {
            var conference = new MUNityAngular.Models.Conference.ConferenceModel();
            var conferenceCreated = new ConferenceService(_env.ConnectionString).CreateConference(conference, null);
            Assert.IsTrue(conferenceCreated);
            var resolutionService = new ResolutionService(_env.ConnectionString, 
                _env.MunityMongoDatabaseSettings.ConnectionString, 
                _env.MunityMongoDatabaseSettings.DatabaseName);
            var resolution = resolutionService.CreateResolution();
            resolutionService.LinkResolutionToConference(resolution, conference);
            var linkCount = Tools.Connection(_env.ConnectionString).ResolutionConference.Count();
            Assert.AreEqual(1, linkCount);
            var linkedResolutions = resolutionService.GetResolutionsOfConference(conference.ID);
            Assert.AreEqual(1, linkedResolutions.Count);
            Assert.NotNull(linkedResolutions.Find(n => n.ID == resolution.ID));
        }

        [Test]
        public void LinkResolutionToCommitteeTest()
        {
            var conference = new MUNityAngular.Models.Conference.ConferenceModel();
            var committee = new MUNityAngular.Models.Conference.CommitteeModel();
            conference.AddCommittee(committee);
            var conferenceService = new ConferenceService(_env.ConnectionString);
            var conferenceCreated = conferenceService.CreateConference(conference);
            Assert.IsTrue(conferenceCreated);
            var resolutionService = new ResolutionService(_env.ConnectionString, 
                _env.MunityMongoDatabaseSettings.ConnectionString, 
                _env.MunityMongoDatabaseSettings.DatabaseName);
            var resolution = resolutionService.CreateResolution();
            resolutionService.LinkResolutionToCommittee(resolution, committee);
            var linkCount = Tools.Connection(_env.ConnectionString).ResolutionConference.Count();
            Assert.AreEqual(1, linkCount);
            var linkedResolutions = resolutionService.GetResolutionsOfCommittee(committee.ID);
            Assert.AreEqual(1, linkedResolutions.Count);
            Assert.NotNull(linkedResolutions.Find(n => n.ID == resolution.ID));

        }
    }
}
