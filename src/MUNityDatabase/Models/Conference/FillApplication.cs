using MUNity.Base;
using MUNity.Database.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Database.Models.Conference
{
    /// <summary>
    /// A fill application allows to take any type of role at the conference.
    /// The user is open to take any of the roles or fill up in any delegation, committee etc.
    /// </summary>
    public class FillApplication
    {
        public int FillApplicationId { get; set; }

        public MunityUser User { get; set; }

        public Conference Conference { get; set; }

        public string PreferedRoleType { get; set; }

        public ICollection<ConferenceFillApplicationFieldInput> Inputs { get; set; }

        public DateTime ApplicationDate { get; set; }

        public ApplicationStatuses Status { get; set; }
    }
}
