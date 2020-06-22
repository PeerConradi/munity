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
    [Obsolete("Legacy Code: Was replaced with using the Entity Framework")]
    public class Connector
    {
        private const string DATABASE_NAME = "munity";

        //Private Connection String
        //private static string connectionString { get; set; } = @"server=localhost;userid=root;password='';database='munity'";

        //Release Connection String
        private static string connectionString { get; set; } = @"server=localhost;userid=munity-user;password='12345';database='munity'";

        public static DatabaseSaveAttribute GetAttributeForProperty(PropertyInfo property)
        {
            return (DatabaseSaveAttribute)property.GetCustomAttribute(typeof(DatabaseSaveAttribute));
        }

        public static (PrimaryKeyAttribute attribute, PropertyInfo propertyInfo) GetPrimaryKey(object o)
        {
            PrimaryKeyAttribute val = null;


            var keys = o.GetType().GetProperties().Select(n => n.GetCustomAttribute(typeof(PrimaryKeyAttribute)));
            var property = o.GetType().GetProperties().Where(n => n.GetCustomAttribute(typeof(PrimaryKeyAttribute)) != null)?.First();
            if (keys.Count() > 0)
                val = (PrimaryKeyAttribute)keys.First();

            return (val, property);
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

        public static MySqlCommand GetReplaceCommand(string tablename, object model)
        {
            var cmdStr = "REPLACE INTO " + tablename + " (";
            var paramList = new List<string>();
            var valList = new Dictionary<string, object>();

            var parametrable = model.GetType().GetProperties().Where(n => n.GetCustomAttribute<DatabaseSaveAttribute>() != null);

            //Build the command!
            foreach(var prop in parametrable)
            {
                var attr = prop.GetCustomAttribute<DatabaseSaveAttribute>();
                cmdStr += attr.ColumnName + ",";
            }
            if (cmdStr.EndsWith(',')) cmdStr = cmdStr.Substring(0, cmdStr.Length - 1);
            cmdStr += ") VALUES (";
            foreach (var prop in parametrable)
            {
                var attr = prop.GetCustomAttribute<DatabaseSaveAttribute>();
                cmdStr += "@" + attr.ColumnName + ",";
            }
            if (cmdStr.EndsWith(',')) cmdStr = cmdStr.Substring(0, cmdStr.Length - 1);
            cmdStr += ")";

            var cmd = new MySqlCommand(cmdStr);
            foreach (var prop in parametrable)
            {
                var attr = prop.GetCustomAttribute<DatabaseSaveAttribute>();
                cmd.Parameters.AddWithValue("@" + attr.ColumnName, prop.GetValue(model));
            }
            return cmd;
        }
    
        /// <summary>
        /// Updates new object in the given table, notice that it needs a primary key (only one!)
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="newModel"></param>
        /// <returns></returns>
        public static MySqlCommand CreateUpdateCommand(string tablename, object newModel)
        {

            var primaryKey = GetPrimaryKey(newModel);
            if (primaryKey.Item1 == null)
                throw new Exception("Primary Key not found!");

            //UPDATE tableName SET key=@value WHERE primaryKeyName=@pkValue

            Dictionary<string, object> newValues = new Dictionary<string, object>();
            var modelType = newModel.GetType();
            foreach(var newProp in modelType.GetProperties())
            {
                var attr = GetAttributeForProperty(newProp);
                //Do not allow to change the Primary key
                if (newProp.GetCustomAttribute(typeof(PrimaryKeyAttribute)) == null)
                {
                    var saveAttribute = newProp.GetCustomAttribute<DatabaseSaveAttribute>();
                    if (saveAttribute != null)
                    {
                        newValues.Add(saveAttribute.ColumnName, newProp.GetValue(newModel));
                    }
                }
            }

            if (newValues.Count == 0)
                throw new Exception("No Arguments to change found!");

            var cmdStr = "UPDATE " + tablename + " SET ";
            //TODO: Column setzen ausgehend von den newValues Dict
            foreach (var key in newValues)
            {
                cmdStr += " " + key.Key + "=" + "@" + key.Key + ",";
            }
            cmdStr = cmdStr.Substring(0, cmdStr.Length - 1);
            cmdStr += " WHERE " + primaryKey.propertyInfo.Name + " = @" + primaryKey.propertyInfo.Name + ";";

            var cmd = new MySqlCommand(cmdStr);
            //Insert all the new Values
            foreach(var keyVal in newValues)
            {
                cmd.Parameters.AddWithValue("@" + keyVal.Key, keyVal.Value);
            }
            cmd.Parameters.AddWithValue("@" + primaryKey.Item2.Name, primaryKey.Item2.GetValue(newModel));
            return cmd;         
        }
    }
}
