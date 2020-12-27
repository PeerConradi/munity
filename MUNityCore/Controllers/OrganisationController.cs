using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MUNityCore.Services;
using MUNitySchema.Schema.Organization;

namespace MUNityCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganisationController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IOrganisationService _organisationService;

        /// <summary>
        /// Gets the information of an organization
        /// </summary>
        /// <param name="service"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<OrganizationInformation>> GetOrganisation([FromServices]IOrganisationService service, string id)
        {
            return Ok(await service.GetOrganisation(id));
        }

        /// <summary>
        /// Creates a new organization
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        [Authorize]
        public ActionResult<OrganizationInformation> CreateOrganisation([FromBody]MUNitySchema.Schema.Organization.CreateOrganizationRequest body)
        {
            // TODO: Needs to check of the user is allowed to create an organization
            var user = _authService.GetUserOfClaimPrincipal(User);
            if (user == null)
                return Forbid();

            var organisation = _organisationService.CreateOrganisation(body.OrganisationName, body.Abbreviation);

            if (organisation == null)
                return Problem("Organization cannot be created. Unknown source of error.");

            // Create the Owner role
            var ownerRole = _organisationService.AddOrganisationRole(organisation, "Owner", true);
            if (ownerRole == null)
                return Problem($"Unable to create an owner role. But the organization was created with id {organisation.OrganizationId}");

            var membership = _organisationService.AddUserToOrganisation(user, organisation, ownerRole);
            if (membership == null)
                return Problem("The organization and owner role was created. But you were not assigned as the owner.");

            return Ok(organisation);
        }

        /// <summary>
        /// Returns the organizations of the logged in user.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        [Authorize]
        public ActionResult<IEnumerable<OrganizationInformation>> GetMyOrganisations()
        {
            var user = _authService.GetUserOfClaimPrincipal(User);
            if (user == null)
                return Forbid("You are not a valid user.");

            var result = _organisationService.GetOrganisationsOfUser(user);
            return Ok(result);
        }

        public OrganisationController(IAuthService authService, IOrganisationService organisationService)
        {
            this._authService = authService;
            this._organisationService = organisationService;
        }


    }
}
