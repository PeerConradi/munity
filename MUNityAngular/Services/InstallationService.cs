using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.DataHandlers.Database;
using MySql.Data.MySqlClient;

namespace MUNityAngular.Services
{
    public class InstallationService
    {
        public List<string> InstallationLog { get; private set; }

        public InstallationService()
        {
            InstallationLog = new List<string>();
            IsInstalled = Connector.DoesDatabaseExist();
        }

        public bool IsInstalled { get; private set; }

        public Version InstalledVersion
        {
            get => throw new NotImplementedException();
        }

        public void CreateDatabaseAndTables()
        {
            //Connector.EnsureDatabaseExists();
            string filePath = System.IO.Path.Combine(Environment.CurrentDirectory, "/Util/Files/database.sql");
            if (System.IO.File.Exists(filePath))
            {
                var cmdStr = System.IO.File.ReadAllText(filePath);
                using (var connection = Connector.Connection)
                {
                    connection.Open();
                    var cmd = new MySqlCommand(cmdStr, connection);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            InstallationLog.Add(reader.ToString());
                        }
                    }
                }
            }
            else
            {
                throw new Exception("DatabaseFile not found!");
            }
        }

        public void Install()
        {
            if (!Connector.DoesDatabaseExist())
            {
                InstallationLog.Add("Database not found!");
                CreateDatabaseAndTables();
            }

            IsInstalled = true;
        }
    }
}
