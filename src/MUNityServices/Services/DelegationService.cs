using Microsoft.AspNetCore.Identity;
using MUNity.Database.Context;
using MUNity.Database.Models.User;
using MUNity.Schema.Conference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MUNity.Schema.Extensions;
using MUNity.Database.Models.Conference;
using Microsoft.EntityFrameworkCore;

namespace MUNity.Services
{
    public class DelegationService
    {
        private MunityContext context;

        private UserConferenceAuthService authService;

        public async Task<ManageDelegationsInfo> GetManageDelegationsInfo(string conferenceId, ClaimsPrincipal claim)
        {
            var isAllowed = await authService.IsUserAllowedToEditConference(conferenceId, claim);
            if (!isAllowed)
            {
                return null;
            }

            ManageDelegationsInfo info = context.Conferences
                .Include(n => n.Delegations)
                .ThenInclude(n => n.Roles)
                .AsSingleQuery()
                .Select(conf => new ManageDelegationsInfo()
            {
                ConferenceId = conf.ConferenceId,
                ConferenceName = conf.Name,
                ConferenceShort = conf.ConferenceShort,
                OrganizationId = conf.ConferenceProject.ProjectOrganization.OrganizationId,
                OrganizationName = conf.ConferenceProject.ProjectOrganization.OrganizationName,
                OrganizationShort = conf.ConferenceProject.ProjectOrganization.OrganizationShort,
                ProjectId = conf.ConferenceProject.ProjectId,
                ProjectName = conf.ConferenceProject.ProjectName,
                ProjectShort = conf.ConferenceProject.ProjectShort,
                Delegations = conf.Delegations.Select(del => new ManageDelegationInfo()
                {
                    DelegationId = del.DelegationId,
                    DelegationName = del.Name,
                    DelegationShort = del.DelegationShort,
                    Roles = del.Roles.Select(role => new ManageDelegationRoleInfo()
                    {
                        ApplicationState = role.ApplicationState,
                        HasParicipant = role.Participations.Any(),
                        RoleCommitteeId = role.Committee.CommitteeId,
                        RoleCommitteeName = role.Committee.Name,
                        RoleId = role.RoleId,
                        RoleName = role.RoleName
                    }).ToList()
                }).ToList()
            }).FirstOrDefault(n => n.ConferenceId == conferenceId);
            return info;
        }

        public async Task<CreateDelegationResponse> CreateDelegationAsync(CreateDelegationRequest request, ClaimsPrincipal claim)
        {
            var response = new CreateDelegationResponse();
            var isAllowed = await authService.IsUserAllowedToEditConference(request.ConferenceId, claim);
            if (!isAllowed)
            {
                response.AddNoPermissionError();
                return response;
            }

            var conference = context.Conferences.FirstOrDefault(n => n.ConferenceId == request.ConferenceId);
            if (conference == null)
            {
                response.AddNotFoundError(nameof(request.ConferenceId));
                return response;
            }

            var delegation = new Delegation()
            {
                Conference = conference,
                DelegationShort = request.DelegationShort,
                FullName = request.DelegationFullName,
                Name = request.DelegationName
            };

            string easyId = conference.ConferenceId + "-" + Util.IdGenerator.AsPrimaryKey(request.DelegationShort);
            if (context.Delegations.All(n => n.DelegationId != easyId))
                delegation.DelegationId = easyId;

            context.Delegations.Add(delegation);
            context.SaveChanges();

            return response;
        }

        public async Task<List<ManageDelegationRoleInfo>> GetAvailableRoles(string conferenceId, ClaimsPrincipal claim)
        {
            var isAllowed = await authService.IsUserAllowedToEditConference(conferenceId, claim);
            if (!isAllowed)
                return new List<ManageDelegationRoleInfo>();

            var roles = context.Delegates
                .Where(n => n.Delegation == null && n.Conference.ConferenceId == conferenceId)
                .OrderBy(n => n.RoleName)
                .Select(n => new ManageDelegationRoleInfo()
                {
                    ApplicationState = n.ApplicationState,
                    HasParicipant = n.Participations.Any(),
                    RoleCommitteeId = n.Committee.CommitteeId,
                    RoleCommitteeName = n.Committee.Name,
                    RoleId = n.RoleId,
                    RoleName = n.RoleName
                }).ToList();
            return roles;
        }

        public async Task<bool> AddRoleToDelegation(int roleId, string delegationId, ClaimsPrincipal claim)
        {
            var role = context.Delegates
                .Include(n => n.Conference)
                .FirstOrDefault(n => n.RoleId == roleId);
            if (role == null || role.Conference == null)
                return false;

            var isAllowed = await authService.IsUserAllowedToEditConference(role.Conference.ConferenceId, claim);
            if (!isAllowed)
                return false;

            var delegation = context.Delegations
                .Include(n => n.Conference)
                .FirstOrDefault(n => n.DelegationId == delegationId);

            if (role.Conference.ConferenceId != delegation.Conference.ConferenceId)
                return false;

            role.Delegation = delegation;
            context.SaveChanges();
            return true;
        }

        public DelegationService(MunityContext context, UserConferenceAuthService authService)
        {
            this.context = context;
            this.authService = authService;
        }
    }
}
