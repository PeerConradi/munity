using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Simulation
{
    public class UsersChangedEventArgs : EventArgs
    {
        public int SimulationId { get; set; }

        public List<SimulationUserItem> Users { get; set; }
    }
}
