using MUNity.Database.Models.Conference;
using MUNity.Database.Models.Organization;
using MUNity.Schema.Conference;
using MUNity.Schema.Organization;
using System.Linq;

namespace MUNity.Services.Extensions.CastExtensions;

public static class ConferenceSructureCast
{

    public static CommitteeSmallInfo AsSmallInfo(this Committee committee)
    {
        var info = new CommitteeSmallInfo()
        {
            CommitteeId = committee.CommitteeId,
            CommitteeShort = committee.CommitteeShort,
            FullName = committee.FullName,
            Name = committee.Name
        };
        return info;
    }

    public static ConferenceInformation AsConferenceInformation(this Conference conference)
    {
        var info = new ConferenceInformation()
        {
            ConferenceId = conference.ConferenceId,
            ConferenceShort = conference.ConferenceShort,
            EndDate = conference.EndDate,
            FullName = conference.FullName,
            Name = conference.Name,
            StartDate = conference.StartDate
        };

        if (conference.ConferenceProject != null)
            info.ProjectId = conference.ConferenceProject.ProjectId;

        if (conference.Committees != null)
            info.Committees = conference.Committees.Select(n => n.AsSmallInfo());

        return info;
    }

    public static OrganizationInformation AsInformation(this Organization organization)
    {
        var mdl = new OrganizationInformation()
        {
            OrganizationId = organization.OrganizationId,
            OrganizationName = organization.OrganizationName,
            OrganizationShort = organization.OrganizationShort
        };
        return mdl;
    }
}
