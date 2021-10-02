using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUNity.Database.Models.Conference;

namespace MUNity.Database.Model.Website
{
    public class ConferenceApplicationPage
    {
        public int ConferenceApplicationPageId { get; set; }

        public Models.Conference.Conference Conference { get; set; }

        public string Title { get; set; }

        public string TitleMotivation { get; set; }

        public string DescriptionMotivation { get; set; }

        public string TitleExperience { get; set; }

        public string DescriptionExperience { get; set; }
    }
}
