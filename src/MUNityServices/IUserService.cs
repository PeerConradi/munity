using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityCore.Models.User;
using MUNity.Schema.User;
using MUNity.Database.Models.User;

namespace MUNity.Services
{
    public interface IUserService
    {

        Task<MunityUser> GetUserByUsername(string username);

        Task<int> UpdateUser(MunityUser user);

        Task<bool> CheckUsername(string username);

        Task<bool> CheckMail(string mail);

        UserPrivacySettings GetUserPrivacySettings(MunityUser user);

        UserPrivacySettings InitUserPrivacySettings(MunityUser user);

        void UpdatePrivacySettings(UserPrivacySettings settings);

        bool BanUser(MunityUser user);

        IEnumerable<MunityUser> GetBannedUsers();

        List<MunityUser> GetUserBlock(int blockid);

        Task<int> GetUserCount();

        void RemoveUser(MunityUser user);

        IEnumerable<MunityUser> GetAdministrators();

        Task<UserInformation> GetUserInformation(string username);

        List<MUNityCore.Dtos.Users.UserWithRolesDto> UsersWithRoles();

        MunityUser GetUserById(string id);
    }
}
