using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MUNityCore.Models.Organization;
using MUNityCore.Schema.Request.Organisation;
using MUNityCore.Services;

namespace MUNityCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganisationController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IOrganisationService _organisationService;

        [HttpGet]
        [Route("[action]")]
        public Task<Organization> GetOrganisation([FromServices]IOrganisationService service, string id)
        {
            return service.GetOrganisation(id);
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize]
        public ActionResult<Organization> CreateOrganisation([FromServices] IOrganisationService service,
            [FromServices]IAuthService authService,
            [FromBody] CreateOrganisationRequest body)
        {
            // TODO: Needs to check of the user is allowed to create an organization
            var user = authService.GetUserOfClaimPrincipal(User);
            if (user == null)
                return Forbid();

            var organisation = service.CreateOrganisation(body.OrganisationName, body.Abbreviation);

            if (organisation == null)
                return Problem("Organization cannot be created. Unknown source of error.");

            // Create the Owner role
            var ownerRole = service.AddOrganisationRole(organisation, "Owner", true);
            if (ownerRole == null)
                return Problem($"Unable to create an owner role. But the organization was created with id {organisation.OrganizationId}");

            var membership = service.AddUserToOrganisation(user, organisation, ownerRole);
            if (membership == null)
                return Problem("The organization and owner role was created. But you were not assigned as the owner.");

            return Ok(organisation);
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize]
        public ActionResult<IEnumerable<Organization>> GetMyOrganisations()
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
