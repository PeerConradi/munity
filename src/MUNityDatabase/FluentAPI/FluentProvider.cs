using MUNity.Database.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Database.FluentAPI;

public class FluentProvider
{
    public MunityContext DbContext { get; private set; }

    public OrganizationTools Organization => new OrganizationTools(DbContext);

    public OrganizationSpecificTools ForOrganization(string organizationId)
    {
        return new OrganizationSpecificTools(DbContext, organizationId);
    }

    public ProjectTools Project => new ProjectTools(this.DbContext);


    public ConferenceTools Conference => new ConferenceTools(DbContext);

    public ConferenceSpecificTools ForConference(string conferenceId) =>
        new ConferenceSpecificTools(this.DbContext, conferenceId);

    public CommitteeSpecificTools ForCommittee(string committeeId) =>
        new CommitteeSpecificTools(this.DbContext, committeeId);

    public FluentProvider(MunityContext context)
    {
        this.DbContext = context;
    }
}
