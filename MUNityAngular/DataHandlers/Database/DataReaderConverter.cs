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

                    var setPropertySuccess = false;

                    if (reader.HasColumn(columnname))
                    {
                        if (propType == typeof(Int16))
                            setPropertySuccess = element.SetProperty(property.Name, reader.GetInt16(columnname));
                        else if (propType == typeof(Int32))
                            setPropertySuccess = element.SetProperty(property.Name, reader.GetInt32(columnname));
                        else if (propType == typeof(Int64))
                            setPropertySuccess = element.SetProperty(property.Name, reader.GetInt64(columnname));
                        else if (propType == typeof(string) || propType == typeof(String))
                        {
                            try
                            {
                                setPropertySuccess = element.SetProperty(property.Name, reader.GetString(columnname));
                            }
                            catch (Exception)
                            {
                                setPropertySuccess = element.SetProperty(property.Name, string.Empty);
                            }

                                
                        }
                            
                        else if (propType == typeof(DateTime))
                            setPropertySuccess = element.SetProperty(property.Name, reader.GetDateTime(columnname));
                        else if (propType == typeof(double))
                            element.SetProperty(property.Name, reader.GetDouble(columnname));
                        else if (propType == typeof(bool))
                            element.SetProperty(property.Name, reader.GetBoolean(columnname));
                        else if (propType == typeof(byte))
                            element.SetProperty(property.Name, reader.GetByte(columnname));
                        else if (propType == typeof(TimeSpan))
                            element.SetProperty(property.Name, reader.GetTimeSpan(columnname));
                    }
                }
            }
            return element;
        }

        public static bool SetProperty(this object o, string name, object value)
        {
            if (o.GetType().GetProperty(name) == null)
                return false;

            if (o.GetType().GetProperty(name).SetMethod == null)
                return false;

            if (o.GetType().GetProperty(name).PropertyType != value.GetType())
                return false;

            o.GetType().GetProperty(name).SetValue(o, value);
            return true;
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
