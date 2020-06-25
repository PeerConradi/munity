using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace MUNityAngular.Models.Resolution.V2
{
    public interface IResolution
    {

        [BsonId]
        public string ResolutionId { get; set; }

        public ResolutionHeader Header { get; set; }

        public DateTime Date { get; set; }

        public ResolutionPreamble Preamble { get; set; }

        public OperativeSection OperativeSection { get; set; }

    }
}
