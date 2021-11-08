using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUNity.Database.Models.Website;
using MUNity.Base;

namespace MUNity.Database.Models.Conference;

public class DelegationApplication
{
    public int DelegationApplicationId { get; set; }

    public ICollection<DelegationApplicationPickedDelegation> DelegationWishes { get; set; }

    public ICollection<DelegationApplicationUserEntry> Users { get; set; }

    public DateTime ApplyDate { get; set; }

    public ICollection<ConferenceDelegationApplicationFieldInput> FormulaInputs { get; set; }

    /// <summary>
    /// Are Others able to see this application and add themselfs to it.
    /// </summary>
    public bool OpenToPublic { get; set; }

    public ApplicationStatuses Status { get; set; }

}
