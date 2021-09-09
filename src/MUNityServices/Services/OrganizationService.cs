﻿using Microsoft.Extensions.Logging;
using MUNity.Database.Context;
using MUNity.Database.Models.Organization;
using MUNity.Database.Models.User;
using MUNity.Schema.Organization;
using MUNity.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Services
{
    public class OrganizationService
    {
        private readonly MunityContext context;

        ILogger<OrganizationService> log;

        public bool IsNameAvailable(string name)
        {
            return context.Organizations.All(n => n.OrganizationName.ToLower() != name.ToLower());
        }

        public bool IsShortAvailable(string shortName)
        {
            return context.Organizations.All(n => n.OrganizationShort.ToLower() != shortName.ToLower());
        }

        public Organization CreateOrganization(string name, string shortName, MunityUser user)
        {
            if (!IsNameAvailable(name))
            {
                log?.LogInformation($"Unable to create an organization with the name: {name} because its already taken.");
                throw new NameTakenException("There is already an organization with the name");
            } 

            if (!IsShortAvailable(shortName))
            {
                log?.LogInformation($"Unable to create an organization with the short: {shortName} because its already taken.");
                throw new NameTakenException("There is already an organization using that shortName");
            }
                
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
            context.OrganizationMember.Add(membership);
            context.SaveChanges();
            return organization;
        }

        public bool OrganizationWithIdExisits(string id)
        {
            return context.Organizations.Any(n => n.OrganizationId == id);
        }

        public bool IsUsernameMemberOfOrganiation(string username, string organizationId)
        {
            return context.OrganizationMember.Any(n => n.User.UserName == username && n.Organization.OrganizationId == organizationId);
        }

        public OrganizationTinyInfo GetTinyInfo(string organizationId)
        {
            return context.Organizations.Select(n => new OrganizationTinyInfo()
            {
                Name = n.OrganizationName,
                OrganizationId = n.OrganizationId,
                Short = n.OrganizationShort
            }).FirstOrDefault(n => n.OrganizationId == organizationId);
        }

        public OrganizationWithConferenceInfo GetOrgaConferenceInfo(string organizationId)
        {
            return context.Organizations.Select(n => new OrganizationWithConferenceInfo()
            {
                Name = n.OrganizationName,
                OrganizationId = n.OrganizationId,
                Short = n.OrganizationShort,
                ProjectCount = n.Projects.Count,
                ConferenceCount = n.Projects.Select(n => n.Conferences).Count()
            }).FirstOrDefault(n => n.OrganizationId == organizationId);
        }

        public OrganizationService(MunityContext context, ILogger<OrganizationService> logger)
        {
            this.context = context;
            this.log = logger;
        }
    }
}
