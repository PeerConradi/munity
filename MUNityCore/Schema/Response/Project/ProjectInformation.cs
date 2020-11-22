using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Schema.Response.Project
{
    public class ProjectInformation
    {
        public string ProjectId { get; set; }

        public string ProjectName { get; set; }

        public string ProjectShort { get; set; }

        public string OrganizationId { get; set; }

        public IEnumerable<Conference.ConferenceSmallInfo> Conferences { get; set; }

        public ProjectInformation(Models.Conference.Project project)
        {
            this.ProjectId = project.ProjectId;
            this.ProjectName = project.ProjectName;
            this.ProjectShort = project.ProjectShort;
            this.OrganizationId = project.ProjectOrganization?.OrganizationId ?? null;
            this.Conferences = project.Conferences?.Cast<Conference.ConferenceSmallInfo>() ?? null;
        }

        public static implicit operator ProjectInformation (Models.Conference.Project project)
        {
            return new ProjectInformation(project);
        }
    }
}
