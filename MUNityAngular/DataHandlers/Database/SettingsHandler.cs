using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.DataHandlers.Database
{
    public static class SettingsHandler
    {
        public static string GetResolutionDir
        {
            get
            {
                using (var connection = Connector.Connection)
                {
                    var cmdStr = "SELECT value FROM settings WHERE name='RESOLUTION_PATH';";
                    connection.Open();
                    var cmd = new MySqlCommand(cmdStr, connection);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return reader.GetString(0);
                        }
                    }
                }
                throw new Exception("Resolution Path not found");
            }
        }
    }
}
