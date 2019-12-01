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
    }
}
