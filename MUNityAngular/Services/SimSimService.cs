using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.Models.SimSim;

namespace MUNityAngular.Services
{

    /// <summary>
    /// A Service to create Simulations of a single committee with some Delegations inside
    /// </summary>
    public class SimSimService
    {
        private List<SimSimModel> _sims;

        public SimSimModel CreateSimSim()
        {
            var sim = new SimSimModel();
            sim.SimSimId = new Random().Next(100000, 999999);
            return sim;
        }

        public SimSimModel GetSimSim(int id)
        {
            return _sims.FirstOrDefault(n => n.SimSimId == id);
        }
    }
}
