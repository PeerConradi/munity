using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MUNityAngular.Models.User;
using MUNityAngular.Schema.Request;
using MUNityAngular.Services;
using MUNityAngular.Util.Extenstions;
using MUNityAngular.DataHandlers.EntityFramework.Models;
using MUNityAngular.Models.Core;
using MUNityAngular.Models.Facades;
using MUNityAngular.Schema.Request.Authentication;
using MUNityAngular.Schema.Response.Authentication;

namespace MUNityAngular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        /// <summary>
        /// Get the basic information of a user.
        /// This function can only used when authorized with a baerer token.
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        [Authorize]
        public async Task<IUserInformation> GetUser([FromServices]IUserService userService, string username)
        {
            return await userService.GetUserByUsername(username);
        }

        /// <summary>
        /// Registers a new User.
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="body">a RegisterRequest Body</param>
        /// <returns>The model of the created user himself with all importand informations.</returns>
        [Route("[action]")]
        [HttpPost]
        [AllowAnonymous]
        public ActionResult<User> Register([FromServices]IUserService userService, [FromBody]RegisterRequest body)
        {
            try
            {
                var user =
                     userService.CreateUser(body.Username, body.Forename, body.Lastname, body.Password, body.Mail, body.Birthday);
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("[action]")]
        [HttpPost]
        [AllowAnonymous]
        public AuthenticationResponse Login([FromServices]IAuthService authService, [FromBody]AuthenticateRequest request)
        {
            return authService.Authenticate(request);
        }

        [Route("[action]")]
        [HttpGet]
        [Authorize]
        public ActionResult JustForVIP()
        {
            return Ok("Hallo du bist wohl wichtig!");
        }

        /// <summary>
        /// Whit this call you are able to update your user options.
        /// Note that only the Forname, Lastname, Gender, City, Street,
        /// Housenumber and Zipcode will be changed. You are not able
        /// to change your age, password or username with this function!
        /// This function will only work for the authorized user and change the
        /// settings of this user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userService"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPatch]
        [Authorize]
        public async Task<int> UpdateMe([FromBody]User user, [FromServices]IUserService userService)
        {
            var username = User.Claims.FirstOrDefault(n => n.Type == ClaimTypes.Name)?.Value ?? "";
            var dbUser = await userService.GetUserByUsername(username);
            dbUser.Forename = user.Forename;
            dbUser.Lastname = user.Lastname;
            
            dbUser.Gender = user.Gender;
            dbUser.City = user.City;
            dbUser.Street = user.Street;
            dbUser.Housenumber = user.Housenumber;
            dbUser.Zipcode = user.Zipcode;

            return await userService.UpdateUser(dbUser);
        }

        [Route("[action]")]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<User>> WhoAmI([FromServices]IUserService service)
        {

            var username = User.Claims.FirstOrDefault(n => n.Type == ClaimTypes.Name)?.Value ?? "";
            var result = await service.GetUserByUsername(username);
            if (result != null)
                return Ok(result);

            return Forbid("A user has no name.");
        }

        [Route("[action]")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<bool> CheckUsername(string username,
            [FromServices]IUserService userService)
        {
            return await userService.CheckUsername(username);
        }

        [Route("[action]")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<bool> CheckMail(string mail,
            [FromServices] IUserService userService)
        {
            return await userService.CheckMail(mail);
        }
    }
}