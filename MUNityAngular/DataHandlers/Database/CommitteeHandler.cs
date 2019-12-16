using MUNityAngular.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.DataHandlers.Database
{
    public class CommitteeHandler
    {
        public static CommitteeModel GetCommittee(string id)
        {
            if (id == TestCommittee.ID)
                return TestCommittee;

            return null;
        }

        public static List<string> GetDelegationIdsOfCommittee(CommitteeModel committee)
        {
            var list = new List<string>();
            using (var connection = Connector.Connection)
            {
                var cmdStr = "SELECT conference_delegation.delegation_id FROM delegation_in_committee ";
                cmdStr += "INNER JOIN committee ON delegation_in_committee.committeeid = committee.id ";
                cmdStr += "INNER JOIN conference_delegation ON delegation_in_committee.linkid = conference_delegation.linkid ";
                cmdStr += "WHERE delegation_in_committee.committeeid = @committeeid";
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@committeeid", committee.ID);
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

        public static List<CommitteeModel> GetCommitteesOfConference(ConferenceModel conference)
        {
            var list = new List<CommitteeModel>();
            using (var connection = Connector.Connection)
            {
                var cmdStr = "SELECT * FROM committee WHERE conferenceid = @conferenceid";
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@conferenceid", conference.ID);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(DataReaderConverter.ObjectFromReader<CommitteeModel>(reader));
                    }
                }
            }
            return list;
        }

        public static CommitteeModel TestCommittee
        {
            get
            {
                var committee = new CommitteeModel();
                committee.ID = "test_committee";
                committee.Name = "Test Committee";
                committee.FullName = "A Committee to Test";
                committee.Abbreviation = "Ctt";
                committee.Article = "the";
                DelegationHandler.AllDefaultDelegations().ToList().ForEach(n => committee.AddDelegation(n));
                return committee;
            }
        }
    }
}
