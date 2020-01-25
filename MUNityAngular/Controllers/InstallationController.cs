using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MUNityAngular.Services;

namespace MUNityAngular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstallationController : ControllerBase
    {
        [Route("[action]")]
        [HttpGet]
        public IActionResult Install([FromServices]InstallationService service)
        {
            if (service.IsInstalled)
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to install anything.");

            service.Install();
            return StatusCode(StatusCodes.Status200OK, service.InstallationLog);
        }
    }
}