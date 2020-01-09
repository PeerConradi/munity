using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Util.Extenstions
{
    public static class ResolutionExtensions
    {
        public static string ToJson(this Models.ResolutionModel resolution)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(resolution, Newtonsoft.Json.Formatting.Indented);
            return json;
        }
    }
}
