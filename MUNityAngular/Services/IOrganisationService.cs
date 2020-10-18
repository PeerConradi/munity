using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.Models.Conference;
using MUNityAngular.Models.Core;
using MUNityAngular.Models.Organisation;

namespace MUNityAngular.Services
{
    public interface IOrganisationService
    {
        Organisation CreateOrganisation(string name, string abbreviation);

        Task<Organisation> GetOrganisation(string id);

        bool CanUserCreateProject(string username, string organisationId);

        Task<Organisation> GetOrganisationWithMembers(string id);

        Task<Project> GetProjectWithOrganisation(string id);

        OrganisationRole AddOrganisationRole(Organisation organisation, string rolename,
            bool canCreateConferences = false);

        IQueryable<OrganisationRole> GetOrganisationRoles(string organisationId);

        IQueryable<Project> GetOrganisationProjects(string organisationId);

        OrganisationMember AddUserToOrganisation(User user, Organisation organisation, OrganisationRole role);

        IEnumerable<Organisation> GetOrganisationsOfUser(User user);
    }
}