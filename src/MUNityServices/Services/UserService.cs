using Microsoft.AspNetCore.Identity;
using MUNity.Database.Context;
using MUNity.Database.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Services
{
    public class UserService
    {
        private MunityContext context;

        private UserManager<MunityUser> userManager;

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

        //public async Task<object> Login(MunityUser user)
        //{
        //    var signIn = await signInManager.SignIn(user, true);
        //}

        public UserService(MunityContext context, UserManager<MunityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }
    }
}
