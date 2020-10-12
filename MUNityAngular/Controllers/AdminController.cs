using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MUNityAngular.Models.User;
using MUNityAngular.Services;
using MUNityAngular.Util.Extenstions;
using MUNityAngular.DataHandlers.EntityFramework.Models;
using MUNityAngular.Models.Conference;
using MUNityAngular.Models.Core;
using User = MUNityAngular.Models.Core.User;

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
        private Services.AuthService _authService;

        private Services.UserService _userService;

        [Route("[action]")]
        [HttpGet]
        [Authorize]
        public ActionResult<bool> CanEnterAdminArea()
        {
            return this._authService.IsUserPrincipalAdmin(User);
        }

        [Route("[action]")]
        [HttpPut]
        [Authorize]
        public ActionResult<User> CreateUser(string username, string forename, string lastname, string password, string mail, string birthday)
        {
            if (!this._authService.IsUserPrincipalAdmin(User))
                return Forbid("You are not allowed to do that!");

            DateTime bd = DateTime.Parse(birthday);
            try
            {
                var user = this._userService.CreateUser(username, forename, lastname, password, mail, bd);
                return user;
            }
            catch (Exception e)
            {
                return Problem("User cannot be created. Maybe the username is taken. More information: " + e.Message);
            }
        }

        [Route("[action]")]
        [HttpPatch]
        [Authorize]
        public async Task<ActionResult<bool>> DeleteUser(string username)
        {
            if (!this._authService.IsUserPrincipalAdmin(User))
                return Forbid("You are not allowed to do that!");

            var user = await this._userService.GetUserByUsername(username);
            if (user == null) return NotFound("User with the given Username not found!");

            this._userService.RemoveUser(user);
            return true;
        }

        [Route("[action]")]
        [HttpPost]
        [Authorize]
        public ActionResult<UserAuth> CreateAuth(string name)
        {
            if (!this._authService.IsUserPrincipalAdmin(User))
                return Forbid("You are not allowed to do that!");

            UserAuth auth = this._authService.CreateAuth(name);
            return auth;
        }

        [Route("[action]")]
        [HttpPatch]
        [Authorize]
        public async Task<ActionResult<bool>> SetUserAuth(string username, int authid)
        {
            if (!this._authService.IsUserPrincipalAdmin(User))
                return Forbid("You are not allowed to do that!");

            var user = await this._userService.GetUserByUsername(username);
            if (user == null) return NotFound("User not found!");

            var auth = await this._authService.GetAuth(authid);
            if (auth == null) return NotFound("Auth not found!");

            await this._authService.SetUserAuth(user, auth);
            return true;
        }

        /// <summary>
        /// Returns a list of banned users. You need to be an administrator to perform this request.
        /// </summary>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        [Authorize]
        [Authorize]
        public ActionResult<IEnumerable<User>> GetBannedUsers()
        {
            if (!this._authService.IsUserPrincipalAdmin(User))
                return Forbid("You are not allowed to show that. You need to be Admin!");

            return Ok(this._userService.GetBannedUsers());
        }

        /// <summary>
        /// Returns a user block of 100 Users ordered by their Lastname.
        /// You need to be registered as admin to perform this request.
        /// </summary>
        /// <param name="blockid">The Block id. 0 will give the first 100 users ordered by their lastname.</param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<User>> GetUserblock(int blockid)
        {
            if (!this._authService.IsUserPrincipalAdmin(User))
                return Forbid("You are not allowed to show that. You need to be Admin!");

            return Ok(this._userService.GetUserBlock(blockid));
        }

        /// <summary>
        /// Returns the amount of users that are registered on the site.
        /// You need to be logged in as admin to use this request.
        /// </summary>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<int>> GetUserCount()
        {
            if (!this._authService.IsUserPrincipalAdmin(User))
                return Forbid("You are not allowed to show that. You need to be Admin!");

            var res = await this._userService.GetUserCount();
            return Ok(res);
        }

        public AdminController(Services.AuthService authService, UserService userService)
        {
            this._authService = authService;
            this._userService = userService;
        }
    }
}