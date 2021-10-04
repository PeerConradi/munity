using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MUNity.Database.Context;
using MUNity.Database.Models.Conference;
using MUNity.Database.Models.Conference.Roles;
using MUNity.Schema.Conference;
using MUNity.Schema.Extensions;

namespace MUNity.Services
{
    public class ConferenceRoleService
    {
        private readonly MunityContext _context;

        private readonly UserConferenceAuthService _authService;

        public async Task<CreateTeamRoleGroupResponse> CreateTeamRoleGroupAsync(CreateTeamRoleGroupRequest request, ClaimsPrincipal claim)
        {
            var response = new CreateTeamRoleGroupResponse();
            var isAllowed = await _authService.IsUserAllowedToEditTeam(request.ConferenceId, claim);
            if (!isAllowed)
            {
                response.AddNoPermissionError();
                return response;
            }

            var conference = _context.Conferences.FirstOrDefault(n => n.ConferenceId == request.ConferenceId);
            if (conference == null)
                response.AddNotFoundError(nameof(request.ConferenceId));


            if (_context.TeamRoleGroups.Any(n => n.Conference.ConferenceId == request.ConferenceId && n.Name == request.GroupName))
                response.AddNotFoundError(nameof(request.GroupName));


            if (_context.TeamRoleGroups.Any(n => n.Conference.ConferenceId == request.ConferenceId && n.FullName == request.GroupFullName))
                response.AddNotFoundError(nameof(request.GroupFullName));


            if (_context.TeamRoleGroups.Any(n => n.Conference.ConferenceId == request.ConferenceId && n.TeamRoleGroupShort == request.GroupShort))
                response.AddNotFoundError(nameof(request.GroupShort));


            if (response.HasError)
                return response;

            var group = new TeamRoleGroup()
            {
                Conference = conference,
                FullName = request.GroupFullName,
                GroupLevel = request.GroupLevel,
                Name = request.GroupName,
                TeamRoleGroupShort = request.GroupShort
            };

            _context.TeamRoleGroups.Add(group);
            await _context.SaveChangesAsync();
            response.CreatedGroupId = group.TeamRoleGroupId;
            return response;
        }

        public async Task<CreateTeamRoleResponse> CreateTeamRoleAsync(CreateTeamRoleRequest request, ClaimsPrincipal claim)
        {
            var response = new CreateTeamRoleResponse();
            var group = _context.TeamRoleGroups.Include(n => n.Conference)
                .FirstOrDefault(n => n.TeamRoleGroupId == request.RoleGroupId);

            if (group == null)
            {
                response.AddInvalidDataError("The given group was not found", nameof(request.RoleGroupId));
                return response;
            }

            var isAllowed = await _authService.IsUserAllowedToEditTeam(group.Conference.ConferenceId, claim);
            if (!isAllowed)
            {
                response.AddNoPermissionError("You don't have permission to create a Team role");
                return response;
            }

            ConferenceTeamRole parentRole = null;
            if (request.ParentRoleId != -1)
            {
                // We are also checking for the ConferenceId to make sure that the given Parent role is in
                // the same conference!
                parentRole = _context.ConferenceTeamRoles.FirstOrDefault(n =>
                n.RoleId == request.ParentRoleId &&
                n.Conference.ConferenceId == group.Conference.ConferenceId);
            }


            var role = new ConferenceTeamRole()
            {
                Conference = group.Conference,
                ParentTeamRole = parentRole,
                RoleName = request.RoleName,
                RoleShort = request.RoleShort,
                RoleFullName = request.RoleFullName,
                TeamRoleGroup = group,
                TeamRoleLevel = 0
            };
            _context.ConferenceTeamRoles.Add(role);
            await _context.SaveChangesAsync();
            response.RoleId = role.RoleId;

            return response;
        }


        public async Task<bool> RemoveDelegateRole(int roleId, ClaimsPrincipal claim)
        {
            var role = _context.Delegates
                .Include(n => n.Conference)
                .FirstOrDefault(n => n.RoleId == roleId);

            if (role == null)
                return false;

            var isAllowed = await _authService.IsUserAllowedToEditConference(role.Conference.ConferenceId, claim);
            if (!isAllowed)
                return false;

            _context.Delegates.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveTeamRole(int roleId, ClaimsPrincipal claim)
        {
            var role = _context.ConferenceTeamRoles
                .Include(n => n.Conference)
                .FirstOrDefault(n => n.RoleId == roleId);

            if (role == null)
                return false;

            var isAllowed = await _authService.IsUserAllowedToEditTeam(role.Conference.ConferenceId, claim);
            if (!isAllowed)
                return false;

            _context.ConferenceTeamRoles.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }

        public ConferenceRoleService(MunityContext context, UserConferenceAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
    }
}
