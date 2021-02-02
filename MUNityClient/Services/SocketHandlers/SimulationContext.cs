using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityClient.Models.Simulation;
using MUNity.Hubs;
using MUNity.Schema.Simulation;
using System.Collections.ObjectModel;

namespace MUNityClient.Services.SocketHandlers
{

    /// <summary>
    /// The SocketHandler is turning more and more into some kind of ViewModel.
    /// It handles the Socket operations and holds the Simulation Object.
    /// </summary>
    // TODO: Der Context müsste jetzt wo er eigentlich kein Service sondern eher ein ViewModel ist auch an eine andere Stelle verschoben werden...
    public class SimulationContext
    {
        public delegate void OnRolesChanged(int sender, IEnumerable<MUNity.Schema.Simulation.SimulationRoleItem> roles);
        public event OnRolesChanged RolesChanged;

        public delegate void OnUserRoleChanged(int sender, int userId, int roleId);
        public event OnUserRoleChanged UserRoleChanged;

        public delegate void OnUserConnected(int sender, MUNity.Schema.Simulation.SimulationUserItem user);
        public event OnUserConnected UserConnected;

        public delegate void OnUserDisconnected(int sender, MUNity.Schema.Simulation.SimulationUserItem user);
        public event OnUserDisconnected UserDisconnected;

        public delegate void OnPhaseChanged(int sender, MUNity.Schema.Simulation.SimulationEnums.GamePhases phase);
        public event OnPhaseChanged PhaseChanged;

        public delegate void OnStatusChanged(int sender, string newStatus);
        public event OnStatusChanged StatusChanged;

        public delegate void OnLobbyModeChanged(int sender, MUNity.Schema.Simulation.SimulationEnums.LobbyModes mode);
        public event OnLobbyModeChanged LobbyModeChanged;

        public delegate void OnChatMessageRecieved(int simId, int userId, string msg);
        public event OnChatMessageRecieved ChatMessageRevieved;

        public delegate void OnUserPetition(Petition petition);
        public event OnUserPetition UserPetition;

        public delegate void OnUserPetitionAccepted(Petition petition);
        public event OnUserPetitionAccepted UserPetitionAccpted;

        public delegate void OnUserPetitionDeleted(Petition petition);
        public event OnUserPetitionDeleted UserPetitionDeleted;

        public event EventHandler<MUNity.Schema.Simulation.VotedEventArgs> UserVoted;

        public event EventHandler<MUNity.Schema.Simulation.CreatedVoteModel> VoteCreated;

        public HubConnection HubConnection { get; set; }

        private readonly int _simulationId;

        public MUNity.Schema.Simulation.SimulationResponse Simulation { get; private set; }

        public IUserItem Me => MyAuth != null ? Simulation.Users.FirstOrDefault(n => n.SimulationUserId == MyAuth.SimulationUserId) : null;

        public ObservableCollection<Petition> Petitions { get; set; }

        public SimulationRoleItem MyRole
        {
            get
            {
                if (Me == null) return null;
                return Simulation?.Roles?.FirstOrDefault(n => n.SimulationRoleId == Me.RoleId);
            }
        }

        public SimulationAuthSchema MyAuth { get; private set; }

        private SimulationContext(SimulationResponse simulation, SimulationAuthSchema auth)
        {
            this.Simulation = simulation;
            this.MyAuth = auth;

            HubConnection = new HubConnectionBuilder().WithUrl($"{Program.API_URL}/simsocket").Build();
            HubConnection.On<int, IEnumerable<MUNity.Schema.Simulation.SimulationRoleItem>>("RolesChanged", (id, roles) => RolesChanged?.Invoke(id, roles));
            HubConnection.On<int, int, int>("UserRoleChanged", (simId, userId, roleId) => UserRoleChanged?.Invoke(simId, userId, roleId));
            HubConnection.On<int, MUNity.Schema.Simulation.SimulationUserItem>("UserConnected", (id, user) => UserConnected?.Invoke(id, user));
            HubConnection.On<int, MUNity.Schema.Simulation.SimulationUserItem>("UserDisconnected", (id, user) => UserDisconnected?.Invoke(id, user));
            HubConnection.On<int, MUNity.Schema.Simulation.SimulationEnums.GamePhases>("PhaseChanged", (id, phase) => PhaseChanged?.Invoke(id, phase));
            HubConnection.On<int, string>("StatusChanged", (id, status) => StatusChanged?.Invoke(id, status));
            HubConnection.On<int, MUNity.Schema.Simulation.SimulationEnums.LobbyModes>("LobbyModeChanged", (id, mode) => LobbyModeChanged?.Invoke(id, mode));
            HubConnection.On<int, int, string>("ChatMessageRecieved", (simId, usrId, msg) => ChatMessageRevieved?.Invoke(simId, usrId, msg));
            HubConnection.On<Petition>("UserPetition", (Petition petition) => UserPetition?.Invoke(petition));
            HubConnection.On<Petition>("UserPetitionAccepted", (Petition petition) => UserPetitionAccpted?.Invoke(petition));
            HubConnection.On<Petition>("UserPetitionDeleted", (Petition petition) => UserPetitionDeleted?.Invoke(petition));
            HubConnection.On<MUNity.Schema.Simulation.VotedEventArgs>("Voted", (args) => UserVoted?.Invoke(this, args));
            HubConnection.On<MUNity.Schema.Simulation.CreatedVoteModel>("VoteCreated", (args) => VoteCreated?.Invoke(this, args));
        }

        public static async Task<SimulationContext> CreateHander(SimulationResponse simulation, SimulationAuthSchema auth)
        {
            var socket = new SimulationContext(simulation, auth);
            await socket.HubConnection.StartAsync();
            return socket;
        }
       
    }
}
