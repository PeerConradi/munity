using MUNityAngular.DataHandlers.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models
{
    public class UserModel
    {

        [DatabaseSave("id")]
        public string Id { get; set; }

        [DatabaseSave("username")]
        public string Username { get; set; }

        [DatabaseSave("forename")]
        public string Forename { get; set; }

        [DatabaseSave("lastname")]
        public string Lastname { get; set; }
    }
}
