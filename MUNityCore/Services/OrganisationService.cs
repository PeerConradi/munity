using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MUNityCore.DataHandlers.EntityFramework;
using MUNityCore.Models.Conference;
using MUNityCore.Models.Core;
using MUNityCore.Models.Organisation;

namespace MUNityCore.Services
{
    public class OrganisationService : IOrganisationService
    {
        private MunCoreContext _context;

        public Organisation CreateOrganisation([NotNull]string name, [NotNull]string abbreviation)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(abbreviation))
                return null;

            var organisation = new Organisation();

            organisation.OrganisationId = Guid.NewGuid().ToString();
            var shortAsKey = Util.Tools.IdGenerator.AsPrimaryKey(abbreviation);
            if (!_context.Organisations.Any(n => n.OrganisationId == shortAsKey))
                organisation.OrganisationId = shortAsKey;

            organisation.OrganisationName = name;
            organisation.OrganisationAbbreviation = abbreviation;
            _context.Organisations.Add(organisation);
            _context.SaveChanges();
            return organisation;
        }

        public Task<Organisation> GetOrganisation(string id)
        {
            return _context.Organisations.FirstOrDefaultAsync(n => n.OrganisationId == id);
        }

        public Task<Organisation> GetOrganisationWithMembers(string id)
        {
            return _context.Organisations.Include(n => n.Member)
                .ThenInclude(n => n.User)
                .FirstOrDefaultAsync(n => n.OrganisationId == id);
        }

        public Task<Project> GetProjectWithOrganisation(string id)
        {
            return _context.Projects
                .Include(n => n.ProjectOrganisation)
                .FirstOrDefaultAsync(n => n.ProjectId == id);
        }

        public bool CanUserCreateProject(string username, string organisationId)
        {
            var result = _context.OrganisationMember.Any(n => n.User.Username == username &&
                                                 n.Organisation.OrganisationId == organisationId &&
                                                 n.Role.CanCreateProject == true);
            return result;
        }

        public IQueryable<Project> GetOrganisationProjects(string organisationId) =>
            this._context.Projects.Where(n => n.ProjectOrganisation.OrganisationId == organisationId);

        public OrganisationRole AddOrganisationRole(Organisation organisation, string rolename, bool canCreateConferences = false)
        {
            var role = new OrganisationRole();
            role.RoleName = rolename;
            role.CanCreateProject = canCreateConferences;

            role.Organisation = organisation;
            _context.OrganisationRoles.Add(role);
            _context.SaveChanges();

            return role;
        }

        public IQueryable<OrganisationRole> GetOrganisationRoles(string organisationId)
        {
            return _context.OrganisationRoles.Where(n => n.Organisation.OrganisationId == organisationId);
        }

        public OrganisationMember AddUserToOrganisation(User user, Organisation organisation, OrganisationRole role)
        {
            var membership = new OrganisationMember();
            membership.User = user;
            membership.Role = role;
            membership.Organisation = organisation;

            _context.OrganisationMember.Add(membership);
            _context.SaveChanges();

            return membership;
        }

        public IEnumerable<Organisation> GetOrganisationsOfUser(User user)
        {
            var organisations = from membership in _context.OrganisationMember
                where membership.User.UserId == user.UserId
                join role in _context.OrganisationRoles on membership.Role equals role
                join organisation in _context.Organisations on role.Organisation equals organisation
                select organisation;
            return organisations;
        }

        public OrganisationService(MunCoreContext context)
        {
            _context = context;
        }
    }
}
