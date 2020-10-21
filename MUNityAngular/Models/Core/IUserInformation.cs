using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Models.Core
{
    public interface IUserInformation
    {
        string Username { get; set; }

        string Title { get; set; }

        string Forename { get; set; }

        string Lastname { get; set; }

        string ProfileImageName { get; set; }

        DateTime RegistrationDate { get; set; }

        DateTime LastOnline { get; set; }
    }
}
