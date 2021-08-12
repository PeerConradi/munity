using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNity.Database.Models.Conference;
using MUNity.Database.Models.Organization;
using MUNity.Database.Models.User;
using MUNityCore.Models.User;

namespace MUNity.Services
{
    public interface IOrganisationService
    {
        Organization CreateOrganisation(string name, string abbreviation);

        Task<Organization> GetOrganisation(string id);

        bool CanUserCreateProject(string username, string organisationId);

        Task<Organization> GetOrganisationWithMembers(string id);

        Task<Project> GetProjectWithOrganisation(string id);

        OrganizationRole AddOrganisationRole(Organization organization, string rolename,
            bool canCreateConferences = false);

        IQueryable<OrganizationRole> GetOrganisationRoles(string organisationId);

        IQueryable<Project> GetOrganisationProjects(string organisationId);

        OrganizationMember AddUserToOrganisation(MunityUser user, Organization organization, OrganizationRole role);

        IEnumerable<Organization> GetOrganisationsOfUser(MunityUser user);
    }
}