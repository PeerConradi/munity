using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MUNityAngular.Models.User;
using MUNityAngular.Services;
using MUNityAngular.Util.Extenstions;
using MUNityAngular.DataHandlers.EntityFramework.Models;
using MUNityAngular.Models.Conference;

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
        public ActionResult<int> GetResolutionMongoCount([FromHeader]string auth, [FromServices]AuthService authService,
            [FromServices]ResolutionService resolutionService)
        {
            var validation = authService.ValidateAuthKey(auth);
            if (validation == null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to ask that!");
            }

            var state = authService.IsAdmin(validation.User.UserId);
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
        public ActionResult<int> GetResolutionDatabaseCount([FromHeader]string auth, [FromServices]AuthService authService,
            [FromServices]ResolutionService resolutionService)
        {
            var validation = authService.ValidateAuthKey(auth);
            if (validation == null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to ask that!");
            }

            var state = authService.IsAdmin(validation.User.UserId);
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
        public ActionResult<int> GetUserCount([FromHeader]string auth, [FromServices]AuthService authService)
        {
            var validation = authService.ValidateAuthKey(auth);
            if (validation == null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to ask that!");
            }

            var state = authService.IsAdmin(validation.User.UserId);
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
        public ActionResult<int> GetConferenceCount([FromHeader]string auth, [FromServices]AuthService authService,
            [FromServices]ConferenceService conferenceService)
        {
            var validation = authService.ValidateAuthKey(auth);
            if (validation == null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to ask that!");
            }

            var state = authService.IsAdmin(validation.User.UserId);
            if (state == true)
            {
                // TODO: Implement the count logic
                return StatusCode(StatusCodes.Status200OK, 1);
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
        public ActionResult<List<User>> GetAllUsers([FromHeader]string auth, [FromServices]AuthService authService)
        {
            var validation = authService.ValidateAuthKey(auth);
            if (validation == null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to ask that!");
            }

            var state = authService.IsAdmin(validation.User.UserId);
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
        [HttpPost]
        public IActionResult GiveConferenceAuth([FromHeader]string auth, [FromHeader]string userid, [FromHeader]string conferenceid,
            [FromServices]AuthService authService, [FromServices]ConferenceService conferenceService) 
        {
            var validation = authService.ValidateAuthKey(auth);
            if (validation == null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to ask that!");
            }

            var state = authService.IsAdmin(validation.User.UserId);
            if (state == true)
            {
                throw new NotImplementedException("To be reworked");
                //var conference = conferenceService.GetConference(conferenceid);
                //if (conference == null)
                //    return StatusCode(StatusCodes.Status404NotFound, "Conference not found");

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
        /// <param name="conferenceService"></param>
        /// <returns>403 - Forbidden if the auth is not valid or a list of all existing Users.</returns>
        [Route("[action]")]
        [HttpGet]
        public ActionResult<List<Conference>> GetConferences([FromHeader]string auth, [FromServices]AuthService authService,
            [FromServices]ConferenceService conferenceService)
        {
            var validation = authService.ValidateAuthKey(auth);
            if (validation == null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to ask that!");
            }

            var state = authService.IsAdmin(validation.User.UserId);
            if (state == true)
            {
                throw new NotImplementedException("To be reworked");
                //return StatusCode(StatusCodes.Status200OK, conferenceService.GetAll());
            }
            else
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to ask that!");
            }
        }

        
        [Route("[action]")]
        [HttpGet]
        public IActionResult RestoreResolutions([FromHeader]string auth, [FromServices]AuthService authService,
            [FromServices]ResolutionService resolutionService)
        {
            var validation = authService.ValidateAuthKey(auth);
            if (validation == null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to ask that!");
            }

            var state = authService.IsAdmin(validation.User.UserId);
            if (state == true)
            {
                resolutionService.RestoreToDatabase(validation.User.UserId);
                return StatusCode(StatusCodes.Status200OK);
            }
            else
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to ask that!");
            }
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult PurgeResolutions([FromHeader]string auth, [FromServices]AuthService authService,
            [FromServices]ResolutionService resolutionService)
        {
            var validation = authService.ValidateAuthKey(auth);
            if (validation == null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to ask that!");
            }

            var state = authService.IsAdmin(validation.User.UserId);
            if (state == true)
            {
                resolutionService.PurgeMongoDB();
                return StatusCode(StatusCodes.Status200OK);
            }
            else
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to ask that!");
            }
        }

    }
}