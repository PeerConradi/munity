using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MUNityAngular.DataHandlers.Database
{
    public static class DataReaderConverter
    {
        public static T ObjectFromReader<T>(MySqlDataReader reader)
        {
            var type = typeof(T);
            T element = (T)Activator.CreateInstance(typeof(T));
            
            foreach(var property in type.GetProperties())
            {
                var databaseAttribute = (DatabaseSaveAttribute)property.GetCustomAttribute(typeof(DatabaseSaveAttribute));
                if (databaseAttribute != null)
                {
                    var propType = property.PropertyType;
                    var columnname = databaseAttribute.ColumnName;

                    if (reader.HasColumn(columnname))
                    {
                        if (propType == typeof(Int16))
                        {
                            element.GetType().GetProperty(property.Name).SetValue(element, reader.GetInt16(columnname));
                        }
                        else if (propType == typeof(Int32))
                        {
                            element.GetType().GetProperty(property.Name).SetValue(element, reader.GetInt32(columnname));
                        }
                        else if (propType == typeof(Int64))
                        {
                            element.GetType().GetProperty(property.Name).SetValue(element, reader.GetInt64(columnname));
                        }
                        else if (propType == typeof(string) || propType == typeof(String))
                        {
                            element.GetType().GetProperty(property.Name).SetValue(element, reader.GetString(columnname));
                        }
                        else if (propType == typeof(DateTime))
                        {
                            element.GetType().GetProperty(property.Name).SetValue(element, reader.GetDateTime(columnname));
                        }
                    }
                }
            }
            return element;
        }


        public static bool HasColumn(this IDataRecord dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }

    }
}
