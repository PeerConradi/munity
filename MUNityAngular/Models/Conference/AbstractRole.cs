using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Conference
{
    public abstract class AbstractRole
    {

        [Key]
        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public Conference Conference { get; set; }

        public string IconName { get; set; }

        public EApplicationStates ApplicationState { get; set; }

        public string ApplicationValue { get; set; }
    }
}
