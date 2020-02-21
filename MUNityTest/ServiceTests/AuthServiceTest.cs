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
    public class AuthServiceTest
    {
        //Change the password if needed localy, keep in mind that inside the git-Action the password needs to be root!
        //private string _connectionString = @"server=127.0.0.1;userid=root;password='root'";
        //private string test_database_name = "munity-test";

        private ConnectionInfo _sqlConnection;

        [SetUp]
        public void Setup()
        {
            //Testinformationen laden
            var TestSettingsPath = Path.Combine(Environment.CurrentDirectory, "NeededFiles/testsettings.json");

            if (!File.Exists(TestSettingsPath))
                Assert.Fail("The Test configuration is missing!");
            var settingsTest = File.ReadAllText(TestSettingsPath);
            _sqlConnection = Newtonsoft.Json.JsonConvert.DeserializeObject<ConnectionInfo>(settingsTest);

            //Create the empty Database
            using (var connection = new MySqlConnection(_sqlConnection.ConnectionString))
            {

                connection.Open();
                //Vorhandene Datenbank entfernen
                var cmdStr = "DROP DATABASE IF EXISTS `" + _sqlConnection.DatabaseName + "`";
                var cmdDrop = new MySqlCommand(cmdStr, connection);
                cmdDrop.ExecuteNonQuery();

                cmdStr = "CREATE DATABASE `" + _sqlConnection.DatabaseName + "`";
                var cmdCreate = new MySqlCommand(cmdStr, connection);
                cmdCreate.ExecuteNonQuery();

                //Wechseln auf die neue Datenbank
                connection.ChangeDatabase(_sqlConnection.DatabaseName);
                this._sqlConnection.ConnectionString += ";database=" + _sqlConnection.DatabaseName;

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
        public void RegistrationTest()
        {
            var service = new AuthService(_sqlConnection.ConnectionString);
            service.Register("test", "password", "mail@domain.ttl");

            var result = Tools.Connection(_sqlConnection.ConnectionString).Table("user").HasEntry("username", "test");
            Assert.IsTrue(result);
        }

        [Test]
        public void CheckLoginDataTest()
        {
            var service = new AuthService(_sqlConnection.ConnectionString);
            service.Register("test", "password", "mail@domain.ttl");

            var login = service.CheckLoginData("test", "password");
            Assert.IsTrue(login.valid);
            var failTest = service.CheckLoginData("test", "not_password");
            Assert.IsFalse(failTest.valid);
        }

        [Test]
        public void ChangePasswordTest()
        {
            var service = new AuthService(_sqlConnection.ConnectionString);
            service.Register("test", "password", "mail@domain.ttl");

            var login = service.CheckLoginData("test", "password");
            Assert.IsTrue(login.valid);
            service.SetPassword(login.userid, "newpassword");

            var retry = service.CheckLoginData("test", "newpassword");
            Assert.IsTrue(retry.valid);
            var negativeTest = service.CheckLoginData("test", "password");
            Assert.IsFalse(negativeTest.valid);
        }

        [Test]
        public void UsernameAvailableTest()
        {
            var service = new AuthService(_sqlConnection.ConnectionString);
            Assert.IsTrue(service.UsernameAvailable("test"));
            service.Register("test", "password", "mail@domain.ttl");
            Assert.IsFalse(service.UsernameAvailable("test"));
        }

        [Test]
        public void AuthKeyTest()
        {
            //Create a user/Login that user and get the auth key then
            //validate the auth-key
            var service = new AuthService(_sqlConnection.ConnectionString);
            service.Register("test", "test", "test@mail.com");
            var login = service.Login("test", "test");
            Assert.IsTrue(login.status);
            var user = service.GetUserByUsername("test");
            //Validieren des Auth Keys sollte den selben benutzer ausgeben
            var validation = service.ValidateAuthKey(login.key);
            Assert.AreEqual(validation.userid, user.Id);
        }

        [Test]
        public void NegativeHeadAdminTest()
        {
            var service = new AuthService(_sqlConnection.ConnectionString);
            var admin = service.GetHeadAdminId();
            Assert.IsNull(admin);
        }
    }
}
