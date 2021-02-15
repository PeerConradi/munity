using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUNityCore.Models.Conference
{

    /// <summary>
    /// A topic that this committee is discussing on the conference.
    /// Every committee can discuss different topics.
    /// </summary>
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
        public byte[] CommitteeTopicTimestamp { get; set; }


        public Committee Committee { get; set; }
    }
}
