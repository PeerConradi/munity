using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Conference.Roles
{
    public class VisitorRole : AbstractRole
    {
        [Column(TypeName = "varchar(100)")]
        public string Organisation { get; set; }

    }
}
