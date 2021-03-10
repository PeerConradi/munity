using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityClient.ViewModels.Simulation
{
    public class SimulationSlotsViewModel
    {
        public string SlotId { get; set; }

        private string _displayName = "";
        public string DisplayName 
        {
            get => _displayName;
            set
            {
                if (value == _displayName)
                    return;
                _displayName = value;
            }
        }

        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public Task Remove()
        {
            throw new NotImplementedException();
        }
    }
}
