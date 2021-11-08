using System;
using MUNity.Base;
using MUNity.Database.Models.Conference;

namespace MUNity.Database.FluentAPI;

public class CommitteeOptionsBuilder
{
    public Committee Committee { get; }

    public CommitteeOptionsBuilder WithName(string name)
    {
        Committee.Name = name;
        return this;
    }

    public CommitteeOptionsBuilder WithFullName(string fullName)
    {
        Committee.FullName = fullName;
        return this;
    }

    public CommitteeOptionsBuilder WithShort(string shortName)
    {
        Committee.CommitteeShort = shortName;
        return this;
    }

    public CommitteeOptionsBuilder WithType(CommitteeTypes type)
    {
        Committee.CommitteeType = type;
        return this;
    }

    public CommitteeOptionsBuilder WithTopic(Action<CommitteeTopicOptionsBuilder> options)
    {
        var builder = new CommitteeTopicOptionsBuilder();
        options(builder);
        Committee.Topics.Add(builder.Topic);
        return this;
    }

    public CommitteeOptionsBuilder WithTopic(string name)
    {
        return WithTopic(options => options.WithName(name));
    }

    public CommitteeOptionsBuilder WithSubCommittee(Action<CommitteeOptionsBuilder> options)
    {
        var builder = new CommitteeOptionsBuilder();
        options(builder);
        Committee.ChildCommittees.Add(builder.Committee);
        builder.Committee.ResolutlyCommittee = Committee;
        builder.Committee.Conference = this.Committee.Conference;
        return this;
    }

    public CommitteeOptionsBuilder()
    {
        this.Committee = new Committee();
    }
}
