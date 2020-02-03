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
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Route("[action]")]
        public IActionResult Register([FromHeader]string username, [FromHeader]string password, [FromHeader]string email,
            [FromServices]AuthService authService)
        {
            if (!authService.IsRegistrationOpened)
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to create an account. The administrator disabled the registration.");

            var status = authService.Register(username, password, email);
            if (status)
                return StatusCode(StatusCodes.Status200OK);
            else
                return StatusCode(StatusCodes.Status406NotAcceptable, "Account not created!");
        }

        class LoginKey
        {
            public string Key { get; set; }
        }

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

        [HttpGet]
        [Route("[action]")]
        public IActionResult ChangePassword([FromHeader]string auth, [FromHeader]string oldpassword, [FromHeader]string newpassword,
            [FromServices]AuthService authService)
        {
            oldpassword = oldpassword.DecodeUrl();
            newpassword = newpassword.DecodeUrl();

            var authstate = authService.ValidateAuthKey(auth);
            var status = authService.CheckPasswordForUserid(authstate.userid, oldpassword);

            if (status == false)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "The password is not valid!");
            }
            else
            {
                authService.SetPassword(authstate.userid, newpassword);
                authService.DeleteAllUserKeys(authstate.userid);
                var login = authService.LoginWithId(authstate.userid, newpassword);
                return StatusCode(StatusCodes.Status200OK, login);
            }
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Logout([FromHeader]string auth, [FromServices]AuthService service)
        {
            service.Logout(auth);
            return StatusCode(StatusCodes.Status200OK);
        }

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


        [Route("[action]")]
        [HttpGet]
        public IActionResult GetRegistrationState([FromServices]AuthService authService)
        {
            return StatusCode(StatusCodes.Status200OK, authService.IsRegistrationOpened);
        }
    }
}