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

        [MaxLength(150)]
        public string RoleName { get; set; }

        [MaxLength(250)]
        public string RoleFullName { get; set; }

        [MaxLength(10)]
        public string RoleShort { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        [JsonIgnore]
        public Conference Conference { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        [JsonIgnore]
        public RoleAuth RoleAuth { get; set; }

        [MaxLength(250)]
        public string IconName { get; set; }

        public EApplicationStates ApplicationState { get; set; }

        [MaxLength(250)]
        public string ApplicationValue { get; set; }

        public virtual bool AllowMultipleParticipations { get; }

        [MaxLength(150)]
        public string RoleType { get; set; }

        [Timestamp]
        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public byte[] RoleTimestamp { get; set; }
    }
}
