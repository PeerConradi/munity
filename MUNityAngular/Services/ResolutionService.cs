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
using MUNityAngular.DataHandlers;
using MUNityAngular.DataHandlers.EntityFramework;
using MUNityAngular.Models.Core;

namespace MUNityAngular.Services
{
    public class ResolutionService : IDisposable
    {
        private CacheService _cacheService;
        private MunityContext _mariadbcontext;
        private MunCoreContext _coreContext;

        public IHubContext<Hubs.ResolutionHub, Hubs.ITypedResolutionHub> HubContext { get; set; }

        //private List<Models.ResolutionModel> resolutions = new List<Models.ResolutionModel>();

        private readonly IMongoCollection<ResolutionModel> _resolutions;

        public ResolutionModel CreateResolution(bool isPublicReadable = false, bool isPublicWriteable = false, int? userid = null)
        {
            var resolution = new ResolutionModel();
            this._resolutions.InsertOne(resolution);
            //resolutions.Add(resolution);
            //Add To database
            var dbmodel = SaveResolutionInDatabase(resolution, isPublicReadable, isPublicWriteable, userid);
            var cacheModel = new Models.Cache.ResolutionAuthCacheObject(resolution.ID);
            cacheModel.ResolutionModel = resolution;
            cacheModel.DbResolution = dbmodel;
            _cacheService.ResolutionCache.Add(cacheModel);
            //Create file
            return resolution;
        }

        public ResolutionModel CreatePrivateResolution(User owner)
        {
            var resolution = new ResolutionModel();
            this._resolutions.InsertOne(resolution);
            return CreateResolution(userid: owner.UserId);
        }

        public ResolutionModel CreatePublicResulution()
        {
            return CreateResolution(true, true);
        }

        public void DeleteResolution(string id)
        {
            _cacheService.ResolutionCache.Where(n => n.ResolutionId == id).ToList().ForEach(n =>
            {
                _cacheService.ResolutionCache.Remove(n);
            });
            this._resolutions.DeleteOne(n => n.ID == id);
            _mariadbcontext.Resolutions.Remove(_mariadbcontext.Resolutions.FirstOrDefault(n => n.ResolutionId == id));
            _mariadbcontext.SaveChanges();
        }

        public long SavedResolutionsCount { get => this._resolutions.CountDocuments(n => n.ID != null); }

        public long DatabaseResolutionsCount { get => _mariadbcontext.Resolutions.Count(); }

        public void RequestSave(ResolutionModel resolution)
        {
            var info = _mariadbcontext.Resolutions.FirstOrDefault(n => n.ResolutionId == resolution.ID);
            if (info != null)
            {
                if (info.Name != resolution.Topic)
                {
                    info.Name = resolution.Topic;
                }
                info.LastChangedDate = DateTime.Now;
                _mariadbcontext.SaveChanges();

            }
            this._resolutions.ReplaceOneAsync(n => n.ID == resolution.ID, resolution);
        }

        public ResolutionModel GetResolution(string id)
        {
            //Find the Resolution inside the cache
            var cacheObject = _cacheService.ResolutionCache.FirstOrDefault(n => n.ResolutionId == id);
            if (cacheObject != null)
            {
                if (cacheObject.ResolutionModel != null)
                    return cacheObject.ResolutionModel;
            }
            var resolution = this._resolutions.Find(n => n.ID == id).FirstOrDefault();
            //Fix. some loading bugs but maybe it doesnt..
            resolution?.HotFix();
            _cacheService.SetCacheObject(resolution);
            
            return resolution;
        }

        public DataHandlers.EntityFramework.Models.Resolution GetResolutionInfoForPublicId(string publicid)
        {
            var cacheObject = _cacheService.ResolutionCache.FirstOrDefault(n => n.DbResolution != null &&
                n.DbResolution.OnlineCode == publicid);
            if (cacheObject != null)
            {
                return cacheObject.DbResolution;
            }
            var dbMododel = _mariadbcontext.Resolutions.FirstOrDefault(n => n.OnlineCode == publicid);
            if (dbMododel != null)
                _cacheService.SetCacheObject(dbMododel);
            return dbMododel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Touple with infos publicid is null when the resolution can not be found!</returns>
        public DataHandlers.EntityFramework.Models.Resolution GetResolutionDatabaseModel(string id)
        {
            // Cache Handling
            var cacheObject = _cacheService.ResolutionCache.FirstOrDefault(n => n.ResolutionId == id);
            if (cacheObject != null)
            {
                if (cacheObject.DbResolution != null)
                    return cacheObject.DbResolution;
            }
            var dbModel = _mariadbcontext.Resolutions.FirstOrDefault(n => n.ResolutionId == id);
            cacheObject = _cacheService.SetCacheObject(dbModel);
            return cacheObject.DbResolution;
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


            var info = _mariadbcontext.Resolutions.FirstOrDefault(n => n.ResolutionId == resolutionid);
            if (info != null)
            {
                info.PublicRead = true;
                info.OnlineCode = publicid;
                _mariadbcontext.SaveChanges();
            }
            //Suche nach der Resolution in der Datenbank und schaue ob diese bereits eine PublicId hat
            return publicid;
        }

        public string GeneratePublicId()
        {
            var rndm = new Random();
            string id = rndm.Next(10000000, 99999999).ToString();
            bool containsId = false;
            do
            {
                containsId = _mariadbcontext.Resolutions.Any(n => n.OnlineCode == id);
                id = rndm.Next(10000000, 99999999).ToString();
            } while (containsId == true);
            
            return id;
        }

        public string GetPublicIdForResolution(string resolutionid)
        {
            // Cache Handling
            var cacheObject = _cacheService.ResolutionCache.FirstOrDefault(n => n.ResolutionId == resolutionid);
            if (cacheObject != null)
            {
                if (cacheObject.DbResolution != null)
                    return cacheObject.DbResolution.OnlineCode;
            }

            return _mariadbcontext.Resolutions.FirstOrDefault(n => n.ResolutionId == resolutionid)?.OnlineCode ?? null;
        }

        public ResolutionModel GetResolutionFromJson(string json)
        {
            var resolution = Newtonsoft.Json.JsonConvert.DeserializeObject<ResolutionModel>(json);
            return resolution;
        }

        
        private DataHandlers.EntityFramework.Models.Resolution SaveResolutionInDatabase(ResolutionModel resolution, bool pread, bool pwrite, int? userid = null)
        {
            var AdvancedInfo = new DataHandlers.EntityFramework.Models.Resolution();
            if (userid != null)
            {
                AdvancedInfo.CreationUser = _coreContext.Users.FirstOrDefault(n => n.UserId == userid);
            }
            AdvancedInfo.ResolutionId = resolution.ID;
            AdvancedInfo.Name = resolution.Topic;
            AdvancedInfo.PublicRead = pread;
            AdvancedInfo.PublicWrite = pwrite;
            AdvancedInfo.CreationDate = DateTime.Now;
            AdvancedInfo.LastChangedDate = DateTime.Now;
            _mariadbcontext.Resolutions.Add(AdvancedInfo);
            _mariadbcontext.SaveChanges();
            return AdvancedInfo;
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
        public List<DataHandlers.EntityFramework.Models.Resolution> GetResolutionsOfUser(int userid)
        {
            return _mariadbcontext.Resolutions.Where(n => n.CreationUser.UserId == userid).ToList();
        }

        public string SetPublicReadMode(string resolutionid, bool mode)
        {
            if (mode)
                return ActivatePublicReadMode(resolutionid);
            else
            {
                var info = _mariadbcontext.Resolutions.FirstOrDefault(n => n.ResolutionId == resolutionid);
                if (info != null)
                {
                    info.PublicRead = false;
                    // We are not clearing the public Id at this point, this should happen later, so the user doesnt has to 
                    // retype the Code every time it has been disabled for a short time...
                    _mariadbcontext.SaveChanges();
                }
               
                return info.OnlineCode;
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
                var isInDatabase = _mariadbcontext.Resolutions.Any(n => n.ResolutionId == id);
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
        public void RestoreToDatabase(int userid)
        {
            var resolutions = _resolutions.Find(n => n.ID != null).ToList();
            foreach (var resolution in resolutions)
            {
                
                var isInDatabase = _mariadbcontext.Resolutions.Any(n => n.ResolutionId == resolution.ID);
                if (!isInDatabase)
                {
                    SaveResolutionInDatabase(resolution, false, false, userid);
                }
            }
        }

        public List<DataHandlers.EntityFramework.Models.Resolution> GetResolutionsUserCanEdit(int userid)
        {
            var list = new List<DataHandlers.EntityFramework.Models.Resolution>();
            // Every resolution the the User is the owner
            list.AddRange(_mariadbcontext.Resolutions.Where(n => n.CreationUser.UserId == userid));
            // Every resolution where the user is inside the group
            list.AddRange(_mariadbcontext.ResolutionUsers.Where(n => n.User.UserId == userid).Select(n => n.Resolution));
            return list;
        }

        public void GrandUserRights(string resolutionid, int userid, bool canRead, bool canWrite)
        {
            var isAlreadyIn = _mariadbcontext.ResolutionUsers.FirstOrDefault(n => n.User.UserId == userid && n.Resolution.ResolutionId == resolutionid);
            if (isAlreadyIn != null)
            {
                isAlreadyIn.CanEdit = canWrite;
                isAlreadyIn.canRead = canRead;
                _mariadbcontext.SaveChanges();
            }
            else
            {
                var auth = new DataHandlers.EntityFramework.Models.ResolutionUser();
                auth.Resolution = _mariadbcontext.Resolutions.FirstOrDefault(n => n.ResolutionId == resolutionid);
                auth.User = _coreContext.Users.FirstOrDefault(n => n.UserId == userid);
                auth.CanEdit = canWrite;
                auth.canRead = canRead;
                _mariadbcontext.ResolutionUsers.Add(auth);
                _mariadbcontext.SaveChanges();
            }
        }


        public ResolutionService(MunityContext context, MunCoreContext coreContext, IMunityMongoDatabaseSettings mongoSettings, CacheService cacheService)
        {
            _cacheService = cacheService;

            //New Saving in MongoDB
            _mariadbcontext = context;
            _coreContext = coreContext;

            var client = new MongoClient(mongoSettings.ConnectionString);
            var database = client.GetDatabase(mongoSettings.DatabaseName);

            this._resolutions = database.GetCollection<ResolutionModel>(mongoSettings.ResolutionCollectionName);

            Console.WriteLine("Resolution Service Started!");
        }

        
    }

    
}
