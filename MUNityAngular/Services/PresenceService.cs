using System;
using MongoDB.Driver;
using MUNityAngular.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Services
{
    public class PresenceService
    {
        string _mySqlConnectionString;
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

        public PresenceService(string mysqlConnectionString, string mongoConnectionString, string mongoDatabaseName)
        {
            _mySqlConnectionString = mysqlConnectionString;

            var resolutionTableString = "Presence";
            var client = new MongoClient(mongoConnectionString);
            var database = client.GetDatabase(mongoDatabaseName);

            this._presences = database.GetCollection<PresenceModel>(resolutionTableString);

        }
    }
}
