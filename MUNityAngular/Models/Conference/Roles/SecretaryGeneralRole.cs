using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace MUNityCore.Models.Conference.Roles
{

    [DataContract]
    public class SecretaryGeneralRole : AbstractRole
    {
        [MaxLength(200)]
        public string Title { get; set; }

    }
}
