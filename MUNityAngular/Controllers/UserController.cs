﻿using System;
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
        public IActionResult Logout([FromHeader]string auth, [FromServices]AuthService service)
        {
            service.Logout(auth);
            return StatusCode(StatusCodes.Status200OK);
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult GetRegistrationState([FromServices]AuthService authService)
        {
            return StatusCode(StatusCodes.Status200OK, authService.IsRegistrationOpened);
        }
    }
}