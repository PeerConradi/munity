using MySql.Data.MySqlClient;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using MUNityAngular.Services;
using MUNityAngular.Models.Conference;
using MUNityAngular.DataHandlers.Database;

namespace MUNityTest.ServiceTests
{

    public class ConferenceServiceTest
    {

        //Change the password if needed localy, keep in mind that inside the git-Action the password needs to be root!
        //private string _connectionString = @"server=127.0.0.1;userid=root;password=''";
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
        public void CreateConferenceTest()
        {
            var service = new ConferenceService(_sqlConnection.ConnectionString);
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
            using (var connection = new MySqlConnection(_sqlConnection.ConnectionString))
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
                            Assert.AreEqual(conference.EndDate, reader.GetDateTime("enddate"));
                            Assert.AreEqual(conference.SecretaryGeneralTitle, reader.GetString("secretarygeneraltitle"));
                            Assert.AreEqual(conference.SecretaryGerneralName, reader.GetString("secretarygeneralname"));
                        }
                    }
                    else
                    {
                        Assert.Fail("The command should return one line with the new created conference");
                    }
                }
            }
        }

        [Test]
        public void RemoveConferenceTest()
        {
            var service = new ConferenceService(_sqlConnection.ConnectionString);
            var conference = new ConferenceModel();
            var result = service.CreateConference(conference, null);
            Assert.IsTrue(result);
            Assert.AreEqual(conference, service.GetConference(conference.ID));

            //Database Checks
            var dbResult = Tools.Connection(_sqlConnection.ConnectionString).Table("conference").HasEntry("id", conference.ID);
            Assert.IsTrue(dbResult);

            service.RemoveConference(conference);

            //Database Checks
            dbResult = Tools.Connection(_sqlConnection.ConnectionString).Table("conference").HasEntry("id", conference.ID);
            Assert.IsFalse(dbResult);
            Assert.IsNull(service.GetConference(conference.ID));
        }
    
        [Test]
        public void ChangeConferenceNameTest()
        {
            var service = new ConferenceService(_sqlConnection.ConnectionString);
            var conference = new ConferenceModel();
            conference.Name = "oldName";
            var result = service.CreateConference(conference, null);
            Assert.IsTrue(result);
            Assert.AreEqual(conference, service.GetConference(conference.ID));

            //Database Checks
            var name = Tools.Connection(_sqlConnection.ConnectionString).Table("conference").GetEntries("id", conference.ID)["name"];
            Assert.AreEqual("oldName", name);
            service.ChangeConferenceName(conference, "newName");

            //Database Check
            name = Tools.Connection(_sqlConnection.ConnectionString).Table("conference").GetEntries("id", conference.ID)["name"];
            Assert.AreEqual("newName", name);

            var cFromService = service.GetConference(conference.ID);
            Assert.IsNotNull(cFromService);
            Assert.AreEqual("newName", cFromService.Name);
        }
    
        [Test]
        public void AddCommitteeTest()
        {
            var service = new ConferenceService(_sqlConnection.ConnectionString);
            var conference = new ConferenceModel();
            var conferenceResult = service.CreateConference(conference, null);
            Assert.IsTrue(conferenceResult);
            Assert.AreEqual(conference, service.GetConference(conference.ID));

            var committee = new CommitteeModel();
            var committeeResult = service.AddCommittee(conference, committee);
            Assert.IsTrue(committeeResult);

            var cf = service.GetConference(conference.ID);
            Assert.AreEqual(1, cf.Committees.Count);

            //Database Checks
            var cValue = Tools.Connection(_sqlConnection.ConnectionString).Table("committee").GetEntries("id", committee.ID)["conferenceid"];
            Assert.AreEqual(conference.ID, cValue);
        }

        [Test]
        public void AddDelegationToConferenceTest()
        {
            var service = new ConferenceService(_sqlConnection.ConnectionString);
            var conference = new ConferenceModel();
            service.CreateConference(conference, null);
            var delegations = service.GetAllDelegations();
            Assert.NotZero(delegations.Count);
            service.AddDelegationToConference(conference.ID, delegations[0].ID, 1, 1);
            var testResult = Tools.Connection(_sqlConnection.ConnectionString).Table("conference_delegation").GetElements<ConferenceDelegationModel>();
            Assert.NotZero(testResult.Count);
            var result = testResult.Find(n => n.ConferenceId == conference.ID && n.DelegationId == delegations[0].ID);
            Assert.NotNull(result);

            var conferenceDelegations = service.GetDelegationsOfConference(conference);
            Assert.NotNull(conferenceDelegations.Find(n => n.ID == delegations[0].ID));
        }

        [Test]
        public void AddDelegationToCommitteeTest()
        {
            var service = new ConferenceService(_sqlConnection.ConnectionString);
            var conference = new ConferenceModel();
            service.CreateConference(conference, null);
            var delegations = service.GetAllDelegations();
            var committee = new CommitteeModel();
            service.AddCommittee(conference, committee);
            service.AddDelegationToCommittee(committee, delegations[0], 1, 1);
            //An dieser Stelle sollte bereits eine Verbindung zwischen der Konferenz und den Delegationen bestehen
            var conferenceDelegations = Tools.Connection(_sqlConnection.ConnectionString).Table("conference_delegation").GetElements<ConferenceDelegationModel>();
            Assert.NotZero(conferenceDelegations.Count);

            //Die Verbindung zwischen Delegation und Gremium sollte natürlich auch existieren
            var committeeDelegations = Tools.Connection(_sqlConnection.ConnectionString).Table("delegation_in_committee").GetElements<DelegationInCommitteeModel>();
            Assert.NotZero(committeeDelegations.Count);

            //Testen der Internen Sammelmethode
            var dels = service.GetDelegationsOfCommittee(committee);
            Assert.NotNull(dels.Find(n => n.ID == delegations[0].ID));
        }

        [Test]
        public void RemoveDelegationFromCommitteeTest()
        {
            var service = new ConferenceService(_sqlConnection.ConnectionString);
            var conference = new ConferenceModel();
            service.CreateConference(conference, null);
            var delegations = service.GetAllDelegations();
            var committee = new CommitteeModel();
            service.AddCommittee(conference, committee);
            service.AddDelegationToCommittee(committee, delegations[0], 1, 1);
            //An dieser Stelle sollte bereits eine Verbindung zwischen der Konferenz und den Delegationen bestehen
            var conferenceDelegations = Tools.Connection(_sqlConnection.ConnectionString).Table("conference_delegation").GetElements<ConferenceDelegationModel>();
            Assert.NotZero(conferenceDelegations.Count);

            //Die Verbindung zwischen Delegation und Gremium sollte natürlich auch existieren
            var committeeDelegations = Tools.Connection(_sqlConnection.ConnectionString).Table("delegation_in_committee").GetElements<DelegationInCommitteeModel>();
            Assert.NotZero(committeeDelegations.Count);

            service.RemoveDelegationFromCommittee(committee, delegations[0]);
            committeeDelegations = Tools.Connection(_sqlConnection.ConnectionString).Table("delegation_in_committee").GetElements<DelegationInCommitteeModel>();
            Assert.Zero(committeeDelegations.Count);
        }

        [Test]
        public void CreateTeamRoleTest()
        {
            var service = new ConferenceService(_sqlConnection.ConnectionString);
            var conference = new ConferenceModel();
            service.CreateConference(conference, null);
            var role = new TeamRoleModel();
            role.Name = "Testrolle";
            role.MinCount = 1;
            role.MaxCount = 1;
            role.Description = "Rolle zum Testen";
            role.ParentRoleId = null;
            role.ConferenceId = conference.ID;
            service.AddTeamRole(role);
            var result = Tools.Connection(_sqlConnection.ConnectionString).ConferenceTeamRoles.HasEntry("name", "Testrolle");
            var copy = Tools.Connection(_sqlConnection.ConnectionString).ConferenceTeamRoles.First<TeamRoleModel>();
            Assert.AreEqual(role.Name, copy.Name);
            Assert.AreEqual(role.MinCount, copy.MinCount);
            Assert.AreEqual(role.MaxCount, copy.MaxCount);
            Assert.AreEqual(role.ParentRoleId, copy.ParentRoleId);
            Assert.AreEqual(role.ConferenceId, copy.ConferenceId);
            Assert.IsTrue(result);
        }

        [Test]
        public void ConferenceTeamTest()
        {
            var service = new ConferenceService(_sqlConnection.ConnectionString);
            var authService = new AuthService(_sqlConnection.ConnectionString);
            var conference = new ConferenceModel();
            service.CreateConference(conference, null);
            var plRole = new TeamRoleModel() { Name = "Projektleitung", MinCount = 2, MaxCount = 3, Description = "Leitung des Projekts" };
            plRole.Id = (int)service.AddTeamRole(plRole);
            authService.Register("test", "test", "test@test.test");
            var user = authService.GetUserByUsername("test");

            //Add user to team
            service.AddUserToConferenceTeam(user, conference, plRole);
            var team = Tools.Connection(_sqlConnection.ConnectionString).ConferenceTeam.GetElements<ConferenceTeamUserModel>();
            Assert.NotZero(team.Count);
            var serviceTeam = service.GetTeamUsers(conference);
            Assert.NotZero(serviceTeam.Count);
            Assert.NotNull(serviceTeam.Find(n => n.Id == user.Id));

            var teamWithRoles = service.GetConferenceTeam(conference);
            Assert.NotZero(teamWithRoles.Count);
            Assert.NotNull(teamWithRoles.Find(n => n.User.Id == user.Id));

            //Test all Team Roles
            var rolesOfUser = service.GetUserTeamRolesAtConference(user, conference);
            Assert.NotZero(rolesOfUser.Count);
        }
    }
}
