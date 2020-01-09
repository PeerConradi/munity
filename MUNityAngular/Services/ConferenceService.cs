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
        private const string conferece_user_auth_table_name = "conference_user_auth";

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
                        throw new Exception("Conference not found!");
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

        public bool AddDelegationToCommittee(DelegationModel delegation)
        {
            throw new NotImplementedException("To be done!");
        }

        public static CommitteeModel GetCommittee(string id)
        {
            return null;
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

        public bool CreateConference(ConferenceModel model, string password, string userid = null)
        {
            model.CreationDate = DateTime.Now;
            Connector.Insert(conference_table_name, model);
            conferences.Add(model);

            var hashedPassword = Util.Hashing.PasswordHashing.InitHashing(password);
            using (var connection = Connector.Connection)
            {
                var cmdStr = "INSERT INTO conference_password (conferenceid, password, salt, rank) VALUES " +
                    "(@conferenceid, @password, @salt, @rank);";
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@conferenceid", model.ID);
                cmd.Parameters.AddWithValue("@password", hashedPassword.Key);
                cmd.Parameters.AddWithValue("@salt", hashedPassword.Salt);
                cmd.Parameters.AddWithValue("@rank", "ADMIN");
                cmd.ExecuteNonQuery();

                if (userid != null)
                {
                    cmdStr = "INSERT INTO " + conferece_user_auth_table_name + "(conferenceid, userid, CanOpen, CanEdit, CanRemove)"+ 
                        " VALUES (@conferenceid, @userid, 1, 1, 1)";
                    cmd = new MySqlCommand(cmdStr, connection);
                    cmd.Parameters.AddWithValue("@conferenceid", model.ID);
                    cmd.Parameters.AddWithValue("@userid", userid);
                    cmd.ExecuteNonQuery();
                }
                
            }

            return true;
        }

        public bool CheckConferencePassword(string conferenceid, string password)
        {
            bool valid = false;
            using (var connection = Connector.Connection)
            {
                connection.Open();
                var cmdStr = "SELECT * FROM conference_password WHERE conferenceid=@id;";
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@id", conferenceid);
                using (var reader = cmd.ExecuteReader())
                {
                    bool rows = reader.HasRows;

                    while (reader.Read())
                    {
                        var hashedPass = reader.GetString("password");
                        var salt = reader.GetString("salt");
                        valid = Util.Hashing.PasswordHashing.CheckPassword(password, salt, hashedPass);
                    }
                }
            }
            return valid;
        }

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

        public List<ConferenceModel> GetAllConferences(string auth)
        {
            var list = new List<ConferenceModel>();
            using (var connection = Connector.Connection)
            {
                string cmdStr = "SELECT conference.* FROM auth"+
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

        public static bool UpdateConference(Models.ConferenceModel model)
        {
            throw new NotImplementedException();
        }

        #region Delegation
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

        public DelegationModel CreateDelegation(string name, string abbreviation, string type, string countryid = null)
        {
            var model = new DelegationModel();
            model.Abbreviation = abbreviation;
            model.CountryId = countryid;
            model.Name = name;
            model.TypeName = type;

            Connector.Insert(delegation_table_name, model);
            return model;
        }



        public List<DelegationModel> GetDelegationsOfConference(ConferenceModel conference)
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
        #endregion


    }
}
