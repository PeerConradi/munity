using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Database.Models.Website
{
    public class ConferenceWebsiteFooter
    {
        [Key]
        public string ConferenceId { get; set; }

        [ForeignKey(nameof(ConferenceId))]
        public Conference.Conference Conference { get; set; }

        public string FooterContent { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
