using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MUNityCore.DataHandlers.EntityFramework;
using MUNityCore.Models.Core;
using MUNityCore.Models.User;

namespace MUNityCore.Services
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
            return await _context.Users.AnyAsync(n => n.Username.ToLower() == username);
        }

        public async Task<bool> CheckMail(string mail)
        {
            return await _context.Users.AnyAsync(n => n.Mail == mail);
        }

        public IEnumerable<User> GetBannedUsers()
        {
            return this._context.Users.Where(n => n.UserState == User.EUserState.BANNED);
        }

        public IEnumerable<User> GetUserBlock(int blockid)
        {
            return this._context.Users.OrderBy(n => n.Lastname).Skip(blockid).Take(100);
        }

        public Task<int> GetUserCount()
        {
            return this._context.Users.CountAsync();
        }

        public Models.User.UserPrivacySettings GetUserPrivacySettings(User user)
        {
            if (user == null) return null;
            return this._context.Users.Include(n => n.PrivacySettings)
                .FirstOrDefault(n => n.UserId == user.UserId)
                ?.PrivacySettings;
        }

        public Models.User.UserPrivacySettings InitUserPrivacySettings(User user)
        {
            if (user == null) return null;
            user.PrivacySettings = new UserPrivacySettings() {User = user};
            this._context.SaveChanges();
            return user.PrivacySettings;
        }

        public void UpdatePrivacySettings(Models.User.UserPrivacySettings settings)
        {
            this._context.Add(settings);
            this._context.SaveChanges();
        }

        

        public void RemoveUser(User user)
        {
            this._context.Users.Remove(user);
            this._context.SaveChanges();
        }

        public bool BanUser(User user)
        {
            if (user == null) return false;
            user.UserState = User.EUserState.BANNED;
            this._context.SaveChanges();
            return true;
        }

        public IEnumerable<User> GetAdministrators()
        {
            return this._context.Users.Where(n =>
                n.Auth.AuthLevel == UserAuth.EAuthLevel.Admin || n.Auth.AuthLevel == UserAuth.EAuthLevel.Headadmin);
        }

        public UserService(MunCoreContext context)
        {
            _context = context;
        }
    }
}
