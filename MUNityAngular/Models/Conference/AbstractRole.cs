using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Conference
{
    public abstract class AbstractRole
    {

        [Key]
        public int RoleId { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string RoleName { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string RoleFullName { get; set; }

        [Column(TypeName = "varchar(10)")]
        public string RoleShort { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        [JsonIgnore]
        public Conference Conference { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        [JsonIgnore]
        public RoleAuth RoleAuth { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string IconName { get; set; }

        public EApplicationStates ApplicationState { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string ApplicationValue { get; set; }

        public virtual bool AllowMultipleParticipations { get; }

        [Column(TypeName = "varchar(100)")]
        public string RoleType => this.GetType().Name;

        [Timestamp]
        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public byte[] RoleTimestamp { get; set; }
    }
}
