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

        public bool IsActive { get; set; }

        public DateTime? ApplicationStartDate { get; set; }

        public DateTime? ApplicationEndDate { get; set; }

        public string PreContent { get; set; }

        public string PostContent { get; set; }

        public string Title { get; set; }

        public string TitleMotivation { get; set; } = "Motivation";

        public string DescriptionMotivation { get; set; }

        public string TitleExperience { get; set; } = "Erfahrung";

        public string DescriptionExperience { get; set; }

        public bool ShowMotivationField { get; set; }

        public bool MotivationRequired { get; set; }

        public int? MotivationMinLength { get; set; }

        public int? MotivationMaxLength { get; set; }

        public bool ShowExperienceField { get; set; }

        public bool ExperienceRequired { get; set; }

        public int? ExperienceMinLength { get; set; }

        public int? ExperienceMaxLength { get; set; }

        public bool ShowSchoolField { get; set; }

        public bool SchoolRequired { get; set; }
    }
}
