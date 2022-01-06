using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.VirtualCommittee.Dtos
{
    /// <summary>
    /// Applies a preset for a given Simulation Room.
    /// </summary>
    public class ApplyPreset : SimulationRequest
    {
        /// <summary>
        /// The id of the preset to apply
        /// </summary>
        public string PresetId { get; set; }
    }
}
