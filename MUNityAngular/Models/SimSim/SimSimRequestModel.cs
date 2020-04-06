using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models.SimSim
{
    public class SimSimRequestModel
    {
        public int SimSimRequestModelId { get; set; }

        public string DelegationId { get; set; }

        public string RequestType { get; set; }

        public DateTime RequestTime { get; set; }
    }
}
