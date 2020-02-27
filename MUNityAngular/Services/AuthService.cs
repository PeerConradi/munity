using MUNityAngular.DataHandlers.Database;
using MUNityAngular.Models.User;
using MUNityAngular.Models.Resolution;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace MUNityAngular.Services
{
    public class AuthService
    {
        private const string user_table_name = "user";
        private const string auth_table_name = "auth";

        private string _connectionString;

        private bool registrationOpened = true;
        public bool IsRegistrationOpened { get => registrationOpened; 
        set
            {
                this.registrationOpened = value;
            }
        }

        public enum EUserClearance
        {
            Default,
            CreateConference
        }

        public bool IsDefaultAuth(string auth)
        {
            return auth == "default";
        }

        public (bool status, string key) Login(string username, string password)
        {
            var success = false;
            var customAuthKey = string.Empty;

            var cmdStr = "SELECT id, password, salt FROM user WHERE username=@username";
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string userid = string.Empty;
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@username", username.ToLower());
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        userid = reader.GetString("id");
                        var hash = reader.GetString("password");
                        var salt = reader.GetString("salt");
                        if (Util.Hashing.PasswordHashing.CheckPassword(password, salt, hash))
                            success = true;
                    }
                }

                //Auth Key
                if (success == true)
                {
                    RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
                    byte[] randoms = new byte[64];
                    rngCsp.GetBytes(randoms);
                    customAuthKey = Convert.ToBase64String(randoms);
                    cmdStr = "INSERT INTO " + auth_table_name + " (authkey, userid, createdate, expiredate) VALUES " +
                        "(@key, @userid, @createdate, @expiredate);";
                    cmd = new MySqlCommand(cmdStr, connection);
                    cmd.Parameters.AddWithValue("@key", customAuthKey);
                    cmd.Parameters.AddWithValue("@userid", userid);
                    cmd.Parameters.AddWithValue("@createdate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@expiredate", DateTime.Now.AddDays(1));
                    cmd.ExecuteNonQuery();
                }
            }

            return (success, customAuthKey);
        }

        public void DeleteAllUserKeys(string userid)
        {
            Tools.Connection(_connectionString).Table(auth_table_name).RemoveEntry("userid", userid);
        }

        public void DeleteAccount(string id)
        {
            //Delete all User Keys
            DeleteAllUserKeys(id);
            Tools.Connection(_connectionString).Table(user_table_name).RemoveEntry("id", id);
        }

        public string GetHeadAdminId()
        {
            var entries = Tools.Connection(_connectionString).Table("admin").GetEntries("rank", "5");
            if (entries.ContainsKey("userid"))
                return entries["userid"] as string;
            return null;
        }



        public (bool status, string key) LoginWithId(string userid, string password)
        {
            var success = false;
            var customAuthKey = string.Empty;

            var cmdStr = "SELECT password, salt FROM user WHERE id=@userid";
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@userid", userid);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var hash = reader.GetString("password");
                        var salt = reader.GetString("salt");
                        if (Util.Hashing.PasswordHashing.CheckPassword(password, salt, hash))
                            success = true;
                    }
                }

                //Auth Key
                if (success == true)
                {
                    RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
                    byte[] randoms = new byte[64];
                    rngCsp.GetBytes(randoms);
                    customAuthKey = Convert.ToBase64String(randoms);
                    cmdStr = "INSERT INTO " + auth_table_name + " (authkey, userid, createdate, expiredate) VALUES " +
                        "(@key, @userid, @createdate, @expiredate);";
                    cmd = new MySqlCommand(cmdStr, connection);
                    cmd.Parameters.AddWithValue("@key", customAuthKey);
                    cmd.Parameters.AddWithValue("@userid", userid);
                    cmd.Parameters.AddWithValue("@createdate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@expiredate", DateTime.Now.AddDays(1));
                    cmd.ExecuteNonQuery();
                }
            }

            return (success, customAuthKey);
        }

        public (bool valid, string userid) CheckLoginData(string username, string password)
        {
            var success = false;
            string userid = string.Empty;
            var cmdStr = "SELECT id, password, salt FROM user WHERE username=@username";
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@username", username);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        userid = reader.GetString("id");
                        var hash = reader.GetString("password");
                        var salt = reader.GetString("salt");
                        success = Util.Hashing.PasswordHashing.CheckPassword(password, salt, hash);
                    }
                }
            }
            return (success, userid);
        }

        public bool CheckPasswordForUserid(string userid, string password)
        {
            var success = false;
            var cmdStr = "SELECT id, password, salt FROM user WHERE id=@userid";
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@userid", userid);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var hash = reader.GetString("password");
                        var salt = reader.GetString("salt");
                        if (Util.Hashing.PasswordHashing.CheckPassword(password, salt, hash))
                            success = true;
                    }
                }
            }
            return success;
        }

        internal void Logout(string auth)
        {
            if (string.IsNullOrEmpty(auth))
                return;

            var cmdStr = "DELETE FROM " + auth_table_name + " WHERE authkey=@authkey";
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@authkey", auth);
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteAuthKeysForUser(string userid)
        {
            if (string.IsNullOrEmpty(userid))
                return;

            var cmdStr = "DELETE FROM " + auth_table_name + " WHERE userid=@userid";
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@userid", userid);
                cmd.ExecuteNonQuery();
            }
        }

        public bool Register(string username, string password, string email)
        {
            if (!UsernameAvailable(username))
                return false;

            var cmdStr = "INSERT INTO " + user_table_name + "(id, username, password, salt, mail, registerdate, status) VALUES " +
                "(@id, @username, @password, @salt, @mail, @registerdate, @status)";

            var hash = Util.Hashing.PasswordHashing.InitHashing(password);

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@id", username.ToLower());
                cmd.Parameters.AddWithValue("@username", username.ToLower());
                cmd.Parameters.AddWithValue("@password", hash.Key);
                cmd.Parameters.AddWithValue("@salt", hash.Salt);
                cmd.Parameters.AddWithValue("@mail", email.ToLower());
                cmd.Parameters.AddWithValue("@registerdate", DateTime.Now);
                cmd.Parameters.AddWithValue("@status", "OK");
                cmd.ExecuteNonQuery();
            }

            return true;
        }

        public void SetPassword(string userid, string newPassword)
        {
            var hash = Util.Hashing.PasswordHashing.InitHashing(newPassword);
            Tools.Connection(_connectionString).Table(user_table_name).SetEntry("id", userid, "password", hash.Key);
            Tools.Connection(_connectionString).Table(user_table_name).SetEntry("id", userid, "salt", hash.Salt);
        }

        public void SetForename(string userid, string newForename) =>
            Tools.Connection(_connectionString).User.SetEntry("id", userid, "forename", newForename);

        public void SetLastname(string userid, string newForename) =>
            Tools.Connection(_connectionString).User.SetEntry("id", userid, "lastname", newForename);

        public bool UsernameAvailable(string username)
        {
            return !Tools.Connection(_connectionString).Table(user_table_name).HasEntry("username", username.ToLower());
        }

        public (bool valid, string userid) ValidateAuthKey(string authkey)
        {
            if (string.IsNullOrEmpty(authkey))
                return (false, null);

            var valid = false;
            string user = null;
            using (var connection = new MySqlConnection(_connectionString))
            {
                var cmdStr = "SELECT COUNT(*), userid FROM auth WHERE authkey=@authkey;";
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@authkey",authkey);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.GetInt16(0) == 1)
                        {
                            valid = true;
                            user = reader.GetString(1);
                        }
                            

                    }
                }
            }

            return (valid, user);
        }

        #region Resolution

        public bool CanCreateResolution(string auth)
        {
            if (auth == "default")
                return true;

            return ValidateAuthKey(auth).valid;
        }

        

        public bool CanEditResolution(string userid, ResolutionModel resolution)
        {

            //If the resolution is public edit return true
            if (GetResolutionPublicState(resolution).writeable)
                return true;

            //Check if the user is owner
            if (GetOwnerId(resolution) == userid)
                return true;

            //Check if the user has the right to edit this document
            //This is not the best way to do it, let me think of something better
            var count = Tools.Connection(_connectionString).ResolutionAuth.CountWhere("resolutionid='" + resolution.ID + "' AND userid='" + userid + "'");
            if (count >= 1)
                return true;

            //Check of the resolution is bindet to a conferece where the user is part
            var cmdStr = "SELECT resolution.* FROM resolution " +
                "INNER JOIN resolution_conference ON resolution_conference.resolutionid = resolution.id " +
                "INNER JOIN conference_team ON conference_team.conferenceid = resolution_conference.conferenceid " +
                "WHERE userid=@userid AND resolution.id=@resoid;";
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@userid", userid);
                cmd.Parameters.AddWithValue("@resoid", resolution.ID);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        return true;
                    }
                }
            }


            return false;
        }

        public (bool readable, bool writeable) GetResolutionPublicState(ResolutionModel resolution)
        {
            if (resolution == null)
                throw new ArgumentNullException("The Resolution cant be empty");

            return GetResolutionPublicState(resolution.ID);
        }

        public (bool readable, bool writeable) GetResolutionPublicState(string resolutionid)
        {
            var read = false;
            var write = false;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var cmdStr = "SELECT resolution.ispublicread, resolution.ispublicwrite FROM resolution WHERE resolution.id=@id";
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@id", resolutionid);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        try
                        {
                            read = reader.GetBoolean(0);
                            write = reader.GetBoolean(1);
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                        
                    }
                }
            }
            return (read, write);
        }

        public string GetOwnerId(ResolutionModel resolution)
        {
            string owner = null;

            if (resolution == null)
                throw new ArgumentNullException("Resolution cant be null");

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var cmdStr = "SELECT `user` FROM resolution WHERE resolution.id=@resoid;";
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@resoid", resolution.ID);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        owner = reader.GetString(0);
                    }
                }
            }
            return owner;
        }

        #endregion

        public UserAuths GetAuthsByAuthkey(string authkey)
        {
            var auths = new UserAuths();
            if (string.IsNullOrEmpty(authkey))
                return auths;

            using (var connection = new MySqlConnection(_connectionString))
            {
                var cmdStr = "SELECT user_clearance.* FROM `user`" +
                    " INNER JOIN user_clearance ON user_clearance.userid = `user`.id" +
                    " INNER JOIN auth ON auth.userid = `user`.id " +
                    " WHERE auth.authkey=@authkey";
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@authkey", authkey);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        auths = DataReaderConverter.ObjectFromReader<UserAuths>(reader);
                    }
                }
            }
            var validation = ValidateAuthKey(authkey);
            if (validation.valid)
            {
                if (IsAdmin(validation.userid))
                {
                    auths.SetAdmin();
                }
            }
            return auths;

        }

        public bool IsAdmin(string userid)
        {
            var cmdStr = "SELECT rank FROM admin WHERE userid=@userid";
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@userid", userid);
                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return false;

                    while (reader.Read())
                    {
                        if (reader.GetInt16(0) == 5)
                            return true;
                    }
                    

                    return false;
                }
            }
        }

        public class UserAuths
        {
            
            [DatabaseSave("userid")]
            public string UserId { get; set; } = null;

            [DatabaseSave("CreateConference")]
            public bool CreateConference { get; set; } = false;

            internal void SetAdmin()
            {
                CreateConference = true;
            }
        }
        
        public List<UserModel> GetAllUsers()
        {
            var users = new List<UserModel>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var cmdStr = "SELECT * FROM user;";
                var cmd = new MySqlCommand(cmdStr, connection);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var user = DataReaderConverter.ObjectFromReader<UserModel>(reader);
                        users.Add(user);
                    }
                    
                }
            }
            return users;
        }

        public int GetUserCount()
        {
            return Tools.Connection(_connectionString).Table(user_table_name).Count();
        }

        public UserModel GetUserByUsername(string username)
        {
            UserModel user = null;
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var cmdStr = "SELECT * FROM user WHERE username=@username;";
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@username", username);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        user = DataReaderConverter.ObjectFromReader<UserModel>(reader);
                    }

                }
            }
            return user;
        }

        public UserModel GetUserById(string id)
        {
            return Tools.Connection(_connectionString).Table(user_table_name).First<UserModel>("id", id);
        }

        public UserModel GetUserByAuth(string auth)
        {
            var cmdStr = "SELECT `user`.* FROM `user` INNER JOIN auth ON auth.userid = `user`.id WHERE auth.authkey = @authkey";
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@authkey", auth);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return DataReaderConverter.ObjectFromReader<UserModel>(reader);
                    }
                }
            }
            return null;
        }

        public AUser User(string userid)
        {
            return new AUser(_connectionString, userid);
        }

        public class AUser
        {
            private string _conenctionString;
            private string _userid;

            public AUser(string connectionString, string userid)
            {
                this._conenctionString = connectionString;
            }

            public bool CanEditResolution(string resolutionid)
            {
                var resolution = Tools.Connection(_conenctionString).Resolution.First<ResolutionAdvancedInfoModel>("id", resolutionid);
                if (resolution.PublicWrite)
                    return true;

                if (resolution.UserId == this._userid)
                    return true;

                var count = Tools.Connection(_conenctionString).ResolutionAuth.CountWhere("resolutionid=" + resolution.ID + " AND userid=" + _userid);
                if (count >= 1)
                    return true;

                return false;
            }
        }


        #region Konferenz

        public bool CanManageConference(string userid, string conferenceid)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var cmdStr = "SELECT FROM conference WHERE id=@conferenceid And creationuser=@userid;";
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@conferenceid", conferenceid);
                cmd.Parameters.AddWithValue("@userid", userid);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        //Der Benutzer ist der Ersteller der Konferenz und hat somit alle notwendigen rechte
                        return true;
                    }
                }
                cmd.CommandText = "SELECT CanEdit FROM conference_user_auth WHERE conferenceid=@conferenceid And userid=@userid;";
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            return reader.GetBoolean(0);
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        #endregion


        public AuthService(string connectionString)
        {
            _connectionString = connectionString;
            Console.WriteLine("Auth-Service Started!");
        }
    }
}
