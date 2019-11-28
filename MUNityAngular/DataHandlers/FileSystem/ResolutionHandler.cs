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
            return resolution;
        }

        public static Models.ResolutionModel ExampleResolution
        {
            get
            {
                var resolution = new Models.ResolutionModel();
                resolution.Name = "Example";
                resolution.FullName = "Example Resolution";
                resolution.Topic = "Example";
                resolution.Preamble.AddParagraph("Präambel Paragraph 1");
                resolution.Preamble.AddParagraph("Präambel Paragraph 2");
                var secionOne = resolution.AddOperativeParagraph("operative Paragraph 1");
                resolution.AddOperativeParagraph("operative Parahraph 2");
                var changeAmendment = new Models.ChangeAmendmentModel();
                changeAmendment.TargetSection = secionOne;
                changeAmendment.NewText = "Das ist der neue Text";
                return resolution;
            }
        }
    }
}
