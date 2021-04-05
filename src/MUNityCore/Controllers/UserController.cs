using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MUNityCore.Models.User;
using MUNityCore.Services;
using MUNity.Schema.User;
using MUNity.Schema.Authentication;
using Microsoft.AspNetCore.Cors;

namespace MUNityCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IAuthService _authService;

        private readonly IUserService _userService;

        /// <summary>
        /// Get the basic information of a user.
        /// This function can only used when authorized with a baerer token.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        [Authorize]
        public async Task<UserInformation> GetUser(string username)
        {
            return await _userService.GetUserInformation(username);
        }

        /// <summary>
        /// Registers a new User.
        /// </summary>
        /// <param name="body">a RegisterRequest Body</param>
        /// <returns>200 OK when registration was successful else it will return a BadRequest exception.</returns>
        [Route("[action]")]
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register([FromBody]RegisterRequest body)
        {
            try
            {
                var user =
                     _userService.CreateUser(body.Username, body.Forename, body.Lastname, body.Password, body.Mail, body.Birthday);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("[action]")]
        [HttpPost]
        [AllowAnonymous]
        public ActionResult<AuthenticationResponse> Login([FromBody]AuthenticateRequest request)
        {
            var result = _authService.Authenticate(request);
            if (result != null) return Ok(result);
            return BadRequest();
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
        /// <returns></returns>
        [Route("[action]")]
        [HttpPatch]
        [Authorize]
        public async Task<int> UpdateMe([FromBody]MunityUser user)
        {
            var username = User.Claims.FirstOrDefault(n => n.Type == ClaimTypes.Name)?.Value ?? "";
            var dbUser = await _userService.GetUserByUsername(username);
            dbUser.Forename = user.Forename;
            dbUser.Lastname = user.Lastname;
            
            dbUser.Gender = user.Gender;
            dbUser.City = user.City;
            dbUser.Street = user.Street;
            dbUser.HouseNumber = user.HouseNumber;
            dbUser.Zipcode = user.Zipcode;

            return await _userService.UpdateUser(dbUser);
        }



        [Route("[action]")]
        [HttpGet]
        [Authorize]
        public ActionResult<UserInformation> WhoAmI()
        {

            var result = _authService.GetUserOfClaimPrincipal(User);
            if (result != null)
            {
                var userInfo = this._userService.GetUserInformation(result.UserName);
                return Ok(userInfo);
            }

            return BadRequest();
        }

        /// <summary>
        /// Checks if a username is taken by another user. Will return true if the username is taken.
        /// will return false if the username is unknown.
        /// The username will be cast to lower and checked. UserA and usera will be referenced to the same
        /// user.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<bool> CheckUsername(string username)
        {
            return await _userService.CheckUsername(username.ToLower());
        }

        /// <summary>
        /// Checks if a Mail Address is still open for registration or if its taken.
        /// Will Return true if the E-Mail Address is used by any user.
        /// false if the email address is not registered in the system.
        /// Note that this function will not validate if the EMail Address is valid to use for the plattform.
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<bool> CheckMail(string mail)
        {
            return await _userService.CheckMail(mail);
        }

        /// <summary>
        /// Sets the privacy mode of the name that is visible to the public of the current logged in user.
        /// Note that the header needs to contain the baerer token for this function to work.
        /// Allowed are the modes from zero to three (0,1,2,3)
        /// 0 = Fullname (Forename + Lastname)
        /// 1 = Full Forname + First Letter of the Lastname
        /// 2 = First letter of the forename and full lastname
        /// 3 = first letter of first name + first letter of lastname
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPatch]
        [Authorize]
        public ActionResult SetMyPrivacyPublicNameDisplayMode(int mode)
        {
            if (mode < 0 || mode > 3) return BadRequest("Only mode 0-3 are allowed!");

            var user = _authService.GetUserOfClaimPrincipal(User);
            if (user == null) BadRequest();
            var privacySettings = _userService.GetUserPrivacySettings(user);
            if (privacySettings == null) privacySettings = _userService.InitUserPrivacySettings(user);
            privacySettings.PublicNameDisplayMode = (Models.User.UserPrivacySettings.ENameDisplayMode) mode;
            this._userService.UpdatePrivacySettings(privacySettings);
            return Ok("settings saved");
        }

        public UserController(IAuthService authService, IUserService userService)
        {
            this._authService = authService;
            this._userService = userService;
        }
    }
}