using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.DataHandlers.EntityFramework.Models
{
    public class Conference
    {
        public string ConferenceId { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }

        public string Abbreviation { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string SecretaryGeneralTitle { get; set; }

        public string SecretaryGeneralName { get; set; }

        public DateTime CreationDate { get; set; }

        public User CreationUser { get; set; }

        public List<Committee> Committees { get; set; }

        public Conference()
        {
            ConferenceId = Guid.NewGuid().ToString();
        }
    }
}
