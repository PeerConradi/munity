using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Database.Models.Conference
{
    public class ConferenceDashboardCard
    {
        public int ConferenceDashboardCardId { get; set; }

        public Conference Conference { get; set; }

        public string LanguageCode { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public bool ShowToRegistrationButton { get; set; }

        public bool ShowToWebsiteButton { get; set; }

        public string ExternalWebsiteLink { get; set; }

        public bool Active { get; set; }
    }
}
