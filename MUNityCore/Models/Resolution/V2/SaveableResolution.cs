using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MUNityCore.Models.Resolution.V2
{
    public class SaveableResolution : MUNity.Models.Resolution.Resolution
    {
        [BsonId]
        public string _id { get; set; }

        public SaveableResolution(MUNity.Models.Resolution.Resolution resolution)
        {
            Date = resolution.Date;
            Header = resolution.Header;
            OperativeSection = resolution.OperativeSection;
            Preamble = resolution.Preamble;
            _id = resolution.ResolutionId;
        }
    }
}
