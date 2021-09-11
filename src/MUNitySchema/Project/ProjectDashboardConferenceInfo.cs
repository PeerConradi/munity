using System;

namespace MUNity.Schema.Project
{
    public class ProjectDashboardConferenceInfo
    {
        public string ConferenceId { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }

        public string Short { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime CreationDate { get; set; }

        public string CreationUserUsername { get; set; }
    }
}
