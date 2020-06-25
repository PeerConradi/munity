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
        [Route("[action]")]
        [HttpGet]
        public async Task<User> GetUser([FromServices]UserService userService, string username)
        {
            return await userService.GetUserByUsername(username);
        }

        [Route("[action]")]
        [HttpPost]
        [AllowAnonymous]
        public ActionResult<User> Register([FromServices] UserService userService, [FromBody]RegisterRequest body)
        {
            try
            {
                return userService.CreateUser(body.Username, body.Password, body.Mail, body.Birthday);
            }
            catch (Exception e)
            {
                return BadRequest("Invalid username, password or to young!");
            }
        }

        [Route("[action]")]
        [HttpPost]
        [AllowAnonymous]
        public AuthenticationResponse Login([FromServices] AuthService authService, [FromBody]AuthenticateRequest request)
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

        [Route("[action]")]
        [HttpGet]
        [Authorize]
        public async Task<User> WhoAmI([FromServices]UserService service)
        {
            var username = User.Claims.FirstOrDefault(n => n.Type == ClaimTypes.Name)?.Value ?? "";
            return await service.GetUserByUsername(username);
        }
    }
}