using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.Util.Extenstions;
using System.Threading;
using Microsoft.AspNetCore.SignalR;
using MUNityAngular.DataHandlers.Database;
using MongoDB.Driver;
using MUNityAngular.Models.Resolution;

namespace MUNityAngular.Services
{
    public class ResolutionService : IDisposable
    {
        private const string resolution_table_name = "resolution";

        public IHubContext<Hubs.ResolutionHub, Hubs.ITypedResolutionHub> HubContext { get; set; }

        //private List<Models.ResolutionModel> resolutions = new List<Models.ResolutionModel>();

        private readonly IMongoCollection<ResolutionModel> _resolutions;

        public ResolutionModel CreateResolution(bool isPublicReadable = false, bool isPublicWriteable = false, string userid = "anon")
        {
            var resolution = new ResolutionModel();
            this._resolutions.InsertOne(resolution);
            //resolutions.Add(resolution);
            //Add To database
            SaveResolutionInDatabase(resolution, isPublicReadable, isPublicWriteable, userid);

            //Create file
            return resolution;
        }

        public void DeleteResolution(string id)
        {
            this._resolutions.DeleteOne(n => n.ID == id);
            using (var connection = Connector.Connection)
            {
                connection.Open();
                var cmdStr = "DELETE FROM resolution WHERE id=@id";
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        public long SavedResolutionsCount { get => this._resolutions.CountDocuments(n => n.ID != null); }

        public long DatabaseResolutionsCount
        {
            get
            {
                long count = 0;
                using (var connection = Connector.Connection)
                {
                    connection.Open();
                    var cmdStr = "SELECT COUNT(*) FROM resolution;";
                    var cmd = new MySqlCommand(cmdStr, connection);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                count = reader.GetInt64(0);
                            }
                        }
                    }
                }
                return count;
            }
        }

        public void RequestSave(ResolutionModel resolution)
        {
            
            this._resolutions.ReplaceOne(n => n.ID == resolution.ID, resolution);
        }

        public ResolutionModel GetResolution(string id)
        {
            var resolution = this._resolutions.Find(n => n.ID == id).FirstOrDefault();
            return resolution;
        }

        public (string id, bool publicRead, bool publicWrite, bool publicAmendment) GetResolutionInfoForPublicId(string publicid)
        {
            string id = string.Empty;
            bool isPublicRead = false;
            bool isPublicWrite = false;
            bool isOnlineAmendment = false;
            using (var connection = Connector.Connection)
            {
                var cmdStr = "SELECT * FROM resolution WHERE onlinecode=@onlineid";
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@onlineid", publicid);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        id = reader.GetString("id");
                        isPublicRead = reader.GetBoolean("ispublicread");
                        isPublicWrite = reader.GetBoolean("ispublicwrite");
                        isOnlineAmendment = reader.GetBoolean("allowamendments");
                    }
                }
            }

            return (id, isPublicRead, isPublicWrite, isOnlineAmendment);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Touple with infos publicid is null when the resolution can not be found!</returns>
        public ResolutionAdvancedInfoModel GetResolutionInfoForId(string id)
        {
            ResolutionAdvancedInfoModel model = null;
            using (var connection = Connector.Connection)
            {
                var cmdStr = "SELECT * FROM resolution WHERE id=@id";
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@id", id);
                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        model = null;
                    }
                    else
                    {
                        while (reader.Read())
                        {
                            model = new ResolutionAdvancedInfoModel();
                            model.ID = id;
                            model.Name = reader.GetString("name");
                            model.OnlineCode = reader.GetString("onlinecode");
                            model.CreationDate = reader.GetDateTime("creationdate");
                            model.LastChangedDate = reader.GetDateTime("lastchangeddate");
                            model.PublicRead = reader.GetBoolean("ispublicread");
                            model.PublicWrite = reader.GetBoolean("ispublicwrite");
                            model.PublicAmendment = reader.GetBoolean("allowamendments");
                        }
                    }
                }
            }

            return model;
        }

        /// <summary>
        /// Activates the Public-Read-Mode and returns the Public Id of
        /// the resolution.
        /// </summary>
        /// <param name="resolutionid"></param>
        /// <returns></returns>
        public string ActivatePublicReadMode(string resolutionid)
        {
            string publicid = GetPublicIdForResolution(resolutionid);

            //Suche nach der Resolution in der Datenbank und schaue ob diese bereits eine PublicId hat
            using (var connection = Connector.Connection)
            {
                connection.Open();
                
                if (string.IsNullOrWhiteSpace(publicid))
                {
                    publicid = GeneratePublicId();
                }
                //Updaten der Resolution:
                var updateCommandStr = "UPDATE resolution SET onlinecode=@onlinecode, ispublicread=1";
                var updateCommand = new MySqlCommand(updateCommandStr, connection);
                updateCommand.Parameters.AddWithValue("@onlinecode", publicid);
                updateCommand.ExecuteNonQuery();
            }
            return publicid;
        }

        public string GeneratePublicId()
        {
            string id = new Random().Next(10000000, 99999999).ToString();
            bool containsId = false;
            do
            {
                using (var connection = Connector.Connection)
                {
                    var cmdStr = "SELECT COUNT(*) FROM resolution WHERE onlinecode=@code";
                    var cmd = new MySqlCommand(cmdStr, connection);
                    cmd.Parameters.AddWithValue("@code", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            containsId = (reader.GetInt16(0) > 0);
                        }
                    }
                }
            } while (containsId == true);
            
            return id;
        }

        public string GetPublicIdForResolution(string resolutionid)
        {
            string publicid = null;
            using (var connection = Connector.Connection)
            {
                connection.Open();
                var getOnlineIdCommand = "SELECT onlinecode FROM resolution WHERE id=@resoid";
                var cmd = new MySqlCommand(getOnlineIdCommand, connection);
                cmd.Parameters.AddWithValue("@resoid", resolutionid);
                using (var idreader = cmd.ExecuteReader())
                {
                    while (idreader.Read())
                    {
                        publicid = idreader.GetString(0);
                    }
                }
            }
            return publicid;
        }

        public ResolutionModel GetResolutionFromJson(string json)
        {
            var resolution = Newtonsoft.Json.JsonConvert.DeserializeObject<ResolutionModel>(json);
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

        
        private bool SaveResolutionInDatabase(ResolutionModel resolution, bool pread, bool pwrite, string userid = "anon")
        {
            using (var connection = Connector.Connection)
            {
                var cmdStr = "INSERT INTO "+ resolution_table_name + " (id, name, user, creationdate, lastchangeddate, onlinecode,ispublicread,ispublicwrite) VALUES (" +
                    "@id, @name, @user, @creationdate, @lastchangeddate, @onlinecode, @pread, @pwrite);";
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@id", resolution.ID);
                cmd.Parameters.AddWithValue("@name", resolution.Topic);
                cmd.Parameters.AddWithValue("@user", userid);
                cmd.Parameters.AddWithValue("@creationdate", DateTime.Now);
                cmd.Parameters.AddWithValue("@lastchangeddate", DateTime.Now);
                cmd.Parameters.AddWithValue("@onlinecode", new Random().Next(10000000, 99999999));
                cmd.Parameters.AddWithValue("@pread", pread);
                cmd.Parameters.AddWithValue("@pwrite", pwrite);
                cmd.ExecuteNonQuery();
                return true;
            }
        }

        public void Dispose()
        {
            //Nothing to Dispose()
        }

        

        /// <summary>
        /// Returns a List of resolutions where the User is the creator/owner
        /// </summary>
        /// <param name="userid"></param>
        /// <returns>a touple with the first value as the id, and the second value is the name</returns>
        internal List<ResolutionAdvancedInfoModel> GetResolutionsOfUser(string userid)
        {
            var cmdStr = "SELECT * FROM " + resolution_table_name + " WHERE user=@userid";
            var list = new List<ResolutionAdvancedInfoModel>();
            using (var connection = Connector.Connection)
            {
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@userid", userid);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(DataReaderConverter.ObjectFromReader<ResolutionAdvancedInfoModel>(reader));
                    }
                }
            }
            return list;
        }

        internal void UpdateResolutionName(string resolutionid, string newtitle)
        {
            var cmdStr = "UPDATE " + resolution_table_name + " SET name = @newname WHERE id=@id";
            using (var connection = Connector.Connection)
            {
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@newname", newtitle);
                cmd.Parameters.AddWithValue("@id", resolutionid);
                cmd.ExecuteNonQuery();
            }
        }

        public ResolutionService(string connectionString, string databaseString)
        {
            //New Saving in MongoDB
            var resolutionTableString = "Resolutions";
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseString);

            this._resolutions = database.GetCollection<ResolutionModel>(resolutionTableString);

            Console.WriteLine("Resolution Service Started!");
        }

        internal void SetPublicReadMode(string resolutionid, bool mode)
        {
            using (var connection = Connector.Connection)
            {
                var cmdStr = "UPDATE resolution SET ispublicread=@mode WHERE id=@resoid;";
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@mode", mode);
                cmd.Parameters.AddWithValue("@resoid", resolutionid);
                cmd.ExecuteNonQuery();
            }
        }
    }

    
}
