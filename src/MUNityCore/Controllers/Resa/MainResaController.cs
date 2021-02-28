using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MUNity.Models.Resolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Controllers.Resa
{
    [Route("api/Resa")]
    [ApiController]
    public class MainResaController : ControllerBase
    {
        Services.SqlResolutionService _resolutionService;

        public MainResaController(Services.SqlResolutionService resolutionService)
        {
            this._resolutionService = resolutionService;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<string>> Public()
        {
            var resolutionId = await this._resolutionService.CreatePublicResolutionAsync("Neue Resolution");
            if (string.IsNullOrEmpty(resolutionId))
                return BadRequest();
            return Ok(resolutionId);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<Resolution>> Public(string id)
        {
            var resolution = await this._resolutionService.GetResolutionDtoAsync(id);
            if (resolution == null) return NotFound();
            return Ok(resolution);
        }
    }
}
