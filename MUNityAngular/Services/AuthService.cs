using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Services
{
    public class AuthService
    {
        public bool Login(string username, string password)
        {
            throw new NotImplementedException("Not implemented yet.");
        }

        public bool Register(string username, string password, string email)
        {
            throw new NotImplementedException();
        }

        public bool UsernameAvailable(string username)
        {
            throw new NotImplementedException();
        }

        public bool ValidateAuthKey(string authkey)
        {
            return true;
        }

        
    }
}
