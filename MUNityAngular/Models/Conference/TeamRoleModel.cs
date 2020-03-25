using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.DataHandlers.Database;

namespace MUNityAngular.Models.Conference
{

    [Obsolete("Use DataHandlers.EntityFramework.Models.TeamRole")]
    public class TeamRoleModel
    {
        [DatabaseSave("id")]
        [PrimaryKey]
        public int? Id { get; set; }

        [DatabaseSave("name")]
        public string Name { get; set; }

        [DatabaseSave("description")]
        public string Description { get; set; }

        [DatabaseSave("parentrole")]
        public int? ParentRoleId { get; set; }

        [DatabaseSave("conferenceid")]
        public string ConferenceId { get; set; }

        [DatabaseSave("mincount")]
        public int MinCount { get; set; }

        [DatabaseSave("maxcount")]
        public int MaxCount { get; set; }
    }
}
