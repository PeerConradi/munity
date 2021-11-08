using System.Collections.Generic;

namespace MUNity.Schema.Project
{
    public class ProjectDashboardInfo
    {
        public string ProjectId { get; set; }

        public string Name { get; set; }

        public string Short { get; set; }

        public List<ProjectDashboardConferenceInfo> Conferences { get; set; }
    }
}
