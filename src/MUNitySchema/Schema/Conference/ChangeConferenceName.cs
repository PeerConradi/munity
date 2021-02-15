using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MUNity.Schema.Conference
{
    /// <summary>
    /// A request body to change a conference name.
    /// </summary>
    public class ChangeConferenceName
    {
        /// <summary>
        /// the id of the conference where you want to change the name.
        /// </summary>
        [Required]
        [MaxLength(80)]
        public string ConferenceId { get; set; }

        /// <summary>
        /// the new name you want to give the conference.
        /// </summary>
        [Required]
        [MaxLength(150)]
        public string NewName { get; set; }
    }
}
