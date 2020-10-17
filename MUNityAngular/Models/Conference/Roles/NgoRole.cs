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
    public class NgoRole : AbstractRole
    {

        [DataMember]
        public int Group { get; set; }

        [DataMember]
        [MaxLength(100)]
        public string NgoName { get; set; }

        [DataMember]
        public bool Leader { get; set; }

    }
}
