using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Schema.Request.Conference
{
    public class ConferenceRequests
    {
        public class ChangeConferenceName
        {
            [Required]
            [MaxLength(80)]
            public string ConferenceId { get; set; }

            [Required]
            [MaxLength(150)]
            public string NewName { get; set; }
        }

        public class ChangeConferenceFullName
        {
            [Required]
            [MaxLength(80)]
            public string ConferenceId { get; set; }

            [Required]
            [MaxLength(250)]
            public string NewFullName { get; set; }
        }

        public class ChangeConferenceAbbreviation
        {
            [Required]
            [MaxLength(80)]
            public string ConferenceId { get; set; }

            [Required]
            [MaxLength(18)]
            public string NewAbbreviation { get; set; }
        }

        public class ChangeConferenceDate
        {
            [Required]
            [MaxLength(80)]
            public string ConferenceId { get; set; }

            [Required]
            public DateTime StartDate { get; set; }

            [Required]
            public DateTime EndDate { get; set; }
        }

        public class CreateCommittee
        {
            [Required]
            [MaxLength(80)]
            public string ConferenceId { get; set; }

            [Required]
            [MaxLength(150)]
            public string Name { get; set; }

            [Required]
            [MaxLength(250)]
            public string FullName { get; set; }

            [Required]
            [MaxLength(10)]
            public string Abbreviation { get; set; }

            [Required]
            [MaxLength(10)]
            public string Article { get; set; }

            public string ResolutlyCommitteeId { get; set; }
        }


    }
}
