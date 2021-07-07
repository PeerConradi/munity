using MUNity.Database.Models.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNity.Services
{
    public interface ISimulationService
    {
        public Simulation CreateSimulation(string name);

        Task<Simulation> GetSimulation(int id);


    }
}
