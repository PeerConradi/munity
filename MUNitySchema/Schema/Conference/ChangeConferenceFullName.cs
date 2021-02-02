using System.ComponentModel.DataAnnotations;

namespace MUNity.Schema.Conference
{
    /// <summary>
    /// A request body to change the long name (full name) of a conference.
    /// </summary>
    public class ChangeConferenceFullName
    {
        /// <summary>
        /// the id of the conference you want to change to full name at.
        /// </summary>
        [Required]
        [MaxLength(80)]
        public string ConferenceId { get; set; }

        /// <summary>
        /// the new full name.
        /// </summary>
        [Required]
        [MaxLength(250)]
        public string NewFullName { get; set; }
    }
}
