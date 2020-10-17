using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.Models.Core;
using MUNityAngular.Models.Organisation;

namespace MUNityAngular.Services
{
    public interface IOrganisationService
    {
        Organisation CreateOrganisation(string name, string abbreviation);

        Task<Organisation> GetOrganisation(string id);

        OrganisationRole AddOrganisationRole(Organisation organisation, string rolename,
            bool canCreateConferences = false);

        IQueryable<OrganisationRole> GetOrganisationRoles(string organisationId);

        OrganisationMember AddUserToOrganisation(User user, Organisation organisation, OrganisationRole role);

        IEnumerable<Organisation> GetOrganisationsOfUser(User user);
    }
}