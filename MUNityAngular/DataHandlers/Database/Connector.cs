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
        private const string DATABASE_PASSWORD = "";

        private static List<IDatabaseHandler> databaseHandlers;

        public static void RegisterDatabaseHandler(IDatabaseHandler handler)
        {
            if (databaseHandlers == null)
                databaseHandlers = new List<IDatabaseHandler>();
            databaseHandlers.Add(handler);
        }

        

        private static string connectionString { get; set; }

        public static bool InitializeConnection()
        {
            connectionString = @"server=localhost;userid=" + DATABASE_USER + ";password=" + DATABASE_PASSWORD;
            using (var connection = Connection)
            {
                connection.Open();
                //Create Database if not exists (AddWithValue failed here)
                string s0 = "CREATE DATABASE IF NOT EXISTS `" + DATABASE_NAME + "`;";
                var cmd = new MySqlCommand(s0, connection);
                cmd.ExecuteNonQuery();
            }
            connectionString += ";database=" + DATABASE_NAME + ";";
            return true;
        }

        public static void CreateOrUpdateTables()
        {
            databaseHandlers.ForEach(n => {
                if (n.TableStatus == DatabaseInformation.ETableStatus.NotExisting)
                    n.CreateTables();
            });
        }

        public static void ClearHandlers()
        {
            databaseHandlers.Clear();
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
            if (cmdStr.EndsWith(',')) cmdStr.Remove(cmdStr.Length - 1);
            cmdStr += ") VALUES (";
            foreach (var item in valList)
            {
                cmdStr += item.Key;
            }
            if (cmdStr.EndsWith(',')) cmdStr.Remove(cmdStr.Length - 1);
            cmdStr += ")";
            using (var connection = Connector.Connection)
            {
                var cmd = new MySqlCommand(cmdStr, connection);
                foreach (var item in valList)
                {
                    cmd.Parameters.AddWithValue(item.Key, item.Value.ToString());
                }
                connection.Open();
                cmd.ExecuteNonQuery();
            }
            return true;
        }
    }
}
