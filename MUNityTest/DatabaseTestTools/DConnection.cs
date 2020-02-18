using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace MUNityTest.DatabaseTestTools
{
    public class DConnection
    {
        private string _connectionString;

        public string ConnectionString { get => _connectionString; }

        public DConnection(string connectionString)
        {
            this._connectionString = connectionString;
        }
        
    }

    public static class Tools
    {
        public static DConnection Connection(string cs)
        {
            return new DConnection(cs);
        }
    }

    public class DTable
    {
        private string _name;
        private string _connectionString;

        public DTable(string name, string connectionstring)
        {
            this._name = name;
            this._connectionString = connectionstring;
        }

        public bool HasEntry(string key, object value)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var cmdStr = "SELECT * FROM " + _name + " WHERE " + key + "=@value;";
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@value", value);
                using (var reader = cmd.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
        }

        /// <summary>
        /// Get Entries with one filter condition
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Dictionary<string, object> GetEntries(string key, object value)
        {
            var dict = new Dictionary<string, object>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var cmdStr = "SELECT * FROM " + _name + " WHERE " + key + "=@value;";
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@value", value);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        for (int i=0;i<reader.FieldCount; i++)
                        {
                            dict.Add(reader.GetName(i), reader.GetFieldValue<object>(i));
                        }
                    }
                }
            }
            return dict;
        }
    }


    public static class TableExtension
    {
        public static DTable Table(this DConnection connection, string name)
        {
            return new DTable(name, connection.ConnectionString);
        }
    }
}
