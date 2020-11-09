using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Models.Simulation
{
    public class SimSimRequestModel
    {
        public int SimSimRequestModelId { get; set; }

        public string UserToken { get; set; }

        public string RequestType { get; set; }

        public string Message { get; set; }

        public DateTime RequestTime { get; set; }

        public SimSimRequestModel()
        {
            SimSimRequestModelId = new Random().Next(0, 100000000);
        }
    }
}
