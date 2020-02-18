using MySql.Data.MySqlClient;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using MUNityAngular.Services;
using MUNityAngular.Models.Conference;

namespace MUNityTest.ServiceTests
{
    class ConferenceServiceTest
    {
        private string _connectionString = @"server=127.0.0.1;userid=root;password=''";
        private string test_database_name = "munity-test";

        [SetUp]
        public void Setup()
        {
            //Create the empty Database
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                //Vorhandene Datenbank entfernen
                var cmdStr = "DROP DATABASE IF EXISTS `" + test_database_name + "`";
                var cmdDrop = new MySqlCommand(cmdStr, connection);
                cmdDrop.ExecuteNonQuery();

                cmdStr = "CREATE DATABASE `" + test_database_name + "`";
                var cmdCreate = new MySqlCommand(cmdStr, connection);
                cmdCreate.ExecuteNonQuery();

                //Wechseln auf die neue Datenbank
                connection.ChangeDatabase(test_database_name);
                this._connectionString += ";database=" + test_database_name;

                //Schaue wo die munitysql hin kopiert wird...
                var path = Path.Combine(Environment.CurrentDirectory, "NeededFiles/munity.sql");
                var sql = File.ReadAllText(path);
                cmdStr = sql;
                var cmdFill = new MySqlCommand(sql, connection);
                var filled = cmdFill.ExecuteNonQuery();
                Console.WriteLine(filled + " Test Data added");
            }
        }

        [Test]
        public void CreateConferenceTest()
        {
            var service = new ConferenceService(_connectionString);
            var conference = new ConferenceModel();
            conference.Name = "Test";
            conference.FullName = "Test";
            conference.Abbreviation = "TT";
            conference.StartDate = new DateTime(2020, 1, 1, 12, 0, 0);
            conference.EndDate = new DateTime(2020, 1, 2, 12, 0, 0);
            conference.SecretaryGeneralTitle = "SG";
            conference.SecretaryGerneralName = "SGName";
            var result = service.CreateConference(conference, null);
            Assert.IsTrue(result);
            Assert.AreEqual(conference, service.GetConference(conference.ID));

            //Database Checks
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var cmdStr = "SELECT * FROM conference WHERE id=@id";
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@id", conference.ID);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Assert.AreEqual(conference.Name, reader.GetString("name"));
                            Assert.AreEqual(conference.FullName, reader.GetString("fullname"));
                            Assert.AreEqual(conference.StartDate, reader.GetDateTime("startdate"));
                        }
                    }
                    else
                    {
                        Assert.Fail("The command should return one line with the new created conference");
                    }
                }
            }
        }
    }
}
