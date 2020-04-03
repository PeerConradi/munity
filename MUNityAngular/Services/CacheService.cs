using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.Models.Cache;

namespace MUNityAngular.Services
{
    public class CacheService
    {
        public List<ResolutionAuthCacheObject> ResolutionCache = new List<Models.Cache.ResolutionAuthCacheObject>();

        public ResolutionAuthCacheObject AddCacheObject(string id)
        {
            if (!ResolutionCache.Any(n => n.ResolutionId == id))
            {
                var model = new ResolutionAuthCacheObject(id);
                ResolutionCache.Add(model);
                return model;
            }
            return null;
        }

        public ResolutionAuthCacheObject AddCacheObject(Models.Resolution.ResolutionModel model)
        {
            var mdl = AddCacheObject(model.ID);
            mdl.ResolutionModel = model;
            return mdl;
        }

        public ResolutionAuthCacheObject AddCacheObject(DataHandlers.EntityFramework.Models.Resolution model)
        {
            var mdl = AddCacheObject(model.ResolutionId);
            mdl.DbResolution = model;
            return mdl;
        }

        public ResolutionAuthCacheObject SetCacheObject(Models.Resolution.ResolutionModel model)
        {
            if (model == null)
                return null;
            var current = ResolutionCache.FirstOrDefault(n => n.ResolutionId == model.ID);
            if (current == null)
            {
                current = AddCacheObject(model);
            }
            if (current != null)
            {
                current.ResolutionModel = model;
            }
            return current;
        }

        public ResolutionAuthCacheObject SetCacheObject(DataHandlers.EntityFramework.Models.Resolution model)
        {
            if (model == null)
                return null;

            var current = ResolutionCache.FirstOrDefault(n => n.ResolutionId == model.ResolutionId);
            if (current == null)
            {
                current = AddCacheObject(model);
            }

            if (current != null)
            {
                current.DbResolution = model;
            }
            return current;
        }


    }
}
