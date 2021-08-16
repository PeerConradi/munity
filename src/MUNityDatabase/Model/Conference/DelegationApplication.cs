using MUNity.Database.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Database.Models.Conference
{
    public class DelegationApplication
    {
        public int DelegationApplicationId { get; set; }

        public Delegation Delegation { get; set; }

        public MunityUser User { get; set; }

        public DateTime ApplyDate { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
    }
}
