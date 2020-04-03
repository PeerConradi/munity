using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Facades
{
    public interface IUserFacade
    {
        string Username { get; set; }

        string Title { get; set; }

        string Forename { get; set; }

        string Lastname { get; set; }

        string Gender { get; set; }
    }
}
