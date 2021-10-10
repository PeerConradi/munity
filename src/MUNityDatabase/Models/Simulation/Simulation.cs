using MUNityBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUNity.Database.Models.Simulation
{
    public class Simulation
    {

        public int SimulationId { get; set; }

        public string Name { get; set; }

        public GamePhases Phase { get; set; }

        public LobbyModes LobbyMode { get; set; }

        public DateTime? LastStatusChange { get; set; }

        /// <summary>
        /// The password is used for administration of the Simulation.
        /// </summary>
        public string Password { get; set; }

        public ICollection<SimulationRole> Roles { get; set; }

        public ICollection<SimulationUser> Users { get; set; }

        // Chat vorerst nicht speichern.
        //public List<AllChatMessage> AllChat { get; set; }

        public ICollection<SimulationStatus> Statuses { get; set; }

        public ICollection<AgendaItem> AgendaItems { get; set; }

        public ICollection<PetitionTypeSimulation> PetitionTypes { get; set; }

        /// <summary>
        /// Die Redner in diesem Gremium.
        /// </summary>
        public MUNity.Database.Models.LoS.ListOfSpeakers ListOfSpeakers { get; set; }

        public string CurrentResolutionId { get; set; }

        public ICollection<Resolution.ResolutionAuth> Resolutions { get; set; }

        public ICollection<SimulationPresents> PresentChecks { get; set; }


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
