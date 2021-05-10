using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Models.Simulation
{
    public class SimulationPresents
    {
        public int SimulationPresentsId { get; set; }

        public Simulation Simulation { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime? CheckedDate { get; set; } = null;

        public List<PresentsState> CheckedUsers { get; set; }

        public bool MarkedFinished { get; set; }

        public SimulationPresents()
        {
            CreatedTime = DateTime.Now;
        }
    }

}
