using System;
using System.ComponentModel.DataAnnotations;

namespace MUNity.Schema.Conference
{
    /// <summary>
    /// a request body to change the date when a conference will take place.
    /// </summary>
    public class ChangeConferenceDate
    {
        /// <summary>
        /// the id of the conference you want to change the date of
        /// </summary>
        [Required]
        [MaxLength(80)]
        public string ConferenceId { get; set; }

        /// <summary>
        /// the new start Date.
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// The new end date.
        /// </summary>
        [Required]
        public DateTime EndDate { get; set; }
    }
}
