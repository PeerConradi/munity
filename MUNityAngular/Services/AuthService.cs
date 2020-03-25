using MUNityAngular.DataHandlers.Database;
using MUNityAngular.Models.User;
using MUNityAngular.Models.Resolution;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using MUNityAngular.DataHandlers.EntityFramework;

namespace MUNityAngular.Services
{
    public class AuthService
    {

        private MunityContext _context;

        private bool registrationOpened = true;
        public bool IsRegistrationOpened { get => registrationOpened; 
        set
            {
                this.registrationOpened = value;
            }
        }

        public enum EUserClearance
        {
            Default,
            CreateConference
        }

        public bool IsDefaultAuth(string auth)
        {
            return auth == "default";
        }

        public (bool status, string key) Login(string username, string password)
        {
            var success = false;
            var customAuthKey = string.Empty;

            var user = _context.Users.FirstOrDefault(n => n.Username == username);
            if (user == null)
                return (false, null);

            if (Util.Hashing.PasswordHashing.CheckPassword(password, user.Salt, user.Password))
            {
                //Create new Auth Key
                var key = new MUNityAngular.DataHandlers.EntityFramework.Models.AuthKey();
                _context.AuthKey.Add(key);
                _context.SaveChanges();
                return (true, key.AuthId);
            }

            return (false, null);
        }

        public void DeleteAllUserKeys(int userid)
        {
            _context.AuthKey.RemoveRange(_context.AuthKey.Where(n => n.User.UserId == userid));
            _context.SaveChanges();
        }

        public void DeleteAccount(int id)
        {
            //Delete all User Keys
            DeleteAllUserKeys(id);
            _context.Users.Remove(_context.Users.FirstOrDefault(n => n.UserId == id));
            _context.SaveChanges();
        }

        public int? GetHeadAdminId()
        {
            return _context.Admins.FirstOrDefault()?.User.UserId ?? null;
        }

        public (bool status, string key) LoginWithId(int userid, string password)
        {

            var user = _context.Users.FirstOrDefault(n => n.UserId == userid);
            if (user == null)
                return (false, null);

            return Login(user.Username, password);
        }

        public bool CheckPasswordForUserid(int userid, string password)
        {
            var user = _context.Users.FirstOrDefault(n => n.UserId == userid);
            if (user == null)
                return false;
            return Util.Hashing.PasswordHashing.CheckPassword(password, user.Salt, user.Password);
        }

        internal void Logout(string auth)
        {
            if (string.IsNullOrEmpty(auth))
                return;

            _context.AuthKey.Remove(_context.AuthKey.FirstOrDefault(n => n.AuthId == auth));
            _context.SaveChanges();
        }

        public void DeleteAuthKeysForUser(int userid)
        {
            _context.AuthKey.RemoveRange(_context.AuthKey.Where(n => n.User.UserId == userid));
            _context.SaveChanges();
        }

        public bool Register(string username, string password, string email)
        {
            if (!UsernameAvailable(username))
                return false;

            var user = new DataHandlers.EntityFramework.Models.User();
            user.RegistrationDate = DateTime.Now;

            var hash = Util.Hashing.PasswordHashing.InitHashing(password);
            user.Salt = hash.Salt;
            user.Password = hash.Key;
            _context.Users.Add(user);
            _context.SaveChanges();

            return true;
        }

        public void SetPassword(int userid, string newPassword)
        {
            var hash = Util.Hashing.PasswordHashing.InitHashing(newPassword);
            var user = _context.Users.FirstOrDefault(n => n.UserId == userid);
            if (user == null)
                return;
            user.Salt = hash.Salt;
            user.Password = hash.Key;
            _context.SaveChanges();
        }

        public void SetForename(int userid, string newForename)
        {
            var user = _context.Users.FirstOrDefault(n => n.UserId == userid);
            if (user != null)
            {
                user.Forename = newForename;
                _context.SaveChanges();
            }
        }
            

        public void SetLastname(int userid, string newLastname)
        {
            var user = _context.Users.FirstOrDefault(n => n.UserId == userid);
            if (user != null)
            {
                user.Lastname = newLastname;
                _context.SaveChanges();
            }
        }

        public bool UsernameAvailable(string username)
        {
            return _context.Users.FirstOrDefault(n => n.Username == username) == null;
        }

        public (bool valid, int? userid) ValidateAuthKey(string authkey)
        {
            var auth = _context.AuthKey.FirstOrDefault(n => n.AuthId == authkey);
            if (auth == null)
                return (false, null);

            return (true, auth.User.UserId);
        }

        #region Resolution

        public bool CanCreateResolution(string auth)
        {
            if (auth == "default")
                return true;

            return ValidateAuthKey(auth).valid;
        }

        

        public bool CanEditResolution(int userid, ResolutionModel resolution)
        {

            //If the resolution is public edit return true
            if (GetResolutionPublicState(resolution).writeable)
                return true;

            //Check if the user is owner
            if (GetOwnerId(resolution).Value == userid)
                return true;

            //Check if the user has the right to edit this document
            //This is not the best way to do it, let me think of something better
            bool inDocumentGroup =
                _context.ResolutionUsers.FirstOrDefault(n => n.User.UserId == userid)?.CanEdit ?? false;

            //Check of the resolution is bindet to a conferece where the user is part
            // of the Team
            // Get Conferences where the user is inside the team
            var userConferences = _context.TeamUsers.Where(n => n.User.UserId == userid).Select(n => n.Role.Conference.ConferenceId);
            // Get the Resolution Conferences
            var resolutionConferences = _context.ConferenceResolutions.Where(n => n.Resolution.ResolutionId == resolution.ID).Select(n => n.Conference.ConferenceId);
            var editBecauseInConference = resolutionConferences.Any(n => userConferences.Contains(n));

            return editBecauseInConference || inDocumentGroup;
        }

        public (bool readable, bool writeable) GetResolutionPublicState(ResolutionModel resolution)
        {
            if (resolution == null)
                throw new ArgumentNullException("The Resolution cant be empty");

            return GetResolutionPublicState(resolution.ID);
        }

        public (bool readable, bool writeable) GetResolutionPublicState(string resolutionid)
        {
            var resolution = _context.Resolutions.FirstOrDefault(n => n.ResolutionId == resolutionid);
            if (resolution == null)
                return (false, false);
            return (resolution.PublicRead, resolution.PublicWrite);
        }

        public int? GetOwnerId(ResolutionModel resolution)
        {
            return _context.Resolutions.FirstOrDefault(n => n.ResolutionId == resolution.ID).CreationUser?.UserId ?? null;
        }

        #endregion

        public DataHandlers.EntityFramework.Models.UserAuths GetAuthsByAuthkey(string authkey)
        {
            var authUser = _context.AuthKey.FirstOrDefault(n => n.AuthId == authkey)?.User ?? null;
            if (authUser == null)
                return null;

            return _context.UserAuths.FirstOrDefault(n => n.User == authUser);
        }

        public bool IsAdmin(int userid)
        {
            return _context.Admins.Any(n => n.User.UserId == userid && n.PowerRank == 5);
        }
        
        public List<DataHandlers.EntityFramework.Models.User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public int GetUserCount()
        {
            return _context.Users.Count();
        }

        public DataHandlers.EntityFramework.Models.User GetUserByUsername(string username)
        {
            return _context.Users.FirstOrDefault(n => n.Username == username);
        }

        public DataHandlers.EntityFramework.Models.User GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(n => n.UserId == id);
        }

        public DataHandlers.EntityFramework.Models.User GetUserByAuth(string auth)
        {
            return _context.AuthKey.FirstOrDefault(n => n.AuthId == auth).User;
        }


        #region Konferenz

        public DataHandlers.EntityFramework.Models.ConferenceUserAuth CanManageConference(int userid, string conferenceid)
        {
            var conferenceAuths = _context.ConferenceUserAuths.FirstOrDefault(n => n.User.UserId == userid && n.Conference.ConferenceId == conferenceid);
            return conferenceAuths;
        }

        #endregion


        public AuthService(MunityContext context)
        {
            _context = context;
            Console.WriteLine("Auth-Service Started!");
        }
    }
}
