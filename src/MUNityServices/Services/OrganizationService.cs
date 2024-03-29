﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MUNity.Database.Context;
using MUNity.Database.Models.Organization;
using MUNity.Database.Models.User;
using MUNity.Schema.Organization;
using MUNity.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Services
{
    public class OrganizationService
    {
        private readonly MunityContext context;

        ILogger<OrganizationService> log;

        private UserManager<MunityUser> userManager;

        public bool IsNameAvailable(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;
            return context.Organizations.AsNoTracking().All(n => n.OrganizationName.ToLower() != name.ToLower());
        }

        public bool IsShortAvailable(string shortName)
        {
            if (string.IsNullOrWhiteSpace(shortName))
                return false;
            return context.Organizations.AsNoTracking().All(n => n.OrganizationShort.ToLower() != shortName.ToLower());
        }

        public Organization CreateOrganization(string name, string shortName, MunityUser user)
        {
                
            var organization = new Organization()
            {
                OrganizationName = name,
                OrganizationShort = shortName,
            };

            var easyId = Util.IdGenerator.AsPrimaryKey(shortName);
            if (context.Organizations.All(n => n.OrganizationId != easyId))
                organization.OrganizationId = easyId;

            var orgaAdminRole = new OrganizationRole()
            {
                CanCreateProject = true,
                CanManageMembers = true,
                CanCreateRoles = true,
                RoleName = "Admin",
                Organization = organization
            };

            var membership = new OrganizationMember()
            {
                JoinedDate = DateTime.Now,
                Organization = organization,
                Role = orgaAdminRole,
                User = user
            };

            context.Organizations.Add(organization);
            context.OrganizationRoles.Add(orgaAdminRole);
            context.OrganizationMembers.Add(membership);
            context.SaveChanges();
            return organization;
        }

        public async Task<CreateOrganizationResponse> CreateOrganizationAsync(CreateOrganizationRequest request, ClaimsPrincipal claim)
        {
            log?.LogDebug("Request creating an organization!");

            var response = new CreateOrganizationResponse();

            if (claim == null ||claim.Identity == null || string.IsNullOrEmpty(claim.Identity.Name))
            {
                response.Status = CreateOrganizationResponse.CreateOrgaStatusCodes.Error;
                log?.LogDebug($"The given claim was not valid.");
                return response;
            }

            log?.LogInformation($"{claim.Identity.Name} wants to create the organization {request.Name}");

            var user = await userManager.GetUserAsync(claim);
            if (user == null)
            {
                response.Status = CreateOrganizationResponse.CreateOrgaStatusCodes.Error;
                log?.LogDebug($"User was not found to create the organization.");
                return response;
            }

            if (!IsNameAvailable(request.Name))
            {
                response.Status = CreateOrganizationResponse.CreateOrgaStatusCodes.NameTaken;
                log?.LogDebug($"Organization Name: {request.Name} was already taken!");
                return response;
            }

            if (!IsShortAvailable(request.ShortName))
            {
                response.Status = CreateOrganizationResponse.CreateOrgaStatusCodes.ShortTaken;
                log?.LogDebug($"Organization Short: {request.ShortName} was already taken!");
                return response;
            }

            var createdOrganization = CreateOrganization(request.Name, request.ShortName, user);
            if (createdOrganization != null)
            {
                response.OrganizationId = createdOrganization.OrganizationId;
                log?.LogDebug($"Organization {request.Name} with the id {createdOrganization.OrganizationId} has been created!");
            }
            return response;
        }

        

        public bool OrganizationWithIdExisits(string id)
        {
            return context.Organizations.AsNoTracking().Any(n => n.OrganizationId == id);
        }

        public bool IsUsernameMemberOfOrganiation(string username, string organizationId)
        {
            return context.OrganizationMembers.AsNoTracking().Any(n => n.User.UserName == username && n.Organization.OrganizationId == organizationId);
        }

        public bool IsMemberOfOrganization(ClaimsPrincipal principal, string organizationId)
        {
            return IsUsernameMemberOfOrganiation(principal.Identity.Name, organizationId);
        }

        public OrganizationTinyInfo GetTinyInfo(string organizationId)
        {
            return context.Organizations.AsNoTracking().Select(n => new OrganizationTinyInfo()
            {
                Name = n.OrganizationName,
                OrganizationId = n.OrganizationId,
                Short = n.OrganizationShort
            }).FirstOrDefault(n => n.OrganizationId == organizationId);
        }

        public List<OrganizationTinyInfo> GetTyinInfosOfAllOrgas()
        {
            return context.Organizations.AsNoTracking().Select(n => new OrganizationTinyInfo()
            {
                Name = n.OrganizationName,
                OrganizationId = n.OrganizationId,
                Short = n.OrganizationShort
            }).ToList();
        }

        public List<OrganizationTinyInfo> GetTinyInfoOfUserOrganizations(string username)
        {
            return context.Organizations.AsNoTracking().Where(n => n.Member.Any(a => a.User.UserName == username))
                .Select(n => new OrganizationTinyInfo()
                {
                    Name = n.OrganizationName,
                    OrganizationId = n.OrganizationId,
                    Short = n.OrganizationShort
                }).ToList();
        }

        public List<OrganizationTinyInfo> GetTinyInfoOfUserOrganizations(ClaimsPrincipal claimPrincipal)
        {
            return GetTinyInfoOfUserOrganizations(claimPrincipal.Identity.Name);
        }

        public OrganizationWithConferenceInfo GetOrgaConferenceInfo(string organizationId)
        {
            return context.Organizations.AsNoTracking().Select(n => new OrganizationWithConferenceInfo()
            {
                Name = n.OrganizationName,
                OrganizationId = n.OrganizationId,
                Short = n.OrganizationShort,
                ProjectCount = n.Projects.Count,
                ConferenceCount = n.Projects.Select(n => n.Conferences).Count()
            }).FirstOrDefault(n => n.OrganizationId == organizationId);
        }

        public Organization GetCompleteOrganizationInfo(string organizationId)
        {
            return context.Organizations
                .AsNoTracking()
                .Include(n => n.Member)
                .ThenInclude(n => n.User)
                .Include(n => n.Projects)
                .Include(n => n.Roles)
                .FirstOrDefault(n => n.OrganizationId == organizationId);
        }

        public OrganizationDashboardInfo GetDashboardInfo(string organizationId)
        {
            var info = context.Organizations
                .AsNoTracking()
                .Select(n => new OrganizationDashboardInfo()
                {
                    OrganizationId = n.OrganizationId,
                    Name = n.OrganizationName,
                    Short = n.OrganizationShort,
                    Projects = n.Projects.Select(a => new OrganizationDashboardProjectInfo()
                    {
                        ConferenceCount = a.Conferences.Count,
                        Name = a.ProjectName,
                        ProjectId = a.ProjectId
                    }).ToList()
                })
                .FirstOrDefault(n => n.OrganizationId == organizationId);
            info.Memebrs = context.OrganizationMembers
                .Where(n => n.Organization.OrganizationId == organizationId)
                .Take(8)
                .Select(a => new OrganizationMemberInfo()
                {
                    Forename = a.User.Forename,
                    LastName = a.User.Lastname,
                    MemberUserName = a.User.UserName,
                    RoleName = a.Role.RoleName
                }).ToList();

            return info;
        }

        public OrganizationService(MunityContext context, ILogger<OrganizationService> logger, UserManager<MunityUser> userManager)
        {
            this.context = context;
            this.log = logger;
            this.userManager = userManager;
        }
    }
}
