using MUNityAngular.Models.Resolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.DataHandlers.EntityFramework.Models;

namespace MUNityAngular.Models.Cache
{
    public class ResolutionAuthCacheObject
    {
        public string ResolutionId { get; set; }

        public ResolutionModel ResolutionModel { get; set; }


        public List<string> AllowedAuths { get; set; }

        public List<int> AllowedUserIds { get; set; }

        public ResolutionAuthCacheObject(string resolutionid)
        {
            ResolutionId = resolutionid;
            AllowedAuths = new List<string>();
            AllowedUserIds = new List<int>();
        }
    }
}
