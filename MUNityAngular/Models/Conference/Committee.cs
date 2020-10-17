using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MUNityAngular.Models.Conference;

namespace MUNityAngular.Models.Conference
{

    /// <summary>
    /// The committees are listed inside each Conference. Every conference
    /// needs to create its own list of committees, they should not be reused inside of other
    /// Conferences.
    /// </summary>
    [DataContract]
    public class Committee : ICommitteeFacade
    {

        [DataMember]
        public string CommitteeId { get; set; }

        [DataMember]
        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [DataMember]
        [MaxLength(250)]
        public string FullName { get; set; }

        [DataMember]
        [MaxLength(10)]
        public string Abbreviation { get; set; }

        [DataMember]
        [MaxLength(10)]
        public string Article { get; set; }

        [IgnoreDataMember]
        [Newtonsoft.Json.JsonIgnore]
        [JsonIgnore]
        public Committee ResolutlyCommittee { get; set; }

        [DataMember]
        [NotMapped]
        public string ResolutlyCommitteeId => ResolutlyCommittee?.ResolutlyCommitteeId ?? "";

        [DataMember]
        [NotMapped]
        public string ConferenceId => Conference?.ConferenceId ?? "";

        [IgnoreDataMember]
        [Newtonsoft.Json.JsonIgnore]
        [JsonIgnore]
        public Conference Conference { get; set; }

        public List<CommitteeTopic> Topics { get; set; }

        [IgnoreDataMember]
        [Timestamp]
        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public byte[] CommitteeTimestamp { get; set; }

        public Committee(MUNityAngular.Schema.Request.Conference.ConferenceRequests.CreateCommittee request)
        {
            CommitteeId = Guid.NewGuid().ToString();
            Article = request.Article;
            Name = request.Name;
            FullName = request.FullName;
            Abbreviation = request.Abbreviation;
        }

        public Committee()
        {
            CommitteeId = Guid.NewGuid().ToString();
        }
    }
}
