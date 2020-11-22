using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MUNityCore.Models.User;
using MUNityCore.Util.Extensions;
using MUNityCore.Models.Conference;
using MUNityCore.Models.Core;
using MUNityCore.Schema.Request;
using MUNityCore.Schema.Response.User;
using MUNityCore.Services;
using User = MUNityCore.Models.Core.User;

namespace MUNityCore.Controllers
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
        private readonly IAuthService _authService;

        private readonly IUserService _userService;

        private readonly IResolutionService _resolutionService;

        /// <summary>
        /// Checks if the user behind the baerer token is an Admin or HeadAdmin.
        /// Returns true if this is the case and false if not. Will always return a Code 200.
        /// </summary>
        /// <returns>true of the user is an admin and false if not!</returns>
        [Route("[action]")]
        [HttpGet]
        [Authorize]
        public ActionResult<bool> CanEnterAdminArea()
        {
            return this._authService.IsUserPrincipalAdmin(User);
        }

        /// <summary>
        /// Creates a new User. Note that this is not the method to register a new user. Take a look into the UserController
        /// to learn more about how to create an account. This method is for Administrators to create a new user without
        /// Age limitation or validation.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="forename"></param>
        /// <param name="lastname"></param>
        /// <param name="password"></param>
        /// <param name="mail"></param>
        /// <param name="birthday"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPut]
        [Authorize]
        public ActionResult<UserInformation> CreateUser(string username, string forename, string lastname, string password, string mail, string birthday)
        {
            if (!this._authService.IsUserPrincipalAdmin(User))
                return Forbid("You are not allowed to do that!");

            DateTime bd = DateTime.Parse(birthday);
            try
            {
                var user = this._userService.CreateUser(username, forename, lastname, password, mail, bd);
                var userInfo = this._userService.GetUserInformation(user.Username);
                return Ok(userInfo);
            }
            catch (Exception e)
            {
                return Problem("User cannot be created. Maybe the username is taken. More information: " + e.Message);
            }
        }

        /// <summary>
        /// This method will delete a user Account and everything that depens on that account.
        /// Be careful with this method. Once the data is deleted it can not be restored.
        /// This API is not Facebook, when deleting something it will be deleted.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates a new UserAuth. A UserAuth can be assigned to different users and give them
        /// access to more API calls.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates a new user auth and returns the new created auth.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        [Authorize]
        public ActionResult<UserAuth> CreateUserAuth([FromBody] AdminSchema.CreateUserAuthBody body)
        {
            if (!_authService.IsUserPrincipalAdmin(User)) return Forbid();

            var auth = this._authService.CreateUserAuth(body);
            return Ok(auth);
        }


        /// <summary>
        /// Sets the Auth of a user with a given username.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="authid"></param>
        /// <returns></returns>
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
        public ActionResult<IEnumerable<User>> GetBannedUsers()
        {
            if (!this._authService.IsUserPrincipalAdmin(User))
                return Forbid();

            return Ok(this._userService.GetBannedUsers());
        }

        /// <summary>
        /// Bans a user by the given username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpDelete]
        [Authorize]
        public async Task<ActionResult<bool>> BanUser(string username)
        {
            if (!this._authService.IsUserPrincipalAdmin(User))
                return Forbid();

            var user = await this._userService.GetUserByUsername(username);
            if (user == null)
                return NotFound("User not found");

            this._userService.BanUser(user);
            return Ok(true);

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
        public ActionResult<IEnumerable<User>> GetUserBlock(int blockid)
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
                return Forbid();

            var res = await this._userService.GetUserCount();
            return Ok(res);
        }

        /// <summary>
        /// Gets the total count of resolution auths that are created. The number can differ from the
        /// real amount of resolutions because the content of resolution is stored inside a mongoDb and not
        /// inside the sql database.
        /// </summary>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<int>> GetResolutionCount()
        {
            if (!this._authService.IsUserPrincipalAdmin(User))
                return Forbid();

            var result = await this._resolutionService.GetResolutionCount();
            return Ok(result);
        }

        /// <summary>
        /// Returns all users that are admin or Head Admins.
        /// </summary>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<User>> GetAdministrators()
        {
            if (this._authService.IsUserPrincipalAdmin(User))
                return Forbid();

            IEnumerable<User> admins = this._userService.GetAdministrators();
            return Ok(admins);
        }

        public AdminController(IAuthService authService, IUserService userService, IResolutionService resolutionService)
        {
            this._authService = authService;
            this._userService = userService;
            this._resolutionService = resolutionService;
        }
    }
}