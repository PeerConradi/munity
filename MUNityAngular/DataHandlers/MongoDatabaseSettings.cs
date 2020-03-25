using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.DataHandlers
{
    public class MunityMongoDatabaseSettings : IMunityMongoDatabaseSettings
    {
        public string ResolutionCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }

        public string PresenceCollectionName { get; set; }
    }

    public interface IMunityMongoDatabaseSettings
    {
        string ResolutionCollectionName { get; set; }
        string ConnectionString { get; set; }

        string DatabaseName { get; set; }

        string PresenceCollectionName { get; set; }
    }
}
