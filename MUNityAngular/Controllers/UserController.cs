using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MUNityAngular.Models.User;
using MUNityAngular.Schema.Request;
using MUNityAngular.Services;
using MUNityAngular.Util.Extenstions;

namespace MUNityAngular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="authService"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public IActionResult Register([FromBody]RegistrationModel model,
            [FromServices]AuthService authService)
        {
            if (!authService.IsRegistrationOpened)
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to create an account. The administrator disabled the registration.");

            var status = authService.Register(model.Username, model.Password, model.Mail);
            if (status)
                return StatusCode(StatusCodes.Status200OK);
            else
                return StatusCode(StatusCodes.Status406NotAcceptable, "Account not created!");
        }

        class LoginKey
        {
            public string Key { get; set; }
        }

        /// <summary>
        /// Logs in the user and will return a session key for this user.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="authService"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public IActionResult Login([FromHeader]string username, [FromHeader]string password,
            [FromServices]AuthService authService)
        {
            var status = authService.Login(username, password);
            if (status.status)
            {
                var key = new LoginKey() { Key = status.key };
                return StatusCode(StatusCodes.Status200OK, key);
            }
            else
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, "Invalid login");
            }
        }

        /// <summary>
        /// Changes the password of the given user
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="oldpassword"></param>
        /// <param name="newpassword"></param>
        /// <param name="authService"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public IActionResult ChangePassword([FromHeader]string auth, [FromBody]ChangePasswordRequest request,
            [FromServices]AuthService authService)
        {

            var authstate = authService.ValidateAuthKey(auth);
            var status = authService.CheckPasswordForUserid(authstate.userid, request.OldPassword);

            if (status == false)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "The password is not valid!");
            }
            else
            {
                authService.SetPassword(authstate.userid, request.NewPassword);
                authService.DeleteAllUserKeys(authstate.userid);
                var login = authService.LoginWithId(authstate.userid, request.NewPassword);
                return StatusCode(StatusCodes.Status200OK, login);
            }
        }


        /// <summary>
        /// Logs out the user and deletes the auth key that is used to do this.
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public IActionResult Logout([FromHeader]string auth, [FromServices]AuthService service)
        {
            service.Logout(auth);
            return StatusCode(StatusCodes.Status200OK);
        }

        /// <summary>
        /// Validates the given Auth Key and returns if its valid (true) or not (false)
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="authService"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public IActionResult ValidateKey([FromHeader]string auth, [FromServices]AuthService authService)
        {
            var validation = authService.ValidateAuthKey(auth);
            if (validation.valid == true)
            {
                return StatusCode(StatusCodes.Status200OK);
            } else
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
        }

        /// <summary>
        /// Checks if the user behind the given Auth key has admin rights.
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="authService"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public IActionResult IsAdmin([FromHeader]string auth, [FromServices]AuthService authService)
        {
            var authState = authService.ValidateAuthKey(auth);
            if (authState.valid == false)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to ask that!");
            }

            var state = authService.IsAdmin(authState.userid);
            if (state == true)
            {
                return StatusCode(StatusCodes.Status200OK, true);
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, false);
            }
        }

        /// <summary>
        /// Checks if the username is available: true -> username taken, false -> username available
        /// </summary>
        /// <param name="username"></param>
        /// <param name="authService"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public IActionResult CheckUsername(string username, [FromServices]AuthService authService)
        {
            if (string.IsNullOrWhiteSpace(username))
                return StatusCode(StatusCodes.Status406NotAcceptable, "The username cannot be empty.");
            var result = !authService.UsernameAvailable(username);
            return StatusCode(StatusCodes.Status200OK, result);
        }

        /// <summary>
        /// Checks if the registration is opened or closed.
        /// </summary>
        /// <param name="authService"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        public IActionResult GetRegistrationState([FromServices]AuthService authService)
        {
            return StatusCode(StatusCodes.Status200OK, authService.IsRegistrationOpened);
        }

        /// <summary>
        /// To protect data from robots you can only get a user when you are logged
        /// in.
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="username"></param>
        /// <param name="authService"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        public IActionResult GetUserByUsername(
            [FromHeader]string auth,
            [FromHeader]string username,
            [FromServices]AuthService authService)
        {
            var validCheck = authService.ValidateAuthKey(auth);

            if (validCheck.valid == false)
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that");

            var user = authService.GetUserByUsername(username);

            if (user == null)
                return StatusCode(StatusCodes.Status404NotFound, "User with the given Username not found");

            return StatusCode(StatusCodes.Status200OK, user.AsNewtonsoftJson());
        }
    }
}