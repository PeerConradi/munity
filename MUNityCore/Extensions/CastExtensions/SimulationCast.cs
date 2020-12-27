﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNitySchema.Schema.Simulation;
using MUNityCore.Models.Simulation;

namespace MUNityCore.Extensions.CastExtensions
{
    public static class SimulationCast
    {
        public static SimulationRoleItem AsRoleItem(this SimulationRole role)
        {
            var mdl = new SimulationRoleItem()
            {
                Iso = role.Iso,
                Name = role.Name,
                RoleType = (SimulationEnums.RoleTypes)role.RoleType,
                SimulationRoleId = role.SimulationRoleId,
            };
            if (role.Simulation != null && role.Simulation.Users != null)
                mdl.Users = role.Simulation.Users.Where(n => n.Role == role).Select(n => n.DisplayName);
            return mdl;
        }

        public static SimulationUserItem AsUserItem(this SimulationUser user)
        {
            var mdl = new SimulationUserItem()
            {
                DisplayName = user.DisplayName,
                RoleId = user.Role?.SimulationRoleId ?? -2,
                SimulationUserId = user.SimulationUserId
            };
            return mdl;
        }

        public static SimulationResponse AsResponse (this Simulation simulation)
        {
            var mdl = new SimulationResponse()
            {
                Name = simulation.Name,
                Phase = (SimulationEnums.GamePhases)simulation.Phase,
                Roles = simulation.Roles?.Select(n => n.AsRoleItem()) ?? new List<SimulationRoleItem>(),
                Users = simulation.Users?.Select(n => n.AsUserItem()) ?? new List<SimulationUserItem>(),
                SimulationId = simulation.SimulationId
            };
            return mdl;
        }

        public static SimulationTokenResponse AsTokenResponse(this SimulationUser user)
        {
            var mdl = new SimulationTokenResponse()
            {
                SimulationId = user.Simulation.SimulationId,
                Name = user.Simulation.Name,
                Pin = user.Pin,
                Token = user.Token
            };
            return mdl;
        }

        public static SimulationListItem AsListItem(this Simulation simulation)
        {
            var mdl = new SimulationListItem()
            {
                Name = simulation.Name,
                Phase = (SimulationEnums.GamePhases)simulation.Phase,
                SimulationId = simulation.SimulationId,
                UsingPassword = !string.IsNullOrEmpty(simulation.Password)
            };
            return mdl;
        }

        public static SimulationAuthSchema AsAuthSchema(this SimulationUser user)
        {
            var mdl = new SimulationAuthSchema()
            {
                CanCreateRole = user.CanCreateRole,
                CanEditListOfSpeakers = user.CanEditListOfSpeakers,
                CanEditResolution = user.CanEditResolution,
                CanSelectRole = user.CanSelectRole,
                SimulationUserId = user.SimulationUserId
            };
            return mdl;
        }
    }
}