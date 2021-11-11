using Microsoft.AspNetCore.Identity;
using MUNity.Database.Context;
using MUNity.Database.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MUNity.Schema.Conference;
using Microsoft.EntityFrameworkCore;
using MUNity.Database.Extensions;

namespace MUNity.Services
{
    public class UserService
    {
        private MunityContext context;

        private UserManager<MunityUser> userManager;

        private ILogger<UserService> _logger;

        private IMailService _mailService;

        public async Task<MunityUser> CreateUser(string username, string mail, string password)
        {
            var hasher = userManager.PasswordHasher;
           
            var user = new MunityUser()
            {
                UserName = username,
                NormalizedUserName = username.ToUpper(),
                Email = mail,
                NormalizedEmail = mail.ToUpper()
            };
            var pass = hasher.HashPassword(user, password);
            user.PasswordHash = pass;
            var success = await userManager.CreateAsync(user);
            if (!success.Succeeded)
            {
                // TODO: Log errors
                return null;
            }
            return user;
        }

        public async Task<MunityUser> CreateShadowUser(string mail)
        {
            var result = await userManager.CreateShadowUser(mail);
            if (result.Result.Succeeded)
            {
                _mailService.SendMail(mail, "Eingeladen", "Guten Tag, <br> Sie wurden zur Konferenz... eingeladen.");
                return result.User;

            }
            else
            {
                _logger.LogWarning($"Unable to create shadow user {mail} codes: {string.Join(", ", result.Result.Errors.Select(n => n.Code))}");
            }
            return null;
        }

        public async Task<MunityUser> CreateUser(MUNity.Schema.Account.RegisterModel model)
        {
            return await CreateUser(model.Username, model.Mail, model.Password);
        }

        public MunityUser GetUserByUsername(string username)
        {
            return context.Users.FirstOrDefault(n => n.UserName == username);
        }

        public bool LoginValid(string username, string password)
        {
            var user = GetUserByUsername(username);
            if (user == null)
                return false;

            var result = userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Success;
        }

        public async Task<string> GetForeAndLastNameAsync(ClaimsPrincipal claim)
        {
            if (claim == null)
            {
                _logger.LogWarning($"GetForeAndLastNameAsync was called with a null claim principal. Make sure you make valid calls to this method.");
                return "-";
            }
            var user = await userManager.GetUserAsync(claim);
            if (user == null) return "-";

            if (!string.IsNullOrEmpty(user.Forename) || !string.IsNullOrEmpty(user.Lastname))
                return user.Forename + " " + user.Lastname;

            return "-";
        }

        public List<AddingToApplicationUser> GetUsersThatCanBeAddedToApplication(string conferenceId, string searchTerm)
        {
            var list = this.context.Users
                .Where(n => EF.Functions.Like(n.UserName, $"%{searchTerm}%"))
                .Select(n => new AddingToApplicationUser()
                {
                    UserName = n.UserName,
                    DisplayName = n.GetDisplayNamePublic
                });

            return list.ToList();
        }

        //public async Task<object> Login(MunityUser user)
        //{
        //    var signIn = await signInManager.SignIn(user, true);
        //}

        public UserService(MunityContext context, UserManager<MunityUser> userManager, IMailService mailService, ILogger<UserService> logger)
        {
            this.context = context;
            this.userManager = userManager;
            this._logger = logger;
            this._mailService = mailService;
        }
    }
}
