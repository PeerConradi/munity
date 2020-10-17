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

        [MaxLength(150)]
        public string TopicName { get; set; }

        [MaxLength(250)]
        public string TopicFullName { get; set; }

        public string TopicDescription { get; set; }

        [MaxLength(18)]
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
