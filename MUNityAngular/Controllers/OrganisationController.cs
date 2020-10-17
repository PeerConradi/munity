using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MUNityAngular.Models.Organisation;
using MUNityAngular.Schema.Request.Organisation;
using MUNityAngular.Services;

namespace MUNityAngular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganisationController : ControllerBase
    {
        private IAuthService _authService;
        private IOrganisationService _organisationService;

        [HttpGet]
        [Route("[action]")]
        public Task<Organisation> GetOrganisation([FromServices]IOrganisationService service, string id)
        {
            return service.GetOrganisation(id);
        }

        [HttpPost]
        [Route("[action]")]

        // While in Testing phase you dont need to be logged in to create an Organisation!
        [Authorize]
        public ActionResult<Organisation> CreateOrganisation([FromServices] IOrganisationService service,
            [FromServices]IAuthService authService,
            [FromBody] CreateOrganisationRequest body)
        {
            // TODO: Needs to check of the user is allowed to create an organisation
            var user = authService.GetUserOfClaimPrincipal(User);
            if (user == null)
                Forbid("You are not allowed to create an organisation!");

            var organisation = service.CreateOrganisation(body.OrganisationName, body.Abbreviation);

            if (organisation == null)
                return Problem("Organisation cannot be created. Unknown source of error.");

            // Create the Owner role
            var ownerRole = service.AddOrganisationRole(organisation, "Owner", true);
            if (ownerRole == null)
                return Problem($"Unable to create an owner role. But the organisation was created with id {organisation.OrganisationId}");

            var membership = service.AddUserToOrganisation(user, organisation, ownerRole);
            if (membership == null)
                return Problem("The organisation and owner role was created. But you were not assigned as the owner.");

            return Ok(organisation);
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize]
        public ActionResult<IEnumerable<Organisation>> GetMyOrganisations()
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
