using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.DataHandlers.Database;
using MySql.Data.MySqlClient;
using MUNityAngular.Models.Conference;
using MUNityAngular.Models.User;

namespace MUNityAngular.Services
{
    public class ConferenceService
    {
        private string _connectionString;
        
        private const string conference_table_name = "conference";
        private const string delegation_table_name = "delegation";
        private const string conference_user_auth_table_name = "conference_user_auth";
        private const string committee_table_name = "committee";




        private List<ConferenceModel> conferences = new List<ConferenceModel>();

        private ConferenceModel LoadConferenceFromDatabase(string id)
        {
            var conference = new ConferenceModel();
            using (var connection = new MySqlConnection(_connectionString))
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

                        conference.Committees.ForEach(n => n.DelegationList = GetDelegationsOfCommittee(n).Select(a => a.ID).ToList());
                        conferences.Add(conference);
                    }
                }
            }
            return conference;
        }

        public ConferenceModel GetConference(string id)
        {
            //Look if the conference may is already inside the cache, if its not then load it from the
            //Database. Because this may take longer we save it in cache.
            //To not waste memory the cache should be cleaned every couple of hours.
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
            using (var connection = new MySqlConnection(_connectionString))
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
            conferences.Add(model);
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var cmdInsert = Connector.GetInsertionCommand(conference_table_name, model);
                cmdInsert.Connection = connection;
                cmdInsert.ExecuteNonQuery();


                if (userid != null)
                {
                    var cmdStr = "INSERT INTO " + conference_user_auth_table_name + "(conferenceid, userid, CanOpen, CanEdit, CanRemove)" +
                        " VALUES (@conferenceid, @userid, 1, 1, 1)";
                    var cmd = new MySqlCommand(cmdStr, connection);
                    cmd.Parameters.AddWithValue("@conferenceid", model.ID);
                    cmd.Parameters.AddWithValue("@userid", userid);
                    cmd.ExecuteNonQuery();
                }
                
            }
            
            return true;
        }

        public void SetUserConferenceAuths(UserModel user, ConferenceModel conference, ConferenceUserAuthModel auths)
        {
            auths.ConferenceId = conference.ID;
            auths.UserId = user.Id;
            Tools.Connection(_connectionString).Table(conference_user_auth_table_name).ReplaceInto(auths);
        }

        public void SetUserConferenceAuths(ConferenceUserAuthModel auths)
        {
            Tools.Connection(_connectionString).Table(conference_user_auth_table_name).ReplaceInto(auths);
        }


        public void RemoveConference(ConferenceModel conference)
        {
            if (conference == null)
                throw new ArgumentNullException("The given conference is null");

            conferences.Remove(conference);
            var f = conferences.Where(n => n.ID == conference.ID).ToList();
            f.ForEach(n => conferences.Remove(n));

            Tools.Connection(_connectionString).Table(conference_table_name).Remove(conference);
        }

        public bool ChangeConferenceName(ConferenceModel conference, string newName)
        {
            conference.Name = newName;
            //Tools.Connection(_connectionString).Table(conference_table_name).SetEntry("id", conference.ID, "name", conference.Name);
            Tools.Connection(_connectionString).Table(conference_table_name).Update(conference);
            return true;
        }

        internal int GetConferenceCacheCount()
        {
            return conferences.Count;
        }
        #endregion


        #region Filter Conferences
        public List<string> GetNameOfAllConferences()
        {
            var list = new List<string>();
            using (var connection = new MySqlConnection(_connectionString))
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
            using (var connection = new MySqlConnection(_connectionString))
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

        internal List<ConferenceModel> GetAll()
        {
            return Tools.Connection(_connectionString).Table(conference_table_name).GetElements<ConferenceModel>();
        }
        #endregion

        public bool CanAuthEditConference(string auth, string conferenceid)
        {
            var canEdit = false;
            using (var connection = new MySqlConnection(_connectionString))
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
            using (var connection = new MySqlConnection(_connectionString))
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
            var searchResolutly = GetCommittee(committee.ResolutlyCommitteeID);
            if (searchResolutly == null)
            {
                committee.ResolutlyCommitteeID = null;
            }

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                
                var cmd = Connector.GetInsertionCommand(committee_table_name, committee);
                cmd.Connection = connection;
                cmd.ExecuteNonQuery();
            }

            conference.AddCommittee(committee);
            return true;
        }

        public CommitteeModel GetCommittee(string id)
        {
            if (id == null) return null;
            return Tools.Connection(_connectionString).Table(committee_table_name).First<CommitteeModel>("id", id);
        }

        public List<CommitteeModel> GetCommitteesOfConference(ConferenceModel conference)
        {
            return Tools.Connection(_connectionString).Table(committee_table_name).GetElements<CommitteeModel>("conferenceid", conference.ID);
        }

        #endregion

        #region Delegation

        public List<DelegationModel> GetAllDelegations()
        {
            var list = new List<DelegationModel>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var cmdStr = "SELECT * FROM delegation";
                var cmd = new MySqlCommand(cmdStr, connection);
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

        public DelegationModel GetDelegation(string id)
        {
            DelegationModel model = null;
            using (var connection = new MySqlConnection(_connectionString))
            {
                string cmdStr = "SELECT * FROM " + delegation_table_name + " WHERE id = @id";
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@id", id);
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

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = Connector.GetInsertionCommand(delegation_table_name, model);
                cmd.ExecuteNonQuery();
            }
            return model;
        }

        public void AddDelegationToConference(string conferenceid, string delegationid, int minCount, int maxCount)
        {
            using (var connection = new MySqlConnection(_connectionString))
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
                    cmd.CommandText = "INSERT INTO conference_delegation (conference_id, delegation_id, mincount, maxcount) VALUES " +
                        "(@conferenceid, @delegationid, @mincount, @maxcount);";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        

        public List<DelegationModel> GetDelegationsOfCommittee(CommitteeModel committee)
        {
            return GetDelegationsOfCommittee(committee.ID);
        }

        public List<DelegationModel> GetDelegationsOfCommittee(string committeeid)
        {
            var list = new List<DelegationModel>();

            var cmdStr = "SELECT delegation.* FROM delegation, delegation_in_committee, conference_delegation " +
                "WHERE conference_delegation.linkid = delegation_in_committee.linkid " +
                "AND conference_delegation.delegation_id = delegation.id " +
                "AND delegation_in_committee.committeeid = @committeeid;";
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@committeeid", committeeid);
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

        public bool AddDelegationToCommittee(CommitteeModel committee, DelegationModel delegation, int mincount, int maxcount)
        {
            var linkid = GetDelegationConferenceLinkId(committee.ConferenceID, delegation.ID);

            if (!committee.DelegationList.Contains(delegation.ID))
                committee.DelegationList.Add(delegation.ID);

            var conference = conferences.FirstOrDefault(n => n.ID == committee.ConferenceID);
            if (conference != null)
            {
                if (!conference.Delegations.Any(n => n.ID == delegation.ID))
                {
                    conference.Delegations.Add(delegation);
                }
            }
            
            if (linkid == null)
            {
                AddDelegationToConference(committee.ConferenceID, delegation.ID, mincount, maxcount);
                linkid = GetDelegationConferenceLinkId(committee.ConferenceID, delegation.ID);
            }
            else
            {
                // TODO: Update mincount and maxcount
            }

            if (linkid.HasValue)
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    //Prüfe ob die Verbindung nicht bereits exisitert!
                    int? isIn = null;
                    var cStr = "SELECT delincommitteeid FROM delegation_in_committee WHERE linkid=@linkid AND committeeid=@committeeid;";
                    var c = new MySqlCommand(cStr, connection);
                    c.Parameters.AddWithValue("@linkid", linkid.Value);
                    c.Parameters.AddWithValue("@committeeid", committee.ID);
                    using (var reader = c.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            isIn = reader.GetInt16(0);
                        }
                    }

                    if (isIn.HasValue)
                    {
                        //Update min and max count if already in
                        var cmdStr = "UPDATE delegation_in_committee SET mincount=@mincount, maxcount=@maxcount WHERE delincommitteeid=@inId;";
                        var cmd = new MySqlCommand(cmdStr, connection);
                        cmd.Parameters.AddWithValue("@inId", isIn.Value);
                        cmd.Parameters.AddWithValue("@mincount", mincount);
                        cmd.Parameters.AddWithValue("@maxcount", maxcount);
                        cmd.ExecuteNonQuery();

                        return true;
                    }
                    else
                    {
                        //Insert if doesnt exist at the moment
                        var cmdStr = "INSERT INTO delegation_in_committee (linkid, committeeid, mincount, maxcount) VALUES (@linkid, @committeeid, @mincount, @maxcount);";
                        var cmd = new MySqlCommand(cmdStr, connection);
                        cmd.Parameters.AddWithValue("@linkid", linkid.Value);
                        cmd.Parameters.AddWithValue("@committeeid", committee.ID);
                        cmd.Parameters.AddWithValue("@mincount", mincount);
                        cmd.Parameters.AddWithValue("@maxcount", maxcount);
                        cmd.ExecuteNonQuery();

                        committee.DelegationList.Add(delegation.ID);
                        this.conferences.FirstOrDefault(n => n.ID == committee.ConferenceID)?.Committees.FirstOrDefault(n => n.ID == committee.ID)?.DelegationList.Add(delegation.ID);

                        return true;

                    }
                    
                }
            }
            else
            {
                return false;
            }
        }

        public bool RemoveDelegationFromCommittee(CommitteeModel committee, DelegationModel delegation)
        {
            var linkid = GetDelegationConferenceLinkId(committee.ConferenceID, delegation.ID);
            if (linkid.HasValue)
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    var cmdStr = "DELETE FROM delegation_in_committee WHERE linkid=@linkid AND committeeid=@committeeid";
                    var cmd = new MySqlCommand(cmdStr, connection);
                    cmd.Parameters.AddWithValue("@linkid", linkid);
                    cmd.Parameters.AddWithValue("@committeeid", committee.ID);
                    cmd.ExecuteNonQuery();
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        public int? GetDelegationConferenceLinkId(string conferenceid, string delegationid)
        {
            int? value = null;

            using (var connection = new MySqlConnection(_connectionString))
            {
                var cmdStr = "SELECT linkid FROM conference_delegation WHERE conference_id=@confid AND delegation_id=@delid;";
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@confid", conferenceid);
                cmd.Parameters.AddWithValue("@delid", delegationid);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        value = reader.GetInt16(0);
                    }
                }
            }

            return value;
        }

        public List<DelegationModel> GetDelegationsOfConference(ConferenceModel conference)
        {
            return GetDelegationsOfConference(conference.ID);
        }

        public List<DelegationModel> GetDelegationsOfConference(string conferenceid)
        {
            var list = new List<DelegationModel>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                //Anhängen der ISO
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

        #region Team

        public int AddTeamRole(TeamRoleModel role)
        {
            var val = Tools.Connection(_connectionString).ConferenceTeamRoles.Insert(role);
            return Convert.ToInt32(val);
        }

        internal List<TeamRoleModel> GetRolesOfConference(string conferenceid)
        {
            var val = Tools.Connection(_connectionString).ConferenceTeamRoles.GetElements<TeamRoleModel>("conferenceid", conferenceid);
            return val;
        }

        public void AddUserToConferenceTeam(UserModel user, ConferenceModel conference, TeamRoleModel role)
        {
            var model = new ConferenceTeamUserModel();
            model.ConferenceId = conference.ID;
            model.RoleId = role.Id.Value;
            model.UserId = user.Id;
            Tools.Connection(_connectionString).ConferenceTeam.Insert(model);
        }

        public List<UserModel> GetTeamUsers(ConferenceModel conference)
        {
            var list = new List<UserModel>();
            var cmdStr = "SELECT `user`.* FROM `user` " +
                "INNER JOIN conference_team ON conference_team.userid = `user`.id " +
                "WHERE conference_team.conferenceid = @conferenceid;";
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@conferenceid", conference.ID);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(DataReaderConverter.ObjectFromReader<UserModel>(reader));
                    }
                }
            }
            return list;
        }

        public List<TeamRoleModel> GetUserTeamRolesAtConference(UserModel user, ConferenceModel conference)
        {
            var roles = new List<TeamRoleModel>();
            var cmdStr = "SELECT conference_team_roles.* FROM conference_team_roles " +
                "INNER JOIN conference_team ON conference_team.role = conference_team_roles.id " +
                "WHERE conference_team.userid = @userid AND conference_team.conferenceid = @conferenceid";
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@userid", user.Id);
                cmd.Parameters.AddWithValue("@conferenceid", conference.ID);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        roles.Add(DataReaderConverter.ObjectFromReader<TeamRoleModel>(reader));
                    }
                    
                }
            }
            return roles;
        }

        public List<UserConferenceRoleModel> GetConferenceTeam(ConferenceModel conference)
        {
            var list = new List<UserConferenceRoleModel>();
            //Was ein kack, aber viel kürzer fällt mir gerade nichts ein...
            var cmdStr = "SELECT conference_team_roles.id as 'role.id', " +
                "conference_team_roles.`name` as 'role.name', " +
                "conference_team_roles.description as 'role.description', " +
                "conference_team_roles.parentrole as 'role.parentrole', " +
                "conference_team_roles.mincount as 'role.mincount', " +
                "conference_team_roles.maxcount as 'role.maxcount', " +
                "`user`.id as 'user.id', " +
                "`user`.username as 'user.username', " +
                "`user`.forename as 'user.forename', " +
                "`user`.lastname as 'user.lastname' FROM conference_team_roles " +
                "INNER JOIN conference_team ON conference_team.role = conference_team_roles.id " +
                "INNER JOIN `user` ON conference_team.userid = `user`.id WHERE " +
                "conference_team.conferenceid=@conferenceid;";
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@conferenceid", conference.ID);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var model = new UserConferenceRoleModel();
                        model.User = DataReaderConverter.ObjectFromReader<UserModel>(reader, "user");
                        model.Role = DataReaderConverter.ObjectFromReader<TeamRoleModel>(reader, "role");
                        list.Add(model);
                    }
                }
            }
            return list;
        }

        public ConferenceUserAuthModel GetConferenceUserAuth(ConferenceModel conference, UserModel user)
        {
            if (conference == null || user == null)
                return null;
            return GetConferenceUserAuth(conference.ID, user.Id);
        }

        public ConferenceUserAuthModel GetConferenceUserAuth(string conferenceid, string userid)
        {
            var filters = new Dictionary<string, object>();
            filters.Add("conferenceid", conferenceid);
            filters.Add("userid", userid);
            return Tools.Connection(_connectionString).Table(conference_user_auth_table_name).First<ConferenceUserAuthModel>(filters);
        }

        public int GetConferenceCount()
        {
            return Tools.Connection(_connectionString).Table(conference_table_name).Count();
        }

        #endregion

        public ConferenceService(string connectionString)
        {
            this._connectionString = connectionString;
            Console.WriteLine("Conference-Service Started!");
        }
    }
}
