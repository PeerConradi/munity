using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.DataHandlers.Database
{
    public class DatabaseInformation
    {
        public enum ETableStatus
        {
            Ready,
            NotExisting,
            Different
        }

        public static string GetDatabaseType(Type type)
        {
            if (type == typeof(string)) return "TEXT";
            if (type == typeof(int) || type == typeof(Int16) || type == typeof(Int32)) return "INT";
            if (type == typeof(Int64)) return "BIGINT";
            if (type == typeof(DateTime)) return "DATETIME";

            return string.Empty;
        }

        public static string GetDatabaseType(DatabaseSaveAttribute.EFieldType type)
        {
            switch (type)
            {
                case DatabaseSaveAttribute.EFieldType.AUTO:
                    throw new ArgumentException("AUTO is not a valid type for this function!");
                case DatabaseSaveAttribute.EFieldType.VARCHAR:
                    return "VARCHAR(255)";
                case DatabaseSaveAttribute.EFieldType.TEXT:
                    return "TEXT";
                case DatabaseSaveAttribute.EFieldType.INT:
                    return "INT";
                case DatabaseSaveAttribute.EFieldType.TINYINT:
                    return "TINYINT";
                case DatabaseSaveAttribute.EFieldType.BIGINT:
                    return "BIGINT";
                default:
                    throw new ArgumentException("UNKNOWN/UNHANDLED Type");
            }
        }
    }
}
