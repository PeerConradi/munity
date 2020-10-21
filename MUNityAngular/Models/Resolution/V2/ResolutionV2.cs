using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace MUNityCore.Models.Resolution.V2
{
    public class ResolutionV2 : IResolution
    {

        [BsonId]
        public string ResolutionId { get; set; }

        public DateTime Date { get; set; }

        public ResolutionHeader Header { get; set; }

        public ResolutionPreamble Preamble { get; set; }
        public OperativeSection OperativeSection { get; set; }

        public ResolutionV2()
        {
            ResolutionId = Util.Tools.IdGenerator.RandomString(36);
            Preamble = new ResolutionPreamble();
            OperativeSection = new OperativeSection();
            Header = new ResolutionHeader();
        }
    }
}
