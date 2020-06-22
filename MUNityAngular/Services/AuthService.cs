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
using Microsoft.EntityFrameworkCore;
using MUNityAngular.Models.Conference;
using MUNityAngular.Models.Core;

namespace MUNityAngular.Services
{
    public class AuthService
    {
        public bool CanUserEditConference(User user, Conference conference)
        {
            return true;
        }

        public bool CanUserEditResolution(User user, ResolutionModel resolution)
        {
            return true;
        }

        public bool CanCreateResolution(User user)
        {
            return true;
        }

        public User GetUserOfAuth(string authKey)
        {
            return null;
        }

        public AuthService()
        {

        }
    }
}
