using System.ComponentModel.DataAnnotations;

namespace MUNity.Database.Models.Website
{
    public class AbstractConferenceWebPageElement
    {
        [Key]
        public int ConferenceWebPageComponentId { get; set; }

        public ConferenceWebPage Page { get; set; }

        public int SortOrder { get; set; }
    }
}
