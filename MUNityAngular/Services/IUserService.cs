using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.Models.Core;

namespace MUNityAngular.Services
{
    public interface IUserService
    {
        User CreateUser(string username, string forename, string lastname, string password, string mail, DateTime birthday);

        Task<User> GetUserByUsername(string username);

        Task<int> UpdateUser(User user);

        Task<bool> CheckUsername(string username);

        Task<bool> CheckMail(string mail);
    }
}
