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

        public static Models.ConferenceModel GetConference(string id)
        {
            if (id == TestConference.ID)
                return TestConference;
            return null;

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
            Connector.CreateTable("conference", typeof(ConferenceModel));
            return true;
        }
    }
}
