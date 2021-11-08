using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Database.Models.Conference;

public class DelegationCostResult
{
    public List<DelegationCostPoint> Costs { get; set; }

    public DelegationCostResult()
    {
        Costs = new List<DelegationCostPoint>();
    }
}
