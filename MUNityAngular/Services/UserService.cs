using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MUNityAngular.DataHandlers.EntityFramework;
using MUNityAngular.Models.Core;

namespace MUNityAngular.Services
{
    public class UserService : IUserService
    {
        private readonly MunCoreContext _context;

        public User CreateUser(string username, string forename, string lastname, string password, string mail, DateTime birthday)
        {

            if (!Util.Tools.InputCheck.IsOnlyCharsAndNumbers(username))
                throw new ArgumentException("The username is not valid, you are only allowed to use a-z A-Z 0-9");

            if (_context.Users.Any(n => n.Username.ToLower() == username.ToLower()))
                throw new ArgumentException("The username is already taken!");

            // Save today's date.
            var today = DateTime.Today;

            // Calculate the age.
            var age = today.Year - birthday.Year;

            // Go back to the year the person was born in case of a leap year
            if (birthday.Date > today.AddYears(-age)) age--;

            if (age < 13)
                throw new ArgumentException("you are to young to create an account!");

            var pass = Util.Hashing.PasswordHashing.InitHashing(password);
            var user = new User
            {
                Username = username,
                Mail = mail,
                Forename = forename,
                Lastname = lastname,
                Password = pass.Key,
                Salt = pass.Salt,
                Birthday = birthday
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public Task<User> GetUserByUsername(string username)
        {
            return _context.Users.FirstOrDefaultAsync(n => n.Username.ToLower() == username.ToLower());
        }

        public async Task<int> UpdateUser(User user)
        {
            _context.Users.Update(user);
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckUsername(string username)
        {
            return await _context.Users.AnyAsync(n => n.Username == username);
        }

        public async Task<bool> CheckMail(string mail)
        {
            return await _context.Users.AnyAsync(n => n.Mail == mail);
        }


        public UserService(MunCoreContext context)
        {
            _context = context;
        }
    }
}
