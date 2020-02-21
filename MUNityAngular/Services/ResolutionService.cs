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
            Tools.Connection(_mySqlConnectionString).Table(resolution_table_name).RemoveEntry("id", id);
        }

        public long SavedResolutionsCount { get => this._resolutions.CountDocuments(n => n.ID != null); }

        public long DatabaseResolutionsCount { get => Tools.Connection(_mySqlConnectionString).Table(resolution_table_name).Count(); }

        public void RequestSave(ResolutionModel resolution)
        {
            this._resolutions.ReplaceOne(n => n.ID == resolution.ID, resolution);
        }

        public ResolutionModel GetResolution(string id)
        {
            var resolution = this._resolutions.Find(n => n.ID == id).FirstOrDefault();
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
            ResolutionAdvancedInfoModel model = null;
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
            return Tools.Connection(_mySqlConnectionString).Table(resolutionid).First<ResolutionAdvancedInfoModel>("id", resolutionid)?.OnlineCode ?? null;
        }

        public ResolutionModel GetResolutionFromJson(string json)
        {
            var resolution = Newtonsoft.Json.JsonConvert.DeserializeObject<ResolutionModel>(json);
            return resolution;
        }

        
        private bool SaveResolutionInDatabase(ResolutionModel resolution, bool pread, bool pwrite, string userid = null)
        {
            var AdvancedInfo = new ResolutionAdvancedInfoModel(resolution, userid);
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
        internal List<ResolutionAdvancedInfoModel> GetResolutionsOfUser(string userid)
        {
            return Tools.Connection(_mySqlConnectionString).Table(resolution_table_name).GetElements<ResolutionAdvancedInfoModel>("userid", userid);
        }

        internal void UpdateResolutionName(string resolutionid, string newtitle)
        {
            Tools.Connection(_mySqlConnectionString).Table(resolution_table_name).SetEntry("id", resolutionid, "name", newtitle);
        }

        

        internal void SetPublicReadMode(string resolutionid, bool mode)
        {
            Tools.Connection(_mySqlConnectionString).Table(resolution_table_name).SetEntry("id", resolutionid, "ispublicread", mode);
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
