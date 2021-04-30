using Microsoft.AspNetCore.SignalR;
using MUNityCore.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityCore.Extensions.CastExtensions;
using MUNityCore.Components.Simulation;
using MUNityCore.DataHandlers.EntityFramework;
using MUNity.Schema.Simulation;

namespace MUNityCore.Hubs
{
    public class SimulationHub : Hub<MUNity.Hubs.ITypedSimulationHub>
    {
        private readonly Services.SimulationService _service;

        public SimulationHub(Services.SimulationService service)
        {
            _service = service;
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            ConnectionUsers.ConnectedUser outVal = null;
            ConnectionUsers.ConnectionIds.TryRemove(Context.ConnectionId, out outVal);
            Console.WriteLine("Simulation User Disconnected: " + outVal.SimulationUserId.ToString());
            await NotifyUsersChanged(outVal.SimulationId);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SignIn(int simulationId, int userId)
        {
            var newConnection = new ConnectionUsers.ConnectedUser()
            {
                SimulationId = simulationId,
                SimulationUserId = userId
            };
            Console.WriteLine("Simulation User Connected: " + userId.ToString());
            ConnectionUsers.ConnectionIds.TryAdd(Context.ConnectionId, newConnection);
            await this.Groups.AddToGroupAsync(Context.ConnectionId, "sim_" + simulationId);
            await NotifyUsersChanged(simulationId);
            
        }

        private Task NotifyUsersChanged(int simulationId)
        {
            return Clients.Group($"sim_{simulationId}")
                .ConnectedUsersChanged(ConnectionUsers.ConnectionIds.Where(n =>
                n.Value.SimulationId == simulationId)
                .Select(n => n.Value.SimulationUserId).ToList());
        }

        public void CreateVotingForDelegates(int simulationId, string text, bool allowAbstentions)
        {
            if (!IsContextChair())
                return;

            var mdl = _service.CreateVotingForDelegates(simulationId, text, allowAbstentions);

            var dto = _service.GetCurrentVoting(simulationId);
            this.Clients.Group("sim_" + simulationId.ToString()).VoteCreated(dto);
            return;
        }

        public async Task Vote(string votingId, MUNity.Schema.Simulation.EVoteStates choice)
        {
            var user = GetContextUser();
            if (user == null) return;

            var voted = _service.VoteByUserId(votingId, user.SimulationUserId, choice);
            if (voted)
            {
                await this.Clients.Group("sim_" + user.SimulationId).Voted(new MUNity.Schema.Simulation.VotedEventArgs()
                {
                    Choice = choice,
                    UserId = user.SimulationUserId,
                    VoteId = votingId
                });
            }
        }

        public void CreateAgendaItem(string name, string description)
        {
            var user = GetContextUser();
            if (user == null) return;
            var isChair = _service.IsUserChair(user.SimulationUserId);

            var agendaItem = this._service.CreateAgendaItem(user.SimulationId, name, description);
            if (agendaItem != null)
            {
                this.Clients.Group("sim_" + user.SimulationId).AgendaItemAdded(agendaItem.ToAgendaItemDto());
            }
        }

        public async Task RemoveAgendaItem(int agendaItemId)
        {
            var user = GetContextUser();
            if (user == null) return;
            var isChair = _service.IsUserChair(user.SimulationUserId);

            var removed = this._service.RemoveAgendaItem(agendaItemId);
            if (removed)
            {
                await this.Clients.Group("sim_" + user.SimulationId).AgendaItemRemoved(agendaItemId);
            }
        }
        
        public async Task MakePetition(int agendaItemId, int petitionTypeId )
        {
            var user = GetContextUser();
            if (user == null) return;

            var petition = this._service.SubmitPetition(agendaItemId, petitionTypeId, user.SimulationUserId);
            var info = _service.GetPetitionInfo(petition.PetitionId);
            if (info != null)
            {
                await this.Clients.Group("sim_" + user.SimulationId).PetitionAdded(info);
            }
        }

        public async Task ActivatePetition(string petitionId)
        {
            var user = GetContextUser();
            if (user == null)
                return;

            var isChair = _service.IsUserChair(user.SimulationUserId);
            if (!isChair)
                return;

            this._service.ActivatePetition(petitionId);
            await this.Clients.Group("sim_" + user.SimulationId).PetitionActivated(petitionId);
        }

        public async Task RemovePetition(string petitionId)
        {
            var user = GetContextUser();
            if (user == null)
                return;

            var isChair = _service.IsUserChair(user.SimulationUserId);
            if (!isChair)
                return;

            var agendaItemId = this._service.RemovePetition(petitionId);
            if (agendaItemId != -1)
            {
                await this.Clients.Group("sim_" + user.SimulationId).PetitionDeleted(new MUNity.Schema.Simulation.PetitionInteractedDto()
                {
                    AgendaItemId = agendaItemId,
                    PetitionId = petitionId
                });
            }
        }

        public async Task ChangeStatus(string newStatusText)
        {
            var user = GetContextUser();
            if (user == null)
                return;

            var isChair = _service.IsUserChair(user.SimulationUserId);
            if (!isChair)
                return;

            var newStatus = this._service.SetStatus(user.SimulationId, newStatusText);
            if (newStatus != null)
            {
                var newStatusSocketMessage = new SimulationStatusDto()
                {
                    SimulationStatusId = newStatus.SimulationStatusId,
                    StatusText = newStatus.StatusText,
                    StatusTime = newStatus.StatusTime
                };
                await this.Clients.Group("sim_" + user.SimulationId.ToString()).StatusChanged(newStatusSocketMessage);
            }
        }


        private ConnectionUsers.ConnectedUser GetContextUser()
        {
            ConnectionUsers.ConnectedUser user;
            if (ConnectionUsers.ConnectionIds.TryGetValue(Context.ConnectionId, out user))
            {
                return user;
            }
            return null;
        }

        private bool IsContextChair()
        {
            ConnectionUsers.ConnectedUser user;
            if (ConnectionUsers.ConnectionIds.TryGetValue(Context.ConnectionId, out user))
            {
                return this._service.IsUserChair(user.SimulationUserId);
            }
            return false;
        }

        //public override Task OnConnectedAsync()
        //{
        //    if (ConnectionUsers.ConnectionIds == null)
        //        ConnectionUsers.ConnectionIds = new System.Collections.Concurrent.ConcurrentDictionary<string, bool>();
        //    ConnectionUsers.ConnectionIds.TryAdd(Context.ConnectionId, true);
        //    return base.OnConnectedAsync();
        //}

    }

    public static class ConnectionUsers
    {

        public static System.Collections.Concurrent.ConcurrentDictionary<string, ConnectedUser> ConnectionIds { get; set; } = new System.Collections.Concurrent.ConcurrentDictionary<string, ConnectedUser>();

        public class ConnectedUser
        {
            public int SimulationId { get; set; }

            public int SimulationUserId { get; set; }
        }
    }
}
