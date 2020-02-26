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
        private string _mySqlConnectionString;
        private const string resolution_table_name = "resolution";

        public IHubContext<Hubs.ResolutionHub, Hubs.ITypedResolutionHub> HubContext { get; set; }

        //private List<Models.ResolutionModel> resolutions = new List<Models.ResolutionModel>();

        private readonly IMongoCollection<ResolutionModel> _resolutions;

        public ResolutionModel CreateResolution(bool isPublicReadable = false, bool isPublicWriteable = false, string userid = null)
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
            Tools.Connection(_mySqlConnectionString).Table(resolution_table_name).RemoveEntry("id", id);
        }

        public long SavedResolutionsCount { get => this._resolutions.CountDocuments(n => n.ID != null); }

        public long DatabaseResolutionsCount { get => Tools.Connection(_mySqlConnectionString).Table(resolution_table_name).Count(); }

        public void RequestSave(ResolutionModel resolution)
        {
            Task.Run(() =>
            {
                //An dieser Stelle könnte man auch einen Replace Into verwenden, aber ich bin mir zu 100% sicher, dass
                //diese Funktion deutlich schneller ist!

                var info = GetResolutionInfoForId(resolution.ID);
                if (info.Name != resolution.Topic)
                {
                    Tools.Connection(_mySqlConnectionString).Resolution.SetEntry("id", resolution.ID, "name", resolution.Topic);
                }
                Tools.Connection(_mySqlConnectionString).Resolution.SetEntry("id", resolution.ID, "lastchangeddate", DateTime.Now);

                
            });
            this._resolutions.ReplaceOneAsync(n => n.ID == resolution.ID, resolution);
        }

        public ResolutionModel GetResolution(string id)
        {
            var resolution = this._resolutions.Find(n => n.ID == id).FirstOrDefault();

            //Fix. some loading bugs but maybe it doesnt...
            if (resolution != null)
            {
                if (resolution.OperativeSections != null)
                {
                    foreach (var oa in resolution.OperativeSections)
                    {
                        oa.Resolution = resolution;
                    }
                }
                
                if (resolution.Preamble != null && resolution.Preamble.Paragraphs != null)
                {
                    foreach (var ps in resolution.Preamble.Paragraphs)
                    {
                        ps.Preamble = resolution.Preamble;
                    }
                }
            }
            
            
            return resolution;
        }

        public ResolutionAdvancedInfoModel GetResolutionInfoForPublicId(string publicid)
        {
            return Tools.Connection(_mySqlConnectionString).Table(resolution_table_name).First<ResolutionAdvancedInfoModel>("onlinecode", publicid);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Touple with infos publicid is null when the resolution can not be found!</returns>
        public ResolutionAdvancedInfoModel GetResolutionInfoForId(string id)
        {
            return Tools.Connection(_mySqlConnectionString).Table(resolution_table_name).First<ResolutionAdvancedInfoModel>("id", id);
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

            if (string.IsNullOrWhiteSpace(publicid))
            {
                publicid = GeneratePublicId();
            }

            Tools.Connection(_mySqlConnectionString).Table(resolution_table_name).SetEntry("id", resolutionid, "ispublicread", true);
            Tools.Connection(_mySqlConnectionString).Table(resolution_table_name).SetEntry("id", resolutionid, "onlinecode", publicid);
            //Suche nach der Resolution in der Datenbank und schaue ob diese bereits eine PublicId hat
            return publicid;
        }

        public string GeneratePublicId()
        {
            string id = new Random().Next(10000000, 99999999).ToString();
            bool containsId = false;
            do
            {
                using (var connection = new MySqlConnection(_mySqlConnectionString))
                {
                    connection.Open();
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
            return Tools.Connection(_mySqlConnectionString).Resolution.First<ResolutionAdvancedInfoModel>("id", resolutionid)?.OnlineCode ?? null;
        }

        public ResolutionModel GetResolutionFromJson(string json)
        {
            var resolution = Newtonsoft.Json.JsonConvert.DeserializeObject<ResolutionModel>(json);
            return resolution;
        }

        
        private bool SaveResolutionInDatabase(ResolutionModel resolution, bool pread, bool pwrite, string userid = null)
        {
            var AdvancedInfo = new ResolutionAdvancedInfoModel(resolution, userid);
            AdvancedInfo.Name = resolution.Topic;
            AdvancedInfo.PublicRead = pread;
            AdvancedInfo.PublicWrite = pwrite;
            Tools.Connection(_mySqlConnectionString).Table(resolution_table_name).Insert(AdvancedInfo);
            return true;
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
        public List<ResolutionAdvancedInfoModel> GetResolutionsOfUser(string userid)
        {
            return Tools.Connection(_mySqlConnectionString).Table(resolution_table_name).GetElements<ResolutionAdvancedInfoModel>("user", userid);
        }

        public void UpdateResolutionName(string resolutionid, string newtitle)
        {
            Tools.Connection(_mySqlConnectionString).Table(resolution_table_name).SetEntry("id", resolutionid, "name", newtitle);
        }

        

        public string SetPublicReadMode(string resolutionid, bool mode)
        {
            if (mode)
                return ActivatePublicReadMode(resolutionid);
            else
            {
                Tools.Connection(_mySqlConnectionString).Table(resolution_table_name).SetEntry("id", resolutionid, "ispublicread", false);
                return Tools.Connection(_mySqlConnectionString).Resolution.First<ResolutionAdvancedInfoModel>("id", resolutionid).OnlineCode;
            }
            
        }

        /// <summary>
        /// Deletes all MongoDB Entries where no Resolution was found inside the Database.
        /// </summary>
        public void PurgeMongoDB()
        {
            var ids = _resolutions.Find(n => n.ID != null).ToList().Select(n => n.ID);
            foreach(var id in ids)
            {
                var isInDatabase = Tools.Connection(_mySqlConnectionString).Resolution.HasEntry("id", id);
                if (!isInDatabase)
                {
                    _resolutions.DeleteOne(n => n.ID == id);
                }
            }
        }

        /// <summary>
        /// Serches for Resolutions that are inside MongoDB but not the SQL Database and creates elements
        /// for this resolutions inside the Database
        /// </summary>
        /// <param name="userid">You have to pass a user that should now own this documents</param>
        public void RestoreToDatabase(string userid)
        {
            var resolutions = _resolutions.Find(n => n.ID != null).ToList();
            foreach (var resolution in resolutions)
            {
                
                var isInDatabase = Tools.Connection(_mySqlConnectionString).Resolution.HasEntry("id", resolution.ID);
                if (!isInDatabase)
                {
                    SaveResolutionInDatabase(resolution, false, false, userid);
                }
            }
        }

        public List<ResolutionAdvancedInfoModel> GetResolutionsUserCanEdit(string userid)
        {
            var list = new List<ResolutionAdvancedInfoModel>();
            using (var connection = new MySqlConnection(_mySqlConnectionString))
            {
                connection.Open();
                var cmdStr = "SELECT resolution.* FROM resolution " +
                    "LEFT JOIN resolution_auth ON resolution_auth.resolutionid = resolution.id " +
                    "WHERE resolution.`user` = @userid OR resolution_auth.userid = @userid " +
                    "AND resolution_auth.canwrite = 1";
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

        public void GrandUserRights(string resolutionid, string userid, bool canRead, bool canWrite)
        {
            var cmdStr = "REPLACE INTO resolution_auth (resolutionid, userid, canread, canwrite) VALUES (@resoid, @userid, @canread, @canwrite);";
            using (var connection = new MySqlConnection(_mySqlConnectionString))
            {
                connection.Open();
                var cmd = new MySqlCommand(cmdStr, connection);
                cmd.Parameters.AddWithValue("@resoid", resolutionid);
                cmd.Parameters.AddWithValue("@userid", userid);
                cmd.Parameters.AddWithValue("@canread", canRead);
                cmd.Parameters.AddWithValue("@canwrite", canWrite);
                cmd.ExecuteNonQuery();
            }
        }

        public ResolutionService(string mysqlConnectionString, string mongoConnectionString, string mongoDatabaseName)
        {
            //New Saving in MongoDB
            _mySqlConnectionString = mysqlConnectionString;

            var resolutionTableString = "Resolutions";
            var client = new MongoClient(mongoConnectionString);
            var database = client.GetDatabase(mongoDatabaseName);

            this._resolutions = database.GetCollection<ResolutionModel>(resolutionTableString);

            Console.WriteLine("Resolution Service Started!");
        }
    }

    
}
