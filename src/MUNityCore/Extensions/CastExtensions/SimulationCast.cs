using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNity.Schema.Simulation;
using MUNityCore.Models.Simulation;

namespace MUNityCore.Extensions.CastExtensions
{
    public static class SimulationCast
    {
        public static SimulationRoleDto ToSimulationRoleDto(this SimulationRole role)
        {
            var mdl = new SimulationRoleDto()
            {
                Iso = role.Iso,
                Name = role.Name,
                RoleType = role.RoleType,
                SimulationRoleId = role.SimulationRoleId,
            };
            if (role.Simulation != null && role.Simulation.Users != null)
                mdl.Users = role.Simulation.Users.Where(n => n.Role == role).Select(n => n.DisplayName).ToList();
            return mdl;
        }

        public static SimulationUserDefaultDto AsSimulationUserDefaultDto(this SimulationUser user)
        {
            var mdl = new SimulationUserDefaultDto()
            {
                DisplayName = user.DisplayName,
                RoleId = user.Role?.SimulationRoleId ?? -2,
                SimulationUserId = user.SimulationUserId,
                IsOnline = MUNityCore.Hubs.ConnectionUsers.ConnectionIds.Any(n => n.Value.SimulationUserId == user.SimulationUserId)
            };
            return mdl;
        }

        public static SimulationDto ToSimulationDto (this Simulation simulation)
        {
            var mdl = new SimulationDto()
            {
                Name = simulation.Name,
                Phase = simulation.Phase,
                Roles = simulation.Roles?.Select(n => n.ToSimulationRoleDto()).ToList() ?? new List<SimulationRoleDto>(),
                Users = simulation.Users?.Select(n => n.AsSimulationUserDefaultDto()).ToList() ?? new List<SimulationUserDefaultDto>(),
                SimulationId = simulation.SimulationId
            };
            return mdl;
        }

        public static SimulationTokenResponse ToTokenResponse(this SimulationUser user)
        {
            var mdl = new SimulationTokenResponse()
            {
                SimulationId = user.Simulation.SimulationId,
                Name = user.Simulation.Name,
                Pin = user.Password,
                Token = user.Token
            };
            return mdl;
        }

        public static SimulationListItemDto ToSimulationListItemDto(this Simulation simulation)
        {
            var mdl = new SimulationListItemDto()
            {
                Name = simulation.Name,
                Phase = simulation.Phase,
                SimulationId = simulation.SimulationId,
                UsingPassword = !string.IsNullOrEmpty(simulation.Password)
            };
            return mdl;
        }

        public static SimulationAuthDto ToSimulationAuthDto(this SimulationUser user)
        {
            var mdl = new SimulationAuthDto()
            {
                CanCreateRole = user.CanCreateRole,
                CanEditListOfSpeakers = user.CanEditListOfSpeakers,
                CanEditResolution = user.CanEditResolution,
                CanSelectRole = user.CanSelectRole,
                SimulationUserId = user.SimulationUserId
            };
            return mdl;
        }

        public static SimulationUserAdminDto ToSimulationUserAdminDto(this SimulationUser user) 
        {
            var setup = new SimulationUserAdminDto()
            {
                SimulationUserId = user.SimulationUserId,
                DisplayName = user.DisplayName,
                RoleId = user.Role?.SimulationRoleId ?? -2,
                IsOnline = MUNityCore.Hubs.ConnectionUsers.ConnectionIds.Any(n => n.Value.SimulationUserId == user.SimulationUserId),
                PublicId = user.PublicUserId,
                Password = user.Password,
            };
            return setup;
        }

        public static PetitionDto ToPetitionDto(this Models.Simulation.Petition petition)
        {
            var model = new PetitionDto()
            {
                PetitionDate = petition.PetitionDate,
                PetitionId = petition.PetitionId,
                PetitionTypeId = petition.PetitionType.PetitionTypeId,
                PetitionUserId = petition.SimulationUser.SimulationUserId,
                TargetAgendaItemId = petition.AgendaItem.AgendaItemId,
                Status = petition.Status,
                Text = petition.Text,
            };
            return model;
        }

        public static AgendaItemDto ToAgendaItemDto(this AgendaItem agendaItem)
        {
            var mdl = new AgendaItemDto()
            {
                AgendaItemId = agendaItem.AgendaItemId,
                Description = agendaItem.Description,
                Name = agendaItem.Name,
                Petitions = agendaItem.Petitions.ToPetitionDtoList(),
                Status = agendaItem.Status
            };
            return mdl;
        }

        public static List<PetitionDto> ToPetitionDtoList(this List<Petition> petitions)
        {
            if (petitions == null)
                return new List<PetitionDto>();
            var items = petitions.Select(n => n.ToPetitionDto());
            if (items != null && items.Any()) return items.ToList();
            return new List<PetitionDto>();
        }

        public static MUNity.Schema.Simulation.SimulationStatusDto ToModel(this SimulationStatus status)
        {
            if (status == null)
                return null;

            var newStatusSocketMessage = new SimulationStatusDto()
            {
                SimulationStatusId = status.SimulationStatusId,
                StatusText = status.StatusText,
                StatusTime = status.StatusTime
            };
            return newStatusSocketMessage;
        }
    }
}
