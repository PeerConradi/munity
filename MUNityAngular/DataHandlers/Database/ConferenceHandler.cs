using MUNityAngular.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.DataHandlers.Database
{
    public class ConferenceHandler : IDatabaseHandler
    {
        private static Models.ConferenceModel _testConference;
        private const string conference_table_name = "conference";

        public static Models.ConferenceModel GetConference(string id)
        {
            if (id == TestConference.ID)
                return TestConference;
            return null;
        }

        public static bool CreateConference(ConferenceModel model)
        {
            model.CreationDate = DateTime.Now;
            Connector.Insert(conference_table_name, model);
            return true;
        }

        public static List<string> GetNameOfAllConferences()
        {
            var list = new List<string>();
            using (var connection = Connector.Connection)
            {
                string cmdStr = "SELECT name FROM conference";
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(reader.GetString(0));
                    }
                }
            }
            return list;
        }

        public static List<ConferenceModel> GetAllConferences()
        {
            var list = new List<ConferenceModel>();
            using (var connection = Connector.Connection)
            {
                string cmdStr = "SELECT * FROM conference";
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var conferece = DataReaderConverter.ObjectFromReader<ConferenceModel>(reader);
                        conferece.Committees = CommitteeHandler.GetCommitteesOfConference(conferece);
                        list.Add(conferece);
                    }
                }
            }
            return list;
        }


        public static bool UpdateConference(Models.ConferenceModel model)
        {
            throw new NotImplementedException();
        }

        public static Models.ConferenceModel TestConference
        {
            get
            {
                if (_testConference == null)
                {
                    _testConference = new Models.ConferenceModel();
                    _testConference.ID = "TESTCONFERENCE";
                    _testConference.Name = "Test";
                    _testConference.FullName = "Test Conference";
                    _testConference.AddCommittee(CommitteeHandler.TestCommittee);
                }

                return _testConference;
            }
        }

        public DatabaseInformation.ETableStatus TableStatus
        {
            get
            {
                var v = Connector.DoesTableExists("conference");
                if (v)
                {
                    return DatabaseInformation.ETableStatus.Ready;
                }
                else
                {
                    return DatabaseInformation.ETableStatus.NotExisting;
                }
            }
        }

        public bool CreateTables()
        {
            Connector.CreateTable(conference_table_name, typeof(ConferenceModel));
            return true;
        }
    }
}
