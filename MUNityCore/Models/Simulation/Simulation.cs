using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MUNityCore.Models.Conference;

namespace MUNityCore.Models.Simulation
{
    public class Simulation
    {
        public enum LobbyModes
        {
            /// <summary>
            /// Allows to Join the game with a role token and will then give
            /// you the role.
            /// </summary>
            WithRoleKey,
            /// <summary>
            /// You can join the lobby and pick a role
            /// </summary>
            PickRole,
            /// <summary>
            /// Allow everyone inside the Lobby to create a role
            /// </summary>
            CreateAnyRole,
            /// <summary>
            /// the lobby is closed you cannot join.
            /// </summary>
            Closed
        }

        public enum Phase
        {
            Offline,
            SettingUp,
            Online
        }

        public int SimulationId { get; set; }

        public string Name { get; set; }

        public LobbyModes LobbyMode { get; set; }

        public List<SimulationRole> Roles { get; set; } = new List<SimulationRole>();

        public List<SimulationUser> Users { get; set; } = new List<SimulationUser>();

        public List<AllChatMessage> AllChat { get; set; }

        public List<SimSimRequestModel> Requests { get; set; }


        [JsonIgnore]
        public string Password { get; set; }

        public bool UsingPassword => !string.IsNullOrEmpty(Password);

        public bool CanJoin { get; set; } = true;

        public ListOfSpeakers.ListOfSpeakers ListOfSpeakers { get; set; }


        public Simulation()
        {
            

            // Legacy Code:
            Requests = new List<SimSimRequestModel>();
            AllChat = new List<AllChatMessage>();
        }
    }
}
