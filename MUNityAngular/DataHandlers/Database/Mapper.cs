using MUNityAngular.Models.Conference;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MUNityAngular.DataHandlers.Database
{
    public class DConnection
    {
        private string _connectionString;

        public string ConnectionString { get => _connectionString; }

        public DConnection(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public void CreateTable(object preset)
        {
            throw new NotImplementedException();
        }

        public DTable ConferenceTeamRoles
        {
            get => new DTable("conference_team_roles", ConnectionString);
        }

        public DTable ConferenceTeam { get => new DTable("conference_team", ConnectionString); }

    }

    public static class Tools
    {
        public static DConnection Connection(string cs)
        {
            return new DConnection(cs);
        }
    }

    public class DEntry
    {
        private string _name;
        private object _value;
        public string Name { get => _name; }
        public object Value { get => _value;  }
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

        public List<T> GetElements<T>()
        {
            var list = new List<T>();
            var cmdStr = "SELECT * FROM " + _name + ";";
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(DataReaderConverter.ObjectFromReader<T>(reader));
                    }
                    
                }
            }
            return list;
        }

        public List<T> GetElements<T>(string conditionKey, object conditionValue)
        {
            var list = new List<T>();
            var cmdStr = "SELECT * FROM " + _name + " WHERE " + conditionKey + " = @conditionValue;";
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@conditionValue", conditionValue);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(DataReaderConverter.ObjectFromReader<T>(reader));
                    }

                }
            }
            return list;
        }

        public T First<T>(string conditionKey, object conditionValue)
        {
            var cmdStr = "SELECT * FROM " + _name + " WHERE " + conditionKey + "=@conditionvalue;";
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@conditionvalue", conditionValue);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return DataReaderConverter.ObjectFromReader<T>(reader);
                    }
                }
            }
            throw new NullReferenceException("No element found");
        }

        public T First<T>()
        {
            var cmdStr = "SELECT * FROM " + _name + ";";
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return DataReaderConverter.ObjectFromReader<T>(reader);
                    }
                }
            }
            throw new NullReferenceException("No element found");
        }

        /// <summary>
        /// Get Entries with one filter condition
        /// </summary>
        /// <param name="key">a condition key</param>
        /// <param name="value">a condtion value</param>
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
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            dict.Add(reader.GetName(i), reader.GetFieldValue<object>(i));
                        }
                    }
                }
            }
            return dict;
        }

        public List<Dictionary<string, object>> GetEntries()
        {
            var list = new List<Dictionary<string, object>>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var cmdStr = "SELECT * FROM " + _name + ";";
                var cmd = new MySqlCommand(cmdStr, connection);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var dict = new Dictionary<string, object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            dict.Add(reader.GetName(i), reader.GetFieldValue<object>(i));
                        }
                        list.Add(dict);
                    }
                }
            }
            
            return list;
        }
    
        public void SetEntry(Dictionary<string, object> conditions, string fieldname, object newValue)
        {
            var cmdStr = "UPDATE " + _name + " SET " + fieldname + " = @newValue";
            if (conditions.Count > 0)
            {
                cmdStr += " WHERE ";
                for (int i=0;i<conditions.Count;i++)
                {
                    var elementKey = conditions.ElementAt(i).Key;
                    cmdStr += elementKey + "=@" + elementKey;
                    if (i < conditions.Count - 1)
                    {
                        cmdStr += " AND ";
                    }
                }
            }
            else
            {
                //No conditions close the command
                cmdStr += ";";
            }
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@newValue", newValue);
                for (int i = 0; i < conditions.Count; i++)
                {
                    var element = conditions.ElementAt(i);
                    cmd.Parameters.AddWithValue("@" + element.Key, element.Value);
                }

                cmd.ExecuteNonQuery();
            }
            
        }
    
        public void SetEntry(string conditionKey, string conditionValue, string fieldname, object newValue)
        {
            var cmdStr = "UPDATE " + _name + " SET " + fieldname + " = @newValue WHERE " + conditionKey + "=@conditionValue";
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@newValue", newValue);
                cmd.Parameters.AddWithValue("@conditionValue", conditionValue);
                cmd.ExecuteNonQuery();
            }
        }

        public void RemoveEntry(string conditionKey, object conditionValue)
        {
            var cmdStr = "DELETE FROM " + _name + " WHERE " + conditionKey + "=@conditionValue";
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@conditionValue", conditionValue);
                cmd.ExecuteNonQuery();
            }
        }

        public void Remove(object element)
        {
            var primaryKeyField = Connector.GetPrimaryKey(element);
            if (primaryKeyField.attribute == null)
                throw new Exception("Invalid Element given, it has no primary key!");

            RemoveEntry(primaryKeyField.propertyInfo.Name, primaryKeyField.propertyInfo.GetValue(element));
        }

        public void Update(object o)
        {
            var hasPrimaryKey = o.GetType().GetProperties().Any(n => n.GetCustomAttribute(typeof(PrimaryKeyAttribute)) != null);
            if (hasPrimaryKey)
            {
                
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    var cmd = Connector.CreateUpdateCommand(_name, o);
                    cmd.Connection = connection;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        internal object Insert(object element)
        {
            if (element == null)
                throw new ArgumentNullException("The element cannot be null");

            var cmd = Connector.GetInsertionCommand(_name, element);
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                cmd.Connection = connection;
                cmd.ExecuteNonQuery();
                cmd.CommandText = "SELECT @@Identity;";
                return cmd.ExecuteScalar();
            }
        }
    }




    public static class Extensions
    {
        public static DTable Table(this DConnection connection, string name)
        {
            return new DTable(name, connection.ConnectionString);
        }

        
    }
}
