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

        /// <summary>
        /// Starts the installation of the munity Core.
        /// This function serves no purpose at the moment.
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        public IActionResult Install([FromServices]InstallationService service)
        {

            service.Install();
            return StatusCode(StatusCodes.Status200OK, service.InstallationLog);
        }
    }
}