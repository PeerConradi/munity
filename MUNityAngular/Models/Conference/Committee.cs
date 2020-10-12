using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
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
    public class Committee : ICommitteeFacade
    {

        [Column(TypeName = "varchar(80)")]
        public string CommitteeId { get; set; }

        [Column(TypeName = "varchar(150)")]
        public string Name { get; set; }

        [Column(TypeName = "varchar(250)")]
        public string FullName { get; set; }

        [Column(TypeName = "varchar(10)")]
        public string Abbreviation { get; set; }

        [Column(TypeName = "varchar(10)")]
        public string Article { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        [JsonIgnore]
        public Committee ResolutlyCommittee { get; set; }

        [NotMapped]
        public string ResolutlyCommitteeId => ResolutlyCommittee?.ResolutlyCommitteeId ?? "";

        [NotMapped]
        public string ConferenceId => Conference?.ConferenceId ?? "";

        [Newtonsoft.Json.JsonIgnore]
        [JsonIgnore]
        public Conference Conference { get; set; }

        public List<CommitteeTopic> Topics { get; set; }

        [Timestamp]
        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public byte[] CommitteeTimestamp { get; set; }

        public Committee()
        {
            CommitteeId = Guid.NewGuid().ToString();
        }
    }
}
