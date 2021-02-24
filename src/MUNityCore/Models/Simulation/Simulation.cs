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

        public MUNity.Schema.Simulation.GamePhases Phase { get; set; }

        public MUNity.Schema.Simulation.LobbyModes LobbyMode { get; set; }

        public DateTime? LastStatusChange { get; set; }

        /// <summary>
        /// The password is used for administration of the Simulation.
        /// </summary>
        public string Password { get; set; }

        public List<SimulationRole> Roles { get; set; }

        public List<SimulationUser> Users { get; set; }

        // Chat vorerst nicht speichern.
        //public List<AllChatMessage> AllChat { get; set; }

        public List<SimulationStatus> Statuses { get; set; }

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
            //Users = new List<SimulationUser>();
            //Roles = new List<SimulationRole>();
            //Statuses = new List<SimulationStatus>();
            // Legacy Code:
            //Requests = new List<SimSimRequestModel>();
        }
    }
}
