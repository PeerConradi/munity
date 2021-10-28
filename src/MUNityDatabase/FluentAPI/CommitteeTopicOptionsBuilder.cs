using MUNity.Database.Models.Conference;

namespace MUNity.Database.FluentAPI
{
    public class CommitteeTopicOptionsBuilder
    {
        public CommitteeTopic Topic { get; }

        public CommitteeTopicOptionsBuilder WithName(string name)
        {
            if (string.IsNullOrEmpty(Topic.TopicFullName))
                Topic.TopicFullName = name;

            Topic.TopicName = name;
            return this;
        }

        public CommitteeTopicOptionsBuilder WithFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(Topic.TopicName))
                Topic.TopicName = fullName;

            Topic.TopicFullName = fullName;
            return this;
        }

        public CommitteeTopicOptionsBuilder WithDescription(string description)
        {
            Topic.TopicDescription = description;
            return this;
        }

        public CommitteeTopicOptionsBuilder WithCode(string code)
        {
            Topic.TopicCode = code;
            return this;
        }

        public CommitteeTopicOptionsBuilder()
        {
            this.Topic = new CommitteeTopic();
        }
    }
}
