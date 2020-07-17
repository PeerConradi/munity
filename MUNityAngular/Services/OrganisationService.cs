using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MUNityAngular.DataHandlers.EntityFramework;
using MUNityAngular.Models.Core;
using MUNityAngular.Models.Organisation;

namespace MUNityAngular.Services
{
    public class OrganisationService : IOrganisationService
    {
        private MunCoreContext _context;

        public Organisation CreateOrganisation(string name, string abbreviation)
        {
            var organisation = new Organisation();

            organisation.OrganisationId = Guid.NewGuid().ToString();
            if (!_context.Organisations.Any(n => n.OrganisationId == abbreviation))
                organisation.OrganisationId = abbreviation;

            organisation.OrganisationName = name;
            organisation.OrganisationAbbreviation = abbreviation;
            _context.Organisations.Add(organisation);
            _context.SaveChangesAsync();
            return organisation;
        }

        public Task<Organisation> GetOrganisation(string id)
        {
            return _context.Organisations.FirstOrDefaultAsync(n => n.OrganisationId == id);
        }

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

        public OrganisationService(MunCoreContext context)
        {
            _context = context;
        }
    }
}
