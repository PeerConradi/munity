using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.DataHandlers.Database;

namespace MUNityAngular.Models.Conference
{
    public class ConferenceTeamUserModel
    {
        [PrimaryKey]
        [DatabaseSave("id")]
        public int Id { get; set; }

        [DatabaseSave("conferenceid")]
        public string ConferenceId { get; set; }

        [DatabaseSave("userid")]
        public string UserId { get; set; }

        [DatabaseSave("role")]
        public int RoleId { get; set; }
    }
}
