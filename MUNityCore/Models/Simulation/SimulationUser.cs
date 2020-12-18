using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUNityCore.Models.Simulation
{
    public class SimulationUser
    {
        public int SimulationUserId { get; set; }

        public string Token { get; set; }

        public string DisplayName { get; set; }

        public string Pin { get; set; }

        public SimulationRole Role { get; set; }

        public bool CanCreateRole { get; set; }

        public bool CanSelectRole { get; set; }

        public bool CanEditResolution { get; set; } = false;

        public bool CanEditListOfSpeakers { get; set; } = false;

        public List<SimulationHubConnection> HubConnections { get; set; }

        public Simulation Simulation { get; set; }

        public SimulationUser()
        {
            this.Token = Util.Tools.IdGenerator.RandomString(20);
            this.Pin = new Random().Next(1000, 9999).ToString();
            this.HubConnections = new List<SimulationHubConnection>();
        }
    }
}
