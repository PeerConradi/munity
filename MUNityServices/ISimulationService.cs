using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityCore.Models.Simulation;

namespace MUNityCore.Services
{
    public interface ISimulationService
    {
        public Simulation CreateSimulation(string name);

        Task<Simulation> GetSimulation(int id);


    }
}
