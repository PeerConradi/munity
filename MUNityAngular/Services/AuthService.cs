using MUNityAngular.DataHandlers.Database;
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

        public (bool status, string key) Login(string username, string password)
        {
            var success = false;
            var customAuthKey = string.Empty;

            var cmdStr = "SELECT id, password, salt FROM user WHERE username=@username";
            using (var connection = Connector.Connection)
            {
                connection.Open();
                string userid = string.Empty;
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@username", username);
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

                if (success == true)
                {
                    RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
                    byte[] randoms = new byte[64];
                    rngCsp.GetBytes(randoms);
                    customAuthKey = Convert.ToBase64String(randoms);
                    cmdStr = "INSERT INTO auth (key, userid, createdate, expiredate) VALUES " +
                        "(@key, @userid, @createdate, @expiredate)";
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

        public bool Register(string username, string password, string email)
        {
            if (!UsernameAvailable(username))
                return false;

            var cmdStr = "INSERT INTO " + user_table_name + "(id, username, password, salt, mail, registerdate, status) VALUES " +
                "(@id, @username, @password, @salt, @mail, @registerdate, @status)";

            var hash = Util.Hashing.PasswordHashing.InitHashing(password);

            using (var connection = Connector.Connection)
            {
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@id", username);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", hash.Key);
                cmd.Parameters.AddWithValue("@salt", hash.Salt);
                cmd.Parameters.AddWithValue("@mail", email);
                cmd.Parameters.AddWithValue("@registerdate", DateTime.Now);
                cmd.Parameters.AddWithValue("@status", "OK");
                cmd.ExecuteNonQuery();
            }

            return true;
        }

        public bool ChangePassword(string userid, string oldpassword, string newpassword)
        {
            throw new NotImplementedException();
        }

        public bool UsernameAvailable(string username)
        {
            bool available = true;
            var cmdStr = "SELECT * FROM " + user_table_name + " WHERE username=@username";
            using (var connection = Connector.Connection)
            {
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@username", username);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                        available = false;
                }
            }
            return available;
        }

        public bool ValidateAuthKey(string authkey)
        {
            return true;
        }

        
    }
}
