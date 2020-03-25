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
using MUNityAngular.DataHandlers.EntityFramework.Models;
using MUNityAngular.Models.Facades;

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

        public class LoginKey
        {
            public string Key { get; set; }

            public User User { get; set; }
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
        public ActionResult<LoginKey> Login([FromHeader]string username, [FromHeader]string password,
            [FromServices]AuthService authService)
        {
            var status = authService.Login(username, password);
            if (status.status)
            {
                var key = new LoginKey() { Key = status.key };
                key.User = authService.GetUserByUsername(username);
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
        /// <param name="request"></param>
        /// <param name="authService"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public IActionResult ChangePassword([FromHeader]string auth, [FromBody]ChangePasswordRequest request,
            [FromServices]AuthService authService)
        {

            var authstate = authService.ValidateAuthKey(auth);
            if (authstate.valid == false)
                return StatusCode(StatusCodes.Status403Forbidden, "You are not logged in or the auth is invalid");

            var status = authService.CheckPasswordForUserid(authstate.userid.Value, request.OldPassword);

            if (status == false)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "The password is not valid!");
            }
            else
            {
                authService.SetPassword(authstate.userid.Value, request.NewPassword);
                authService.DeleteAllUserKeys(authstate.userid.Value);
                var login = authService.LoginWithId(authstate.userid.Value, request.NewPassword);
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

        [Route("[action]")]
        [HttpGet]
        public ActionResult<User> GetAuthUser([FromHeader]string auth, [FromServices]AuthService authService)
        {
            var validation = authService.ValidateAuthKey(auth);
            if (validation.valid == true)
            {
                return StatusCode(StatusCodes.Status200OK, authService.GetUserById(validation.userid.Value));
            }
            else
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
        }

        [Route("[action]")]
        [HttpPatch]
        public ActionResult<User> UpdateUserinfo([FromHeader]string auth, [FromBody]IUserFacade userModel, [FromServices]AuthService authService)
        {
            var validation = authService.ValidateAuthKey(auth);
            var userOfAuth = authService.GetUserByAuth(auth);
            if (validation.valid == true && userModel.Username == userOfAuth.Username)
            {
                authService.SetForename(userOfAuth.UserId, userModel.Forename);
                authService.SetLastname(userOfAuth.UserId, userModel.Lastname);
                return StatusCode(StatusCodes.Status200OK, userOfAuth);
            }
            else
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
        public ActionResult<bool> IsAdmin([FromHeader]string auth, [FromServices]AuthService authService)
        {
            var authState = authService.ValidateAuthKey(auth);
            if (authState.valid == false)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to ask that!");
            }

            var state = authService.IsAdmin(authState.userid.Value);
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
        public ActionResult<bool> CheckUsername(string username, [FromServices]AuthService authService)
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
        public ActionResult<bool> GetRegistrationState([FromServices]AuthService authService)
        {
            return StatusCode(StatusCodes.Status200OK, authService.IsRegistrationOpened);
        }


        /// <summary>
        /// Returns the rights of the given user!
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="authService"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        public ActionResult<List<UserAuths>> GetKeyAuths([FromHeader]string auth, [FromServices]AuthService authService)
        {
            var authstate = authService.GetAuthsByAuthkey(auth);
            return StatusCode(StatusCodes.Status200OK, authstate);
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
        public ActionResult<IUserFacade> GetUserByUsername(
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

            return StatusCode(StatusCodes.Status200OK, user as IUserFacade);
        }

        [Route("[action]")]
        [HttpGet]
        public ActionResult<User> GetUserOfAuth([FromHeader]string auth,
            [FromServices]AuthService authService)
        {
            var validation = authService.ValidateAuthKey(auth);
            if (validation.valid == false)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "The Authcode is not valid!");
            }
            var user = authService.GetUserById(validation.userid.Value);
            return StatusCode(StatusCodes.Status200OK, user as IUserFacade);
        }
    }
}