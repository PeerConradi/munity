using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MUNityCore.Models.Conference;
using MUNitySchema.Models.ListOfSpeakers;

namespace MUNityCore.Models.Simulation
{
    public class Simulation
    {

        public int SimulationId { get; set; }

        public string Name { get; set; }

        public MUNitySchema.Schema.Simulation.SimulationEnums.GamePhases Phase { get; set; }

        public MUNitySchema.Schema.Simulation.SimulationEnums.LobbyModes LobbyMode { get; set; }

        /// <summary>
        /// Momentaner Status wie Sitzung, Abstimmung oder informelle Sitzung Pause etc. als Text.
        /// </summary>
        public string Status { get; set; }

        public List<SimulationRole> Roles { get; set; } = new List<SimulationRole>();

        public List<SimulationUser> Users { get; set; } = new List<SimulationUser>();

        // Chat vorerst nicht speichern.
        //public List<AllChatMessage> AllChat { get; set; }

        public List<SimSimRequestModel> Requests { get; set; }

        public string Password { get; set; }

        /// <summary>
        /// Because everything works with tokens and you may delete your cache or
        /// cant remember this you need to be able to use the admin password to go back into the setup.
        /// </summary>
        public string AdminPassword { get; set; }

        public bool CanJoin { get; set; } = true;

        /// <summary>
        /// Die Redner in diesem Gremium.
        /// </summary>
        public ListOfSpeakers ListOfSpeakers { get; set; }

        /// <summary>
        /// Die Resolution welche Momentan in dem Gremium behandelt wird.
        /// </summary>
        public Resolution.V2.ResolutionAuth CurrentResolution { get; set; }


        public Simulation()
        {
            // Legacy Code:
            Requests = new List<SimSimRequestModel>();
        }
    }
}
