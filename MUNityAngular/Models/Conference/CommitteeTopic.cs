using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Conference
{
    public class CommitteeTopic
    {
        public int CommitteeTopicId { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string TopicName { get; set; }

        [Column(TypeName = "varchar(250)")]
        public string TopicFullName { get; set; }

        [Column(TypeName = "varchar(600)")]
        public string TopicDescription { get; set; }

        [Column(TypeName = "varchar(25)")]
        public string TopicCode { get; set; }

        [Timestamp]
        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public byte[] CommiteeTopicTimestamp { get; set; }

        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public Committee Committee { get; set; }
    }
}
