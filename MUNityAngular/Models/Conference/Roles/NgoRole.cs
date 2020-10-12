using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Conference.Roles
{
    public class NgoRole : AbstractRole
    {
        public int Group { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string NgoName { get; set; }

        public bool Leader { get; set; }

    }
}
