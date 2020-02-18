using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MUNityAngular.DataHandlers.Database
{

    /// <summary>
    /// The Connector Class handles every connection to the Database itself
    /// and has some functionality to allow quick checks for tables etc.
    /// </summary>
    public class Connector
    {
        private const string DATABASE_NAME = "munity";
        private const string DATABASE_USER = "root";
        private const string DATABASE_PASSWORD = "12345";

        //Private Connection String
        //private static string connectionString { get; set; } = @"server=localhost;userid=root;password='';database='munity'";

        //Release Connection String
        private static string connectionString { get; set; } = @"server=localhost;userid=munity-user;password='12345';database='munity'";

        public static bool EnsureDatabaseExists()
        {
            connectionString = @"server=localhost;userid=" + DATABASE_USER + ";password=" + DATABASE_PASSWORD;
            using (var connection = Connection)
            {
                //TODO: Was passiert wenn die Datenbank nicht da ist? Kann der Server mit einer Art
                //Mindestkonfiguration selbst ohne Datenbank laufen? Sodass die Services keine Anfragen an die
                //Datenbank stellen und alles in einen Temporären Speicher schreiben, das wird am Ende natürlich
                //ein großer im Nachgang alles in eine Datenbank einzupflegen.

                connection.Open();
                //Create Database if not exists (AddWithValue failed here)
                string s0 = "CREATE DATABASE IF NOT EXISTS `" + DATABASE_NAME + "`;";
                var cmd = new MySqlCommand(s0, connection);
                cmd.ExecuteNonQuery();
            }
            connectionString += ";database=" + DATABASE_NAME + ";";
            return true;
        }

        public static bool DoesDatabaseExist()
        {
            var cmdStr = "SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = @dbname";
            using (var connection = Connection)
            {
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@dbname", DATABASE_NAME);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.GetString(0) != DATABASE_NAME)
                            return false;
                        return true;
                    }
                    return false;
                }
            }
        }

        public static MySqlConnection Connection
        {
            get => new MySqlConnection(connectionString);
        }

        public static bool DoesTableExists(string tablename)
        {
            var val = false;
            using (var connection = Connection)
            {
                
                var cmdStr = "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = '" + DATABASE_NAME +  "' AND table_name = @tablename";

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

        public static bool AddColumnIfNotExists()
        {
            throw new NotImplementedException();
        }

        public static DatabaseSaveAttribute GetAttributeForProperty(PropertyInfo property)
        {
            return (DatabaseSaveAttribute)property.GetCustomAttribute(typeof(DatabaseSaveAttribute));
        }

        public static (PrimaryKeyAttribute, PropertyInfo) GetPrimaryKey(object o)
        {
            PrimaryKeyAttribute val = null;


            var keys = o.GetType().GetProperties().Select(n => n.GetCustomAttribute(typeof(PrimaryKeyAttribute)));
            var property = o.GetType().GetProperties().Where(n => n.GetCustomAttribute(typeof(PrimaryKeyAttribute)) != null)?.First();
            if (keys.Count() > 0)
                val = (PrimaryKeyAttribute)keys.First();

            return (val, property);
        }

        

        public static bool CreateTable(string tablename, Type sourceObject)
        {
            using (var connection = Connection)
            {
                connection.Open();
                string cmdStr = "CREATE TABLE IF NOT EXISTS `" + tablename + "` (";
                var primarykeys = new List<string>();
                foreach (var prep in sourceObject.GetProperties())
                {
                    var databaseAttr = GetAttributeForProperty(prep);
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

        public static bool CreateConnectionTable(string tablename, Type wrapperObject, Type ListObject)
        {
            using (var connection = Connection)
            {
                connection.Open();
                string cmdStr = "CREATE TABLE IF NOT EXISTS `" + tablename + "` (";
                var primarykeys = new List<string>();
                var wrapperPrimaryKeys = wrapperObject.GetProperties().Where(n => n.GetCustomAttribute(typeof(PrimaryKeyAttribute)) != null);
                if (wrapperPrimaryKeys.Count() == 0)
                    throw new ArgumentException("wrapper object does not have any PrmiaryKey");


                wrapperObject.Name.ToLower();
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

        /// <summary>
        /// Inserts a new row into the database
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [Obsolete("Use the Get Insertion String Method in future implementations and make sure every service uses its own connection String")]
        public static bool Insert(string tablename, object model)
        {
            var cmdStr = "INSERT INTO " + tablename + " (";
            var paramList = new List<string>();
            var valList = new Dictionary<string, object>();
            var t = model.GetType();
            foreach (var prep in t.GetProperties())
            {
                var attr = Connector.GetAttributeForProperty(prep);
                if (attr != null)
                {
                    if (prep.GetValue(model) != null)
                    {
                        paramList.Add(attr.ColumnName);
                        valList.Add("@" + attr.ColumnName, prep.GetValue(model));
                    }

                }
            }
            paramList.ForEach(n => cmdStr += n + ",");
            if (cmdStr.EndsWith(',')) cmdStr = cmdStr.Substring(0, cmdStr.Length - 1);
            cmdStr += ") VALUES (";
            foreach (var item in valList)
            {
                cmdStr += item.Key + ",";
            }
            if (cmdStr.EndsWith(',')) cmdStr = cmdStr.Substring(0, cmdStr.Length - 1);
            cmdStr += ")";
            using (var connection = Connector.Connection)
            {
                var cmd = new MySqlCommand(cmdStr, connection);
                foreach (var item in valList)
                {
                    cmd.Parameters.AddWithValue(item.Key, item.Value);
                }
                connection.Open();
                cmd.ExecuteNonQuery();
            }
            return true;
        }

        public static MySqlCommand GetInsertionCommand(string tablename, object model)
        {
            var cmdStr = "INSERT INTO " + tablename + " (";
            var paramList = new List<string>();
            var valList = new Dictionary<string, object>();
            var t = model.GetType();
            foreach (var prep in t.GetProperties())
            {
                var attr = Connector.GetAttributeForProperty(prep);
                if (attr != null)
                {
                    if (prep.GetValue(model) != null)
                    {
                        paramList.Add(attr.ColumnName);
                        valList.Add("@" + attr.ColumnName, prep.GetValue(model));
                    }

                }
            }
            paramList.ForEach(n => cmdStr += n + ",");
            if (cmdStr.EndsWith(',')) cmdStr = cmdStr.Substring(0, cmdStr.Length - 1);
            cmdStr += ") VALUES (";
            foreach (var item in valList)
            {
                cmdStr += item.Key + ",";
            }
            if (cmdStr.EndsWith(',')) cmdStr = cmdStr.Substring(0, cmdStr.Length - 1);
            cmdStr += ")";
            var cmd = new MySqlCommand(cmdStr);
            foreach (var item in valList)
            {
                cmd.Parameters.AddWithValue(item.Key, item.Value);
            }
            return cmd;
        }
    
        [Obsolete("This function is not fully working and tested right now. Please dont use it!")]
        public static bool Update(string tablename, object oldModel, object newModel, bool allowPrimaryKeyChange = false)
        {
            if (oldModel == null)
                return Insert(tablename, newModel);

            if (oldModel.GetType() != newModel.GetType())
                throw new Exception("The Model have to be from the same type");

            var primaryKey = GetPrimaryKey(newModel);
            if (primaryKey.Item1 == null)
                throw new Exception("Primary Key not found!");

            Dictionary<string, object> newValues = new Dictionary<string, object>();
            var modelType = newModel.GetType();
            foreach(var newProp in modelType.GetProperties())
            {
                var attr = GetAttributeForProperty(newProp);
                //Do not allow to change the Primary key
                if (newProp.GetCustomAttribute(typeof(PrimaryKeyAttribute)) == null && allowPrimaryKeyChange == false || allowPrimaryKeyChange == true)
                {
                    if (attr != null)
                    {
                        var oldPropValue = modelType.GetProperty(newProp.Name)?.GetValue(oldModel);
                        var newPropValue = newProp.GetValue(newModel);
                        if (oldPropValue != newProp.GetValue(newModel))
                        {
                            newValues.Add(newProp.Name, newPropValue);
                        }
                    }
                }
                
            }

            if (newValues.Count == 0)
                return false;

            var cmdStr = "UPDATE " + tablename + " SET ";
            //TODO: Column setzen ausgehend von den newValues Dict
            foreach (var key in newValues)
            {
                cmdStr += " " + key.Key + "=" + "@" + key.Key;
            }
            cmdStr += " WHERE " + primaryKey.Item2.Name + " = @" + primaryKey.Item2.Name + ";";

            using (var connection = Connector.Connection)
            {
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                //Insert all the new Values
                foreach(var keyVal in newValues)
                {
                    cmd.Parameters.AddWithValue("@" + keyVal.Key, keyVal.Value);
                }
                cmd.Parameters.AddWithValue("@" + primaryKey.Item2.Name, primaryKey.Item2.GetValue(newModel));
                cmd.ExecuteNonQuery();
            }
            return true;           
        }
    }
}
