using MUNityAngular.Models.Resolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.DataHandlers.EntityFramework.Models;

namespace MUNityAngular.Models.Cache
{
    public class ResolutionCacheObject
    {
        public ResolutionModel Resolution;

        public List<string> AllowedAuths { get; set; }

        public List<int> AllowedUserIds { get; set; }
    }
}
