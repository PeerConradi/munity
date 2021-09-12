using System.ComponentModel.DataAnnotations;

namespace MUNity.Schema.Conference
{
    /// <summary>
    /// A request body used when creating a new committee for a conference.
    /// </summary>
    public class CreateCommitteeRequest
    {
        /// <summary>
        /// The id of the conference that you want to create the committee at.
        /// </summary>
        [Required]
        [MaxLength(80)]
        public string ConferenceId { get; set; }

        /// <summary>
        /// The name of the committee for example: General Assembly
        /// The max length is 150 characters.
        /// </summary>
        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        /// <summary>
        /// The full name of the committee for example: United Nations General Assembly.
        /// The max length is 250 characters.
        /// </summary>
        [Required]
        [MaxLength(250)]
        public string FullName { get; set; }

        /// <summary>
        /// The short name of the committee for example: GA (for General Assembly)
        /// The Abbreviation (short name) has a max length of 10 characters.
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string Short { get; set; }

        /// <summary>
        /// The article of the committe for example: the, der, die, le, la...
        /// the max length of the article is 10 characters.
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string Article { get; set; }

        /// <summary>
        /// An id of a parent committee. Leave this property null if you dont want to set any.
        /// </summary>
        public string ResolutlyCommitteeId { get; set; }
    }

    public class CreateCommitteeResponse
    {
        public enum StatusCodes
        {
            Success,
            Error,
            NoPermission,
            ConferenceNotFound,
            NameTaken,
            FullNameTaken,
            ShortTaken,
            ResolutlyCommitteeNotFound
        }

        public StatusCodes Status { get; set; }

        public string NewCommitteeId { get; set; }
    }
}
