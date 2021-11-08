using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.General
{
    public interface IBreadcrumbInfo
    {
    }

    public interface IConferenceBreadcrumb : IBreadcrumbInfo
    {
        string OrganizationId { get; set; }

        string OrganizationShort { get; set; }

        string OrganizationName { get; set; }

        string ProjectId { get; set; }

        string ProjectShort { get; set; }

        string ProjectName { get; set; }

        string ConferenceId { get; set; }

        string ConferenceName { get; set; }

        string ConferenceShort { get; set; }
    }
}
