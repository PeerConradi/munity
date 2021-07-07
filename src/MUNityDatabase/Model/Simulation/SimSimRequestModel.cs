using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNity.Database.Models.Simulation
{
    public class SimSimRequestModel
    {
        public enum RequestTypes
        {
            Custom,
            AddToListOfSpeakers,
            AddToQuestions,
            Personal,
            StandingOrders
        }

        public int SimSimRequestModelId { get; set; }

        public string UserToken { get; set; }

        public RequestTypes RequestType { get; set; }

        public string Message { get; set; }

        public DateTime RequestTime { get; set; }

        public SimSimRequestModel()
        {
            SimSimRequestModelId = new Random().Next(0, 100000000);
        }
    }
}
