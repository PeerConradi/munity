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
        [HttpGet]
        [Route("[action]")]
        public Task<Organisation> GetOrganisationInfo([FromServices]IOrganisationService service, string id)
        {
            return service.GetOrganisation(id);
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize]
        public Organisation CreateOrganisation([FromServices] IOrganisationService service,
            [FromBody] CreateOrganisationRequest body)
        {
            return service.CreateOrganisation(body.OrganisationName, body.Abbreviation);
        }


    }
}
