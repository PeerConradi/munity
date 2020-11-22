using MUNityCore.DataHandlers.EntityFramework;
using MUNityCore.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Services
{
    public class InstallationService
    {
        private readonly MunityContext _context;

        public MunityUser CreateAdmin(string username, string password, DateTime birthdate, string mail)
        {
            var auth = new MunityUserAuth()
            {
                UserAuthName = "Admin",
                AuthLevel = MunityUserAuth.EAuthLevel.Headadmin,
                CanCreateOrganization = true,
            };

            var pass = Util.Hashing.PasswordHashing.InitHashing(password);

            var admin = new MunityUser()
            {
                Auth = auth,
                Birthday = birthdate,
                Username = username,
                Password = pass.Key,
                Salt = pass.Salt
            };

            this._context.UserAuths.Add(auth);
            this._context.Users.Add(admin);
            this._context.SaveChanges();
            return admin;
        }

        public InstallationService(MunityContext context)
        {
            this._context = context;
        }

        public void Install()
        {
            // TODO!
        }
    }
}
