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

    /// <summary>
    /// The AdminController handles every request were Admin-Auth is requiered.
    /// This controller allows the user to change any type of conference, manage Users and documents
    /// aswell as turning the gloabal settings on or off.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {

        /// <summary>
        /// Returns a number (integer) of resolutions that are created inside the MongoDB. The count of these resolutions
        /// may differ from the one that are stored inside the MySQL Database.
        /// </summary>
        /// <param name="auth">The Authentication Key. This authkey must be valid for an administrator</param>
        /// <param name="authService"></param>
        /// <param name="resolutionService"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns the number of resolutions inside the MySQL Database. This number may differs from the amount of
        /// resolutions that exist inside the MongoDB.
        /// </summary>
        /// <param name="auth">A valid auth key that has admin previleges</param>
        /// <param name="authService"></param>
        /// <param name="resolutionService"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns a list of all users
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="authService"></param>
        /// <returns>403 - Forbidden if the auth is not valid or a list of all existing Users.</returns>
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
                return StatusCode(StatusCodes.Status200OK, authService.GetAllUsers());
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