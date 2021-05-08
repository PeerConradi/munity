using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityCore.Models.User;
using MUNity.Schema.User;

namespace MUNityCore.Services
{
    public interface IUserService
    {
        MunityUser CreateUser(string username, string forename, string lastname, string password, string mail, DateTime birthday);

        Task<MunityUser> GetUserByUsername(string username);

        Task<int> UpdateUser(MunityUser user);

        Task<bool> CheckUsername(string username);

        Task<bool> CheckMail(string mail);

        Models.User.UserPrivacySettings GetUserPrivacySettings(MunityUser user);

        Models.User.UserPrivacySettings InitUserPrivacySettings(MunityUser user);

        void UpdatePrivacySettings(Models.User.UserPrivacySettings settings);

        bool BanUser(MunityUser user);

        IEnumerable<MunityUser> GetBannedUsers();

        List<MunityUser> GetUserBlock(int blockid);

        Task<int> GetUserCount();

        void RemoveUser(MunityUser user);

        IEnumerable<MunityUser> GetAdministrators();

        Task<UserInformation> GetUserInformation(string username);

        List<Dtos.Users.UserWithRolesDto> UsersWithRoles();

        MunityUser GetUserById(string id);
    }
}
