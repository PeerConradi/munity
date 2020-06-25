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
    public class ResolutionService
    {
        private CacheService _cacheService;
        private MunityContext _mariadbcontext;
        private MunCoreContext _coreContext;

        public IHubContext<Hubs.ResolutionHub, Hubs.ITypedResolutionHub> HubContext { get; set; }

        //private List<Models.ResolutionModel> resolutions = new List<Models.ResolutionModel>();

        private readonly IMongoCollection<ResolutionModel> _resolutions;



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
