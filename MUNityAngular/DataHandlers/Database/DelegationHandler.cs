using MUNityAngular.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.DataHandlers.Database
{
    public class DelegationHandler : IDatabaseHandler
    {
        private const string delegation_table_name = "delegation";

        private static DelegationModel _delegationAF = new Models.DelegationModel() { ID = "default_af", Name = "Afghanistan", ISO = "AF" };
        private static DelegationModel _delegationEG = new Models.DelegationModel() { ID = "default_eg", Name = "Ägypten", ISO = "EQ" };
        private static DelegationModel _delegationAL = new Models.DelegationModel() { ID = "default_al", Name = "Albanien", ISO = "AL" };
        private static List<DelegationModel> _defaultDelegations;

        public static DelegationModel GetDelegation(string id)
        {
            var inDefault = AllDefaultDelegations().FirstOrDefault(n => n.ID == id);
            if (inDefault != null) return inDefault;

            return null;
        }

        public static List<DelegationModel> GetDelegationsOfConference(ConferenceModel conference)
        {
            var list = new List<DelegationModel>();
            using (var connection = Connector.Connection)
            {
                var cmdStr = "SELECT delegation.id, delegation.`name`, delegation.abbreviation, ";
                cmdStr += "delegation.type, delegation.countryid, ";
                cmdStr += "conference.`name` as 'conferencename', conference.fullname, conference.id as 'conferenceid', ";
                cmdStr += "delegation_in_committee.committeeid FROM conference ";
                cmdStr += "INNER JOIN conference_delegation ON conference_delegation.conference_id = conference.id ";
                cmdStr += "INNER JOIN delegation ON conference_delegation.delegation_id = delegation.id ";
                cmdStr += "INNER JOIN delegation_in_committee ON delegation_in_committee.linkid = conference_delegation.linkid ";
                cmdStr += "WHERE conference.id = @conferenceid GROUP BY delegation.id";
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@conferenceid", conference.ID);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(DataReaderConverter.ObjectFromReader<DelegationModel>(reader));
                    }
                }

            }
            return list;
        }

        public static List<Models.DelegationModel> AllDefaultDelegations()
        {
            if (_defaultDelegations == null)
            {
                _defaultDelegations = new List<DelegationModel>();
                _defaultDelegations.Add(_delegationAF);
                _defaultDelegations.Add(_delegationEG);
                _defaultDelegations.Add(_delegationAL);
            }
            return _defaultDelegations;
        }

        public DatabaseInformation.ETableStatus TableStatus
        {
            get
            {
                var v = Connector.DoesTableExists(delegation_table_name);
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
            Connector.CreateTable(delegation_table_name, typeof(DelegationModel));
            return true;
        }
    }
}
