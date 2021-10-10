using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUNity.Database.Models.Conference;
using MUNity.Database.Models.Conference.Roles;

namespace MUNity.Database.Models.Conference
{
    /// <summary>
    /// Cost Rules can be used to define certain rules on pricing a conference, participating in a specific role
    /// or committee differently. Note that the general cost will be the Conference.GeneralParticipationCost.
    /// You can either use a CostRule to change the cost with the Costs-Property or use the CutPercentage/AddPercentage
    /// to calculate a new price.
    ///
    /// You can also add some logic like letting different ages be handled differently.
    /// </summary>
    public class ConferenceParticipationCostRule
    {
        public int ConferenceParticipationCostRuleId { get; set; }

        public Models.Conference.Conference Conference { get; set; }

        public Committee Committee { get; set; }

        public Delegation Delegation { get; set; }

        public AbstractConferenceRole Role { get; set; }

        public string CostRuleTitle { get; set; }

        public byte? UserMinAge { get; set; }

        public byte? UserMaxAge { get; set; }

        public decimal? Costs { get; set; }

        public double? CutPercentage { get; set; }

        public double? AddPercentage { get; set; }
    }
}
