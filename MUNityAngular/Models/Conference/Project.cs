using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Conference
{
    public class Project
    {
        public string ProjectId { get; set; }

        public string ProjectName { get; set; }

        public string ProjectAbbreviation { get; set; }

        public Organisation.Organisation ProjectOrganisation { get; set; }

        public List<Conference> Conferences { get; set; }

        public Project()
        {
            ProjectId = Guid.NewGuid().ToString();
        }
    }
}
