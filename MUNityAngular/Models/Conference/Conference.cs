using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.Models.Conference;

namespace MUNityAngular.Models.Conference
{

    /// <summary>
    /// A conference is as the name tells a single conference.
    /// Like Model United Nations London 2021. This means it is a single
    /// event of maybe a list of Model United Nation Conferences. If there are more
    /// the conference, for example a conference every year, then this will can be
    /// found by the Project
    /// </summary>
    public class Conference
    {
        public string ConferenceId { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }

        public string Abbreviation { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }


        public DateTime CreationDate { get; set; }

        public DataHandlers.EntityFramework.Models.User CreationUser { get; set; }

        public List<Committee> Committees { get; set; }

        public Project ConferenceProject { get; set; }

        public Conference()
        {
            ConferenceId = Guid.NewGuid().ToString();
            Committees = new List<Committee>();
        }
    }
}
