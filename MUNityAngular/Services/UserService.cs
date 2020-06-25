using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MUNityAngular.DataHandlers.EntityFramework;
using MUNityAngular.Models.Core;

namespace MUNityAngular.Services
{
    public class UserService
    {
        private readonly MunCoreContext _context;

        public User CreateUser(string username, string password, string mail, DateTime birthday)
        {

            if (!Util.Tools.InputCheck.IsOnlyCharsAndNumbers(username))
                throw new ArgumentException("The username is not valid, you are only allowed to use a-z A-Z 0-9");

            if (_context.Users.Any(n => n.Username.ToLower() == username.ToLower()))
                throw new ArgumentException("The username is already taken!");

            
            var user = new User();
            user.Username = username;
            user.Mail = mail;
            var pass = Util.Hashing.PasswordHashing.InitHashing(password);
            user.Password = pass.Key;
            user.Salt = pass.Salt;
            user.Birthday = birthday;

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public Task<User> GetUserByUsername(string username)
        {
            return _context.Users.FirstOrDefaultAsync(n => n.Username.ToLower() == username.ToLower());
        }

        public UserService(MunCoreContext context)
        {
            _context = context;
        }
    }
}
