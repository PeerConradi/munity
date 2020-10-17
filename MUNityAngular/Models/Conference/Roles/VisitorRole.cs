using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Conference.Roles
{

    [DataContract]
    public class VisitorRole : AbstractRole
    {
        [MaxLength(100)]
        [DataMember]
        public string Organisation { get; set; }

    }
}
