using System.ComponentModel.DataAnnotations;

namespace MUNity.Schema.Conference
{
    /// <summary>
    /// The Body of a request to change the short name (abbreviation) of a conference.
    /// </summary>
    public class ChangeConferenceAbbreviation
    {
        /// <summary>
        /// The id of the conference that the abbreviation should be changed of.
        /// </summary>
        [Required]
        [MaxLength(80)]
        public string ConferenceId { get; set; }

        /// <summary>
        /// the new abbreviation (short name) that you want to set.
        /// </summary>
        [Required]
        [MaxLength(18)]
        public string NewAbbreviation { get; set; }
    }
}
