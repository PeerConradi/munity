using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.DataHandlers.Database;
using MUNityAngular.Models;
using MySql.Data.MySqlClient;

namespace MUNityAngular.Services
{
    public class ConferenceService
    {
        private const string conference_table_name = "conference";
        private const string delegation_table_name = "delegation";
        private const string conference_user_auth_table_name = "conference_user_auth";
        private const string committee_table_name = "committee";

        private List<Models.ConferenceModel> conferences = new List<Models.ConferenceModel>();
        private void LoadConferencesFromDatabase()
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
                        var conference = DataReaderConverter.ObjectFromReader<ConferenceModel>(reader);
                        conference.Committees = GetCommitteesOfConference(conference);
                        conference.Delegations = GetDelegationsOfConference(conference);
                        conference.Committees.ForEach(n => n.DelegationList = GetDelegationIdsOfCommittee(n));
                        list.Add(conference);
                    }
                }
            }
            conferences = list;
        }

        private ConferenceModel LoadConferenceFromDatabase(string id)
        {
            var conference = new ConferenceModel();
            using (var connection = Connector.Connection)
            {
                string cmdStr = "SELECT * FROM conference WHERE id=@id;";
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@id", id);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                    {
                        conference = null;
                    }

                    while (reader.Read())
                    {
                        conference = DataReaderConverter.ObjectFromReader<ConferenceModel>(reader);
                        conference.Committees = GetCommitteesOfConference(conference);
                        conference.Delegations = GetDelegationsOfConference(conference);
                        conference.Committees.ForEach(n => n.DelegationList = GetDelegationIdsOfCommittee(n));
                        conferences.Add(conference);
                    }
                }
            }
            return conference;
        }

        public ConferenceModel GetConference(string id)
        {
            var conference = this.conferences.FirstOrDefault(n => n.ID == id);
            if (conference == null)
            {
                conference = LoadConferenceFromDatabase(id);
            }
            return conference;
        }

        
        public List<string> GetDelegationIdsOfCommittee(CommitteeModel committee)
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

        #region General Conference Settings
        public bool CreateConference(ConferenceModel model, string userid)
        {
            model.CreationDate = DateTime.Now;
            Connector.Insert(conference_table_name, model);
            conferences.Add(model);
            using (var connection = Connector.Connection)
            {
                var cmdStr = "INSERT INTO " + conference_user_auth_table_name + "(conferenceid, userid, CanOpen, CanEdit, CanRemove)" +
                        " VALUES (@conferenceid, @userid, 1, 1, 1)";
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@conferenceid", model.ID);
                cmd.Parameters.AddWithValue("@userid", userid);
                cmd.ExecuteNonQuery();
            }

            return true;
        }

        public bool ChangeConferenceName(ConferenceModel conference, string newName)
        {
            conference.Name = newName;
            using (var connection = Connector.Connection)
            {
                connection.Open();
                var cmdStr = "UPDATE " + conference_table_name + " SET name=@name WHERE id=@id";
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@name", conference.Name);
                cmd.Parameters.AddWithValue("@id", conference.ID);
                cmd.ExecuteNonQuery();
            }
            return true;
        }
        #endregion


        #region Filter Conferences
        public List<string> GetNameOfAllConferences()
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

        public List<ConferenceModel> GetAllConferencesOfAuth(string auth)
        {
            var list = new List<ConferenceModel>();
            using (var connection = Connector.Connection)
            {
                string cmdStr = "SELECT conference.* FROM auth" +
                    " INNER JOIN conference_user_auth ON conference_user_auth.userid = auth.userid" +
                    " INNER JOIN conference ON conference_user_auth.conferenceid = conference.id" +
                    " WHERE auth.authkey=@authkey";
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@authkey", auth);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var conference = DataReaderConverter.ObjectFromReader<ConferenceModel>(reader);
                        conference.Committees = GetCommitteesOfConference(conference);
                        conference.Delegations = GetDelegationsOfConference(conference);
                        conference.Committees.ForEach(n => n.DelegationList = GetDelegationIdsOfCommittee(n));
                        list.Add(conference);
                    }
                }
            }
            return list;
        }
        #endregion



        public bool CanAuthEditConference(string auth, string conferenceid)
        {
            var canEdit = false;
            using (var connection = Connector.Connection)
            {
                string cmdStr = "SELECT conference_user_auth.CanEdit FROM auth" +
                    " INNER JOIN conference_user_auth ON conference_user_auth.userid = auth.userid" +
                    " INNER JOIN conference ON conference_user_auth.conferenceid = conference.id" +
                    " WHERE auth.authkey=@authkey And conference_user_auth.conferenceid=@confid";
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@authkey", auth);
                cmd.Parameters.AddWithValue("@confid", conferenceid);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        canEdit = reader.GetBoolean(0);
                    }
                }
            }
            return canEdit;
        }

        

        public List<UserModel> UsersWithAccessToConference(string conferenceid)
        {
            var list = new List<UserModel>();
            using (var connection = Connector.Connection)
            {
                string cmdStr = "SELECT `user`.* FROM `user`" +
                    " INNER JOIN conference_user_auth ON conference_user_auth.userid = `user`.id" +
                    " WHERE conference_user_auth.conferenceid=@confid";
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@confid", conferenceid);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var user = DataReaderConverter.ObjectFromReader<UserModel>(reader);
                        if (user != null)
                            list.Add(user);
                    }
                }
            }
            return list;
        }

        #region Committee
        public bool AddCommittee(ConferenceModel conference, CommitteeModel committee)
        {
            if (conference == null)
                throw new ArgumentNullException("The conference cannot be null!");

            committee.ConferenceID = conference.ID;
            Connector.Insert(committee_table_name, committee);
            conference.AddCommittee(committee);
            return true;
        }

        public static BaseCommitteeModel GetCommittee(string id)
        {
            var cmdStr = "SELECT * FROM " + committee_table_name + " WHERE id=@id";
            using (var connection = Connector.Connection)
            {
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@id", id);
                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return null;

                    return DataReaderConverter.ObjectFromReader<BaseCommitteeModel>(reader);
                }
            }
        }

        public List<CommitteeModel> GetCommitteesOfConference(ConferenceModel conference)
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

        #endregion

        #region Delegation

        public List<DelegationModel> GetAllDelegations()
        {
            var list = new List<DelegationModel>();
            using (var connection = Connector.Connection)
            {
                connection.Open();
                var cmdStr = "SELECT * FROM delegation";
                var cmd = new MySqlCommand(cmdStr, connection);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(DataHandlers.Database.DataReaderConverter.ObjectFromReader<DelegationModel>(reader));
                    }
                }
            }
            return list;
        }

        public DelegationModel GetDelegation(string id)
        {
            DelegationModel model = null;
            using (var connection = Connector.Connection)
            {
                string cmdStr = "SELECT * FROM " + delegation_table_name + " WHERE id = @id";
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    model = DataReaderConverter.ObjectFromReader<DelegationModel>(reader);

                }
            }
            return model;
        }

        public DelegationModel CreateDelegation(string name,string fullname, string abbreviation, string type, string countryid = null)
        {
            var model = new DelegationModel();
            model.Abbreviation = abbreviation;
            model.CountryId = countryid;
            model.Name = name;
            model.TypeName = type;
            model.FullName = fullname;

            Connector.Insert(delegation_table_name, model);
            return model;
        }

        public void AddDelegationToConference(string conferenceid, string delegationid, int minCount, int maxCount)
        {
            using (var connection = Connector.Connection)
            {
                connection.Open();
                int? linkId = null;
                //Stelle fest ob es bereits eine Verbindung gibt, wenn diese exisitert passe lediglich die Anzahlen an
                var cmdStr = "SELECT linkid FROM conference_delegation WHERE conference_id=@conferenceid AND delegation_id=@delegationid;";
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@mincount", minCount);
                cmd.Parameters.AddWithValue("@maxcount", maxCount);
                cmd.Parameters.AddWithValue("@conferenceid", conferenceid);
                cmd.Parameters.AddWithValue("@delegationid", delegationid);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        linkId = reader.GetInt16(0);
                    }
                }

                //Update
                if (linkId.HasValue)
                {
                    cmd.CommandText = "UPDATE conference_delegation SET mincount=@mincount, maxcount=@maxcount WHERE linkid=@linkid;";
                    
                    cmd.Parameters.AddWithValue("@linkid", linkId.Value);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    //Insert as new
                    cmd.CommandText = "INERT INTO conference_delegation (conference_id, delegation_id, mincount, maxcount) VALUES " +
                        "(@conferenceid, @delegationid, @mincount, @maxcount);";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public bool AddDelegationToCommittee(DelegationModel delegation)
        {
            throw new NotImplementedException("To be done!");
        }

        public List<DelegationModel> GetDelegationsOfConference(ConferenceModel conference)
        {
            return GetDelegationsOfConference(conference.ID);
        }

        public List<DelegationModel> GetDelegationsOfConference(string conferenceid)
        {
            var list = new List<DelegationModel>();
            using (var connection = Connector.Connection)
            {
                var cmdStr = "SELECT delegation.* FROM delegation INNER JOIN conference_delegation ON conference_delegation.delegation_id = delegation.id WHERE conference_id=@conferenceid";
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@conferenceid", conferenceid);
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
        #endregion

        public ConferenceService()
        {
            Console.WriteLine("Conference-Service Started!");
        }
    }
}
