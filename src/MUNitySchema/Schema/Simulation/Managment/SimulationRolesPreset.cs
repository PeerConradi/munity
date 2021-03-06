﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Simulation
{
    public class SimulationRolesPreset
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public List<MUNity.Schema.Simulation.SimulationRoleDto> Roles { get; set; }
    }
}
