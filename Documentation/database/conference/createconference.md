# Handle Conferences

> Note that this is a guide on how to work with the Database itself. Most things can be handled by using the different Services.

The basic structure in which conferences are stored is a followed:

An Organization has multiple projects. These could be conferences that repeat every year like: MUN Berlin 2020, MUN Berlin 2021 etc. The Project would be MUN Berlin. MUN Berlin 2020 and MUN Berlin 2021 are just conferences inside this project.

The tree could look like this:

* Organization: DMUN e.V.
  * Project: MUN-SH
    * Conference: MUN-SH 2020
    * Conference: MUN-SH 2021
  * Project: MUNBW
    * Conference: MUNBW 2020
    * Conference: MUNBW 2021

> The examples below use the __context__ this is an instance of the MunityContext that can be found here: [src/MUNityDatabase/Context/MunityContext.cs](../../../src/MUNityDatabase/Context/MunityContext.cs)

## Create the organization:

[Learn how to create organizations](../organization/createorganization.md)


## Create a new project

```c#
var project = new Project()
{
    // The full name of the project (Max Length 250 chars)
    ProjectName = "Model United Nations Schleswig-Holstein",
    // a short name of the project (Max Length 20)
    ProjectShort = "MUN-SH",
    // Set the organization that hosts this project.
    ProjectOrganization = orga,
};
context.Projects.Add(project);
context.SaveChanges();
```

The projectId is generated from GUID when the instance is created.

## Create the conference

```c#
var conference = new Conference()
{
    ConferenceProject = project,
    ConferenceShort = "MUN-SH 2022",
    Name = "MUN Schleswig-Holstein 2022",
    FullName = "Model United Nations Schleswig-Holstein 2022"
};
context.Conferences.Add(conference);
context.SaveChanges();
```

The conference has a lot more Properties

| Name | Type | Description |
|------|------|-------------|
| ConferenceId | string | The Id of the conference (primary key) |
| Name | string (max Length: 150) | A Display Name for the conference that is used for every headline etc. |
| FullName | string (max length: 250) | A full Version of the conference name. This name could for example contain the whole name of the conference with the name "Model United Nations" included. |
| ConferenceShort | string (max length: 18) | A short name for the conference. For example __MUN-SH 2021__ |
| StartDate | Nullable DateTime | The first day or day and time the conference starts. This value can be null if the date is not yet set. |
| EndDate | Nullable DateTime | The last day or day and time of the conference. Set to null if the date has to be announced. |
| CreationDate | DateTime | The time this conference was created. Set this value to DateTime.Now when creating a new conference. |
| CreationUser | MunityUser | The user that has created this conference. The user can be null but should not be in a realistic scenario|
| Committees | List of Committee | All the committees that are simulated in this conference. |
| Roles | List of AbstractRoles | All the roles the participants could have from Team to the Delegates, Press and Visitors. Read more at [Roles](roles.md). |
| ConferenceProject | Project | The project that this conference belongs to. |
| Visibility | EConferenceVisibilityMode | Sets who should be able to see the conference. 0 = Only the Participants with a munity account, 1 = Everyone with a munity account, 2 = everyone (public on the website/internet). |
| ConferenceTimestamp | byte-Array | The concurrency Token of the conference. This is handled by the Entity-Framework.