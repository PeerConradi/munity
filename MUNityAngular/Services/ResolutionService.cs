using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Services
{
    public class ResolutionService
    {
        private List<Models.ResolutionModel> resolutions = new List<Models.ResolutionModel>();

        public Models.ResolutionModel CreateResolution()
        {
            var resolution = new Models.ResolutionModel();
            resolutions.Add(resolution);
            //Add To database
            //Create file
            return resolution;
        }

        public string Save(Models.ResolutionModel resolution)
        {
            var resolutionDirectory = DataHandlers.Database.SettingsHandler.GetResolutionDir;
            var filePath = System.IO.Path.Combine(resolutionDirectory, resolution.ID + ".json");

            if (!System.IO.File.Exists(filePath))
            {
                var stream = System.IO.File.Create(filePath);
                stream.Close();
            }
            System.IO.File.WriteAllText(filePath, resolution.ToJson());
            return filePath;
        }

        public Models.ResolutionModel GetResolution(string id)
        {
            var resolution = resolutions.FirstOrDefault(n => n.ID == id);
            if (resolution == null)
            {
                var resolutionDirectory = DataHandlers.Database.SettingsHandler.GetResolutionDir;
                var filePath = System.IO.Path.Combine(resolutionDirectory, id + ".json");
                if (System.IO.File.Exists(filePath))
                {
                    resolution = GetResolutionFromJson(System.IO.File.ReadAllText(filePath));
                    resolutions.Add(resolution);
                }
                else
                {
                    return null;
                }
            }
            return resolution;
        }

        public Models.ResolutionModel GetResolutionFromJson(string json)
        {
            var resolution = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.ResolutionModel>(json);
            return resolution;
        }

        public string GetResolutionJsonFromFile(string id)
        {
            var resolutionDirectory = DataHandlers.Database.SettingsHandler.GetResolutionDir;
            var filePath = System.IO.Path.Combine(resolutionDirectory, id + ".json");
            if (System.IO.File.Exists(filePath))
            {
                return System.IO.File.ReadAllText(filePath);
            }
            else
            {
                return null;
            }
        }

        public bool SaveResolutionInDatabase(Models.ResolutionModel resolution)
        {
            using (var connection = DataHandlers.Database.Connector.Connection)
            {
                var cmdStr = "INSERT INTO resolution (id, name, user, creationdate, lastchangeddate, onlinecode) VALUES (" +
                    "@id, @name, @user, @creationdate, @lastchangeddate, @onlinecode);";
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@id", resolution.ID);
                cmd.Parameters.AddWithValue("@name", resolution.Topic);
                cmd.Parameters.AddWithValue("@user", "anon");
                cmd.Parameters.AddWithValue("@creationdate", DateTime.Now);
                cmd.Parameters.AddWithValue("@lastchangeddate", DateTime.Now);
                cmd.Parameters.AddWithValue("@onlinecode", new Random().Next(10000000, 99999999));
                cmd.ExecuteNonQuery();
                return true;
            }
        }
    }

    public static class ResolutionExtensions
    {
        public static string ToJson(this Models.ResolutionModel resolution)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(resolution, Newtonsoft.Json.Formatting.Indented);
            return json;
        }
    }
}
