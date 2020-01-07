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
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Route("[action]")]
        public IActionResult Register([FromHeader]string username, [FromHeader]string password, [FromHeader]string email,
            [FromServices]AuthService authService)
        {
            var status = authService.Register(username, password, email);
            if (status)
                return StatusCode(StatusCodes.Status200OK);
            else
                return StatusCode(StatusCodes.Status406NotAcceptable, "Account not created!");
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Login([FromHeader]string username, [FromHeader]string password,
            [FromServices]AuthService authService)
        {
            var status = authService.Login(username, password);
            if (status.status)
            {
                return StatusCode(StatusCodes.Status200OK, status.key);
            }
            else
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, "Invalid login");
            }
        }
    }
}