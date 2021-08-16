# Committees

Every conference can have multiple committees. This committees later contain participating users, topics that are discussed and resolutions.

## Create a new committee

The CommitteeId will be created when creating the instance by using a GUID.

```c#
var committee = new Committee()
{
    Conference = conference,
    Article = "die",
    FullName = "Generalversammlung",
    Name = "Generalversammlung"
};
context.Committees.Add(committee);
context.SaveChanges();
```

### Child Committees

Committees can have Parent Committees like this:

```c#
var subCommittee = new Committee()
{
    Article = "die",
    Name = "Abrüstungskommission",
    FullName = "Abrüstungskommission",
    // Set a Resolutly committee
    ResolutlyCommittee = parentCommittee
};
```

You can load them by Including the Child Committees.

```C#
_context.Committees.Include(n => n.ChildCommittees)
```

## Topics

The topics are what will be discussed inside a committee. Every committee can have multiple of this topics.

Create a new Topic:
```C#
var topic = new CommitteeTopic()
{
    Committee = committee,
    TopicCode = "WF",
    TopicName = "Weltfrieden",
    TopicFullName = "Maßnahmen zum sicherstellen des Friedens auf der Welt",
    TopicDescription = "In diesem Topic wird der Frieden auf der Erde debattiert. Hoffen wir mal das klappt, Krieg ist voll nicht spaßig."
};
_context.CommitteeTopics.Add(topic);
_context.SaveChanges();
```

## Session

Sessions are slots when the committee starts debating and may stopped a debate. The SessionId is a string that by Default will be a GUID. A good practice would be to create a string based on the conference.CommitteeShort.StartMonth.StartDay and then look at how many sessions already where at this day. For example: __MUN-SH2022.04.25.1__ and then the next session on that day would be __MUN-SH2022.04.25.2__.

The Session links to a checked attendance of the committee.

It also contains a protocol of the session.

### Create a Session

```C#
var committee = _context.Committees.FirstOrDefault();
var session = new CommitteeSession()
{
    Committee = committee,
    StartDate = DateTime.Now,
    EndDate = DateTime.Now.AddHours(1),
    Name = "Demo Session",
};
_context.CommitteeSessions.Add(session);
_context.SaveChanges();
```

