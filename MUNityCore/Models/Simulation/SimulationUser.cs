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

        public string DisplayName { get; set; }

        public SimulationRole Role { get; set; }

        [JsonIgnore] public bool CanEditResolution { get; set; } = false;

        [JsonIgnore] public bool CanEditListOfSpeakers { get; set; } = false;

        [JsonIgnore] public Simulation Simulation { get; set; }
    }
}
