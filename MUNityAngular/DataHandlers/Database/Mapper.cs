using MUNityAngular.Models.Conference;
using MUNityAngular.Models.User;
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

        public bool CreateTable(string tablename, Type sourceObject)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string cmdStr = "CREATE TABLE IF NOT EXISTS `" + tablename + "` (";
                var primarykeys = new List<string>();
                foreach (var prep in sourceObject.GetProperties())
                {
                    var databaseAttr = Connector.GetAttributeForProperty(prep);
                    if (databaseAttr != null)
                    {
                        if (databaseAttr.FieldType == DatabaseSaveAttribute.EFieldType.AUTO)
                        {
                            cmdStr += "`" + databaseAttr.ColumnName + "` " + DatabaseInformation.GetDatabaseType(prep.PropertyType) + ",";
                        }
                        else
                        {
                            cmdStr += "`" + databaseAttr.ColumnName + "` " + DatabaseInformation.GetDatabaseType(databaseAttr.FieldType) + ",";
                        }

                        var pkAttr = (PrimaryKeyAttribute)prep.GetCustomAttribute(typeof(PrimaryKeyAttribute));
                        if (pkAttr != null)
                            primarykeys.Add(databaseAttr.ColumnName);
                    }
                }
                //PRIMARY KEYS
                if (primarykeys.Count > 0)
                    cmdStr += "PRIMARY KEY (";

                primarykeys.ForEach(key =>
                {
                    cmdStr += key + ",";
                });
                //delete last ,
                if (cmdStr.EndsWith(','))
                    cmdStr = cmdStr.Substring(0, cmdStr.Length - 1);

                if (primarykeys.Count > 0)
                    cmdStr += ")";

                cmdStr += ");";
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.ExecuteNonQuery();
            }
            return true;
        }

        public bool DoesDatabaseExist(string database)
        {
            var cmdStr = "SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = @dbname";
            using (var connection = new MySqlConnection(_connectionString))
            {
                
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@dbname", database);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.GetString(0) != database)
                            return false;
                        return true;
                    }
                    return false;
                }
            }
        }

        public bool DoesTableExists(string tablename)
        {
            var val = false;
            using (var connection = new MySqlConnection(ConnectionString))
            {
                var cmdStr = "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = '" + connection.Database + "' AND table_name = @tablename";

                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@tablename", tablename);
                connection.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int count = reader.GetInt32(0);
                    if (count == 1) val = true;
                }
            }
            return val;
        }

        public DTable ConferenceTeamRoles
        {
            get => new DTable("conference_team_roles", ConnectionString);
        }

        public DTable ConferenceTeam { get => new DTable("conference_team", ConnectionString); }

        public DTable User { get => new DTable("user", ConnectionString); }

        public DTable Resolution { get => new DTable("resolution", ConnectionString);  }

        public DTable ResolutionAuth { get => new DTable("resolution_auth", ConnectionString); }

        public DTable ConferenceUserAuth { get => new DTable("conference_user_auth", ConnectionString); }
    
        public DTable ResolutionConference { get => new DTable("resolution_conference", ConnectionString); }

        public DTable CommitteeStatus { get => new DTable("committee_status", ConnectionString); }
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

        public List<T> Where<T>(string wherecommand)
        {
            var list = new List<T>();
            var cmdStr = "SELECT * FROM " + _name + " WHERE " + wherecommand + ";";
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

        public List<T> Select<T>(string columnname)
        {
            var list = new List<T>();
            var cmdStr = "SELECT " + columnname + " FROM " + _name + ";";
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(reader.GetFieldValue<T>(0));
                    }
                }
            }
            return list;
        }

        public List<T> SelectWhere<T> (string columnname, string wherecommand) 
        {
            var list = new List<T>();
            var cmdStr = "SELECT " + columnname + " FROM " + _name + " WHERE "+ wherecommand + ";";
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(reader.GetFieldValue<T>(0));
                    }
                }
            }
            return list;
        }

        public int CountWhere(string wherecommand)
        {
            var cmdStr = "SELECT COUNT(*) FROM " + _name + " WHERE " + wherecommand + ";";
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return reader.GetInt32(0);
                    }
                }
            }
            return 0;
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

        public int Count()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var cmdStr = "SELECT COUNT(*) FROM " + _name;
                var cmd = new MySqlCommand(cmdStr, connection);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return reader.GetInt32(0);
                    }
                }
            }
            return 0;
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
            if (conditionKey == null)
            {
                throw new ArgumentException("The condition key can not be null");
            }

            var match = conditionKey.IndexOfAny("'\n\"#!§$%&/()=?`´~*,".ToCharArray()) != -1;
            if (match)
            {
                throw new ArgumentException("The condition key has detected an illigal character");
            }

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
            return default;
        }

        public T First<T>(Dictionary<string, object> filter)
        {

            var cmdStr = "SELECT * FROM " + _name;

            if (filter.Count > 0)
            {
                cmdStr += " WHERE";
                for (int i=0;i<filter.Count; i++)
                {
                    if (i > 0)
                        cmdStr += " AND ";

                    var valName = filter.ElementAt(i).Key;
                    cmdStr += " " + valName + " = @" + valName;
                }
            }

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                foreach(var option in filter)
                {
                    cmd.Parameters.AddWithValue("@" + option.Key, option.Value);
                }
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

        internal void ReplaceInto(object element)
        {
            var cmd = Connector.GetReplaceCommand(_name, element);
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                cmd.Connection = connection;
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Inserts or Updates a value and returns the primary Key
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        internal object UpdateOrInsert(object element)
        {
            //Suche nach dem PrimaryKey
            var pkProperty = Connector.GetPrimaryKey(element);

            if (pkProperty.attribute == null)
                throw new Exception("The object has no primary Key to do that!");

            //Hole den DatabaseSave des PrimaryKeys
            var databaseColumnName = pkProperty.propertyInfo.GetCustomAttribute<DatabaseSaveAttribute>();
            var primaryKeyValue = pkProperty.propertyInfo.GetValue(element);
            if (primaryKeyValue == null)
            {
                //Wenn kein Primarykey vorhanden ist muss das objekt wohl inserted werden
                return Insert(element);
            } else
            {
                if (HasEntry(databaseColumnName.ColumnName, primaryKeyValue))
                {
                    Update(element);
                    return primaryKeyValue;
                }
                else
                {
                    return Insert(element);
                }
                
            }

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
