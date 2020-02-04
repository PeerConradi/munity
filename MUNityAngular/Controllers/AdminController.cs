using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MUNityAngular.Services;
using MUNityAngular.Util.Extenstions;

namespace MUNityAngular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        [Route("[action]")]
        [HttpGet]
        public IActionResult GetResolutionMongoCount([FromHeader]string auth, [FromServices]AuthService authService,
            [FromServices]ResolutionService resolutionService)
        {
            var authState = authService.ValidateAuthKey(auth);
            if (authState.valid == false)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to ask that!");
            }

            var state = authService.IsAdmin(authState.userid);
            if (state == true)
            {
                return StatusCode(StatusCodes.Status200OK, resolutionService.SavedResolutionsCount);
            }
            else
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to ask that!");
            }
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult GetResolutionDatabaseCount([FromHeader]string auth, [FromServices]AuthService authService,
            [FromServices]ResolutionService resolutionService)
        {
            var authState = authService.ValidateAuthKey(auth);
            if (authState.valid == false)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to ask that!");
            }

            var state = authService.IsAdmin(authState.userid);
            if (state == true)
            {
                return StatusCode(StatusCodes.Status200OK, resolutionService.DatabaseResolutionsCount);
            }
            else
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to ask that!");
            }
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult GetAllUsers([FromHeader]string auth, [FromServices]AuthService authService)
        {
            var authState = authService.ValidateAuthKey(auth);
            if (authState.valid == false)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to ask that!");
            }

            var state = authService.IsAdmin(authState.userid);
            if (state == true)
            {
                return StatusCode(StatusCodes.Status200OK, authService.GetAllUsers().ToNewtonsoftJson());
            }
            else
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to ask that!");
            }
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult DeleteAccount ([FromHeader]string auth, [FromHeader]string id, 
            [FromServices]AuthService authService)
        {
            var authState = authService.ValidateAuthKey(auth);
            if (authState.valid == false)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to ask that!");
            }

            var state = authService.IsAdmin(authState.userid);
            if (state == true)
            {
                return StatusCode(StatusCodes.Status200OK, authService.GetAllUsers().ToNewtonsoftJson());
            }
            else
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to ask that!");
            }
        }

    }
}