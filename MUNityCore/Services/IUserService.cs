using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityCore.Models.Core;

namespace MUNityCore.Services
{
    public interface IUserService
    {
        User CreateUser(string username, string forename, string lastname, string password, string mail, DateTime birthday);

        Task<User> GetUserByUsername(string username);

        Task<int> UpdateUser(User user);

        Task<bool> CheckUsername(string username);

        Task<bool> CheckMail(string mail);

        Models.User.UserPrivacySettings GetUserPrivacySettings(User user);

        Models.User.UserPrivacySettings InitUserPrivacySettings(User user);

        void UpdatePrivacySettings(Models.User.UserPrivacySettings settings);

        bool BanUser(User user);

        IEnumerable<User> GetBannedUsers();

        IEnumerable<User> GetUserBlock(int blockid);

        Task<int> GetUserCount();

        void RemoveUser(User user);

        IEnumerable<User> GetAdministrators();
    }
}
