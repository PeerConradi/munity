# Participation

## About

Users can participate in different conferences. They can also take multiple Roles inside a conference.

MUNity thinks of Roles like Slots and fills them with users inside a participation. The participation then also contains some more information, for example how much the user has to pay in total and how much of it has already been paid.

So a role can be carried out by multiple users. To make sure to handle the application process and not give a role two multiple users if you dont want to you should use the ApplicationState inside the AbstractRole. This contains the DirectApplication where a user can only directly apply for this role. When this is set and there is a user participaing in this role, the Slot is taken.

The participation is the user already taking a role. The role itself is the slot.

## Create a participation

```c#
var participation = new Participation()
{
    Cost = 50,
    IsMainRole = false,
    Paid = 0,
    Role = teamRole,    // Set any type of role
    User = user         // a MUNityUser
};
_context.Participations.Add(participation);
_context.SaveChanges();
```

## Get all the participations of a conference

```c#
var participationSlots = _context.Participations
    .Where(n => n.Role.Conference.ConferenceId == TestConference.ConferenceId);
```

## Get All the users participation in a conference

With this code snipped you can get all the users that have some kind of role inside the conference.

```C#
var usersOfConference = _context.Participations
    .Where(n => n.Role.Conference.ConferenceId == "ID HERE")
    .Select(n => n.User)
    .Distinct();
```
## Get all users that are participating and inside the Team

```c#
var usersInTeam = _context.Participations
    .Where(n => n.Role.Conference.ConferenceId == "ID HERE" &&
    n.Role is ConferenceTeamRole)
    .Select(n => n.User)
    .Distinct();
```
