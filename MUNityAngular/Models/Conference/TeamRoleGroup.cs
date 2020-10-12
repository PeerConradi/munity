using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MUNityAngular.Models.Conference.Roles;

namespace MUNityAngular.Models.Conference
{
    public class TeamRoleGroup
    {
        public int TeamRoleGroupId { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string FullName { get; set; }

        [Column(TypeName = "varchar(10)")]
        public string Abbreviation { get; set; }

        public int GroupLevel { get; set; }

        public List<TeamRole> TeamRoles { get; set; }

        [Timestamp]
        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public byte[] TeamRoleGroupTimestamp { get; set; }
    }
}
