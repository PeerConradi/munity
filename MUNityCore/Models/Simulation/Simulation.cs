using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MUNityCore.DataHandlers.EntityFramework.Models;
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

        public int SimulationId { get; set; }

        public string Name { get; set; }

        public LobbyModes LobbyMode { get; set; }

        public List<SimulationRole> Roles { get; set; } = new List<SimulationRole>();

        public List<SimulationUser> Users { get; set; } = new List<SimulationUser>();

        public List<AllChatMessage> AllChat { get; set; }

        public List<SimSimRequestModel> Requests { get; set; }

        //internal void RemoveUser(ISimSimUserFacade user)
        //{
        //    _leaderIds.Remove(user.UserToken);
        //    Users.Remove(user);
        //    var signalRkey = SignalRConnections.FirstOrDefault(n => n.Value == user).Key ?? null;
        //    if (signalRkey != null)
        //        SignalRConnections.Remove(signalRkey);

        //    var tokenKey = _userTokens.FirstOrDefault(n => n.Value == user).Key;
        //    if (tokenKey != null)
        //        _userTokens.Remove(tokenKey);
        //}


        [JsonIgnore]
        public string Password { get; set; }

        public bool UsingPassword { get => !string.IsNullOrEmpty(Password); }

        public bool CanJoin { get; set; } = true;

        public SpeakerlistModel ListOfSpeakers { get; set; }




        public Simulation()
        {
            SimulationRole ownerRole = new SimulationRole()
            {
                Name = "Owner",
                Iso = "UN",
                RoleKey = Util.Tools.IdGenerator.RandomString(32),
                RoleType = SimulationRole.RoleTypes.Moderator,
                RoleMaxSlots = 1,
            };

            this.Roles.Add(ownerRole);

            // Legacy Code:
            Requests = new List<SimSimRequestModel>();
            AllChat = new List<AllChatMessage>();
        }
    }
}
