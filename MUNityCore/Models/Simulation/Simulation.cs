using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MUNityCore.Models.Conference;
using MUNity.Models.ListOfSpeakers;

namespace MUNityCore.Models.Simulation
{
    public class Simulation
    {

        public int SimulationId { get; set; }

        public string Name { get; set; }

        public MUNity.Schema.Simulation.SimulationEnums.GamePhases Phase { get; set; }

        public MUNity.Schema.Simulation.SimulationEnums.LobbyModes LobbyMode { get; set; }

        /// <summary>
        /// Momentaner Status wie Sitzung, Abstimmung oder informelle Sitzung Pause etc. als Text.
        /// </summary>
        public string Status { get; set; }

        public DateTime? LastStatusChange { get; set; }

        /// <summary>
        /// The password is used for administration of the Simulation.
        /// </summary>
        public string Password { get; set; }

        public List<SimulationRole> Roles { get; set; } = new List<SimulationRole>();

        public List<SimulationUser> Users { get; set; } = new List<SimulationUser>();

        // Chat vorerst nicht speichern.
        //public List<AllChatMessage> AllChat { get; set; }

        public List<AgendaItem> AgendaItems { get; set; }

        public List<PetitionTypeSimulation> PetitionTypes { get; set; }

        /// <summary>
        /// Die Redner in diesem Gremium.
        /// </summary>
        public ListOfSpeakers ListOfSpeakers { get; set; }

        public string CurrentResolutionId { get; set; }

        public List<Resolution.V2.ResolutionAuth> Resolutions { get; set; }


        public Simulation()
        {
            // Legacy Code:
            //Requests = new List<SimSimRequestModel>();
        }
    }
}
