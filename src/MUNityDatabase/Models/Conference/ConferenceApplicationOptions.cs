using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Database.Models.Conference;

public class ConferenceApplicationOptions
{
    public int ConferenceApplicationOptionsId { get; set; }

    public string ConferenceId { get; set; }

    public Conference Conference { get; set; }


    public bool IsActive { get; set; }

    public bool AllowDelegationApplication { get; set; }

    public bool AllowRoleApplication { get; set; }

    public bool AllowTeamApplication { get; set; }

    public bool AllowDelegationWishApplication { get; set; }

    public bool AllowCountryWishApplication { get; set; }

    public bool AllowFilterByCommitteeType { get; set; }

    public DateTime? ApplicationStartDate { get; set; }

    public DateTime? ApplicationEndDate { get; set; }

    public ICollection<ConferenceApplicationFormula> Formulas { get; set; }
}
