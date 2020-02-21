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
            var (valid, userid) = authService.ValidateAuthKey(auth);
            if (valid == false)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to ask that!");
            }

            var state = authService.IsAdmin(userid);
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
            var (valid, userid) = authService.ValidateAuthKey(auth);
            if (valid == false)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to ask that!");
            }

            var state = authService.IsAdmin(userid);
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
        public IActionResult GetUserCount([FromHeader]string auth, [FromServices]AuthService authService)
        {
            var (valid, userid) = authService.ValidateAuthKey(auth);
            if (valid == false)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to ask that!");
            }

            var state = authService.IsAdmin(userid);
            if (state == true)
            {
                return StatusCode(StatusCodes.Status200OK, authService.GetUserCount());
            }
            else
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to ask that!");
            }
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult GetConferenceCount([FromHeader]string auth, [FromServices]AuthService authService,
            [FromServices]ConferenceService conferenceService)
        {
            var (valid, userid) = authService.ValidateAuthKey(auth);
            if (valid == false)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to ask that!");
            }

            var state = authService.IsAdmin(userid);
            if (state == true)
            {
                return StatusCode(StatusCodes.Status200OK, conferenceService.GetConferenceCount());
            }
            else
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to ask that!");
            }
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult GetConferenceCacheCount([FromHeader]string auth, [FromServices]AuthService authService,
            [FromServices]ConferenceService conferenceService)
        {
            var (valid, userid) = authService.ValidateAuthKey(auth);
            if (valid == false)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to ask that!");
            }

            var state = authService.IsAdmin(userid);
            if (state == true)
            {
                return StatusCode(StatusCodes.Status200OK, conferenceService.GetConferenceCacheCount());
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
            var (valid, userid) = authService.ValidateAuthKey(auth);
            if (valid == false)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to ask that!");
            }

            var state = authService.IsAdmin(userid);
            if (state == true)
            {
                return StatusCode(StatusCodes.Status200OK, authService.GetAllUsers().AsNewtonsoftJson());
            }
            else
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to ask that!");
            }
        }

        [Route("[action]")]
        [HttpPost]
        public IActionResult GiveConferenceAuth([FromHeader]string auth, [FromHeader]string userid, [FromHeader]string conferenceid,
            [FromServices]AuthService authService, [FromServices]ConferenceService conferenceService) 
        {
            var authState = authService.ValidateAuthKey(auth);
            if (authState.valid == false)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to ask that!");
            }

            var state = authService.IsAdmin(authState.userid);
            if (state == true)
            {
                var conference = conferenceService.GetConference(conferenceid);
                if (conference == null)
                    return StatusCode(StatusCodes.Status404NotFound, "Conference not found");

                throw new NotImplementedException("Come back to this!");
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
        public IActionResult GetConferences([FromHeader]string auth, [FromServices]AuthService authService,
            [FromServices]ConferenceService conferenceService)
        {
            var (valid, userid) = authService.ValidateAuthKey(auth);
            if (valid == false)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to ask that!");
            }

            var state = authService.IsAdmin(userid);
            if (state == true)
            {
                return StatusCode(StatusCodes.Status200OK, conferenceService.GetAll().AsNewtonsoftJson());
            }
            else
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to ask that!");
            }
        }

        /// <summary>
        /// Removes an account from the Database and also all the things connected to this account.
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="id"></param>
        /// <param name="authService"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        public IActionResult DeleteAccount ([FromHeader]string auth, [FromHeader]string id, 
            [FromServices]AuthService authService)
        {
            var (valid, userid) = authService.ValidateAuthKey(auth);
            if (valid == false)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to ask that!");
            }

            var state = authService.IsAdmin(userid);
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