using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Conference.Roles
{
    public class SecretaryGeneralRole : AbstractRole
    {
        [Column(TypeName = "varchar(200)")]
        public string Title { get; set; }

    }
}
