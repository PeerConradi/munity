using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Conference
{
    public class CommitteeTopic
    {
        public int CommitteeTopicId { get; set; }

        public string TopicName { get; set; }

        public string TopicFullName { get; set; }

        public string TopicDescription { get; set; }

        public string TopicCode { get; set; }
    }
}
