using System;
using MongoDB.Driver;
using MUNityAngular.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.DataHandlers;

namespace MUNityAngular.Services
{
    public class PresenceService
    {
        private IMongoCollection<PresenceModel> _presences;

        public void SavePresence(PresenceModel model)
        {
            _presences.InsertOne(model);
        }

        public PresenceModel GetLatestPresence(string committeeid)
        {
            var list = _presences.Find(n => n.CommitteeId == committeeid).ToList();
            if (list.Count == 0)
                return null;
            if (list.Count == 1)
                return list[0];

            return list.Aggregate((n1, n2) => 
                n1.CheckDate > n2.CheckDate ? n1 : n2);
        }

        public PresenceService(IMunityMongoDatabaseSettings mongoSettings)
        {
            var client = new MongoClient(mongoSettings.ConnectionString);
            var database = client.GetDatabase(mongoSettings.DatabaseName);

            this._presences = database.GetCollection<PresenceModel>(mongoSettings.PresenceCollectionName);

        }
    }
}
