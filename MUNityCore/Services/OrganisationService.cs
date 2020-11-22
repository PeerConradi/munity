using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MUNityCore.DataHandlers.EntityFramework;
using MUNityCore.Models.Conference;
using MUNityCore.Models.User;
using MUNityCore.Models.Organization;

namespace MUNityCore.Services
{
    public class OrganisationService : IOrganisationService
    {
        private MunityContext _context;

        public Organization CreateOrganisation([NotNull]string name, [NotNull]string abbreviation)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(abbreviation))
                return null;

            var organisation = new Organization();

            organisation.OrganizationId = Guid.NewGuid().ToString();
            var shortAsKey = Util.Tools.IdGenerator.AsPrimaryKey(abbreviation);
            if (!_context.Organizations.Any(n => n.OrganizationId == shortAsKey))
                organisation.OrganizationId = shortAsKey;

            organisation.OrganizationName = name;
            organisation.OrganizationAbbreviation = abbreviation;
            _context.Organizations.Add(organisation);
            _context.SaveChanges();
            return organisation;
        }

        public Task<Organization> GetOrganisation(string id)
        {
            return _context.Organizations.FirstOrDefaultAsync(n => n.OrganizationId == id);
        }

        public Task<Organization> GetOrganisationWithMembers(string id)
        {
            return _context.Organizations.Include(n => n.Member)
                .ThenInclude(n => n.User)
                .FirstOrDefaultAsync(n => n.OrganizationId == id);
        }

        public Task<Project> GetProjectWithOrganisation(string id)
        {
            return _context.Projects
                .Include(n => n.ProjectOrganization)
                .FirstOrDefaultAsync(n => n.ProjectId == id);
        }

        public bool CanUserCreateProject(string username, string organisationId)
        {
            var result = _context.OrganizationMember.Any(n => n.User.Username == username &&
                                                 n.Organization.OrganizationId == organisationId &&
                                                 n.Role.CanCreateProject == true);
            return result;
        }

        public IQueryable<Project> GetOrganisationProjects(string organisationId) =>
            this._context.Projects
            .Include(n => n.ProjectOrganization)
            .Include(n => n.Conferences)
            .Where(n => n.ProjectOrganization.OrganizationId == organisationId);

        public OrganizationRole AddOrganisationRole(Organization organization, string rolename, bool canCreateConferences = false)
        {
            var role = new OrganizationRole();
            role.RoleName = rolename;
            role.CanCreateProject = canCreateConferences;

            role.Organization = organization;
            _context.OrganizationRoles.Add(role);
            _context.SaveChanges();

            return role;
        }

        public IQueryable<OrganizationRole> GetOrganisationRoles(string organisationId)
        {
            return _context.OrganizationRoles.Where(n => n.Organization.OrganizationId == organisationId);
        }

        public OrganizationMember AddUserToOrganisation(MunityUser user, Organization organization, OrganizationRole role)
        {
            var membership = new OrganizationMember();
            membership.User = user;
            membership.Role = role;
            membership.Organization = organization;

            _context.OrganizationMember.Add(membership);
            _context.SaveChanges();

            return membership;
        }

        public IEnumerable<Organization> GetOrganisationsOfUser(MunityUser user)
        {
            var organisations = from membership in _context.OrganizationMember
                where membership.User.MunityUserId == user.MunityUserId
                join role in _context.OrganizationRoles on membership.Role equals role
                join organisation in _context.Organizations on role.Organization equals organisation
                select organisation;
            return organisations;
        }

        public OrganisationService(MunityContext context)
        {
            _context = context;
        }
    }
}
