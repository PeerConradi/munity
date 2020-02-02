using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.DataHandlers.Database
{
    public static class SettingsHandler
    {
        private static string _resoltutiondir = null;

        public static string GetResolutionDir
        {
            get
            {

                if (_resoltutiondir == null)
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
                                _resoltutiondir =  reader.GetString(0);
                                Console.WriteLine("Set Resolution Save directory to: " + _resoltutiondir);
                            }
                        }
                    }
                }

                if (_resoltutiondir != null)
                {
                    return _resoltutiondir;
                }
                
                throw new Exception("Resolution Path not found");
            }
        }
    }
}
