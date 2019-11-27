using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.DataHandlers.FileSystem
{
    public class ResolutionHandler
    {
        public static string SaveResolutionToLocalFile(Models.ResolutionModel resolution)
        {
            return string.Empty;
        }

        public static string GetJsonFromResolution(Models.ResolutionModel resolution)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(resolution, Newtonsoft.Json.Formatting.Indented);
            return json;
        }

        public static Models.ResolutionModel GetResolutionFromJson(string json)
        {
            var resolution = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.ResolutionModel>(json);
            resolution.Autofix();
            return resolution;
        }
    }
}
