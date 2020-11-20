# Create a Conference

In this guide we will take a look on how you can create a conference from the
API calls.

To create conferences we are using the ConferenceController that can be reached at 
```api/Conference```. Note that to create a conference you have to be logged in.

## Create from scretch

Before we can create a conference you need to create an Organisation and a project at that
organisation.

### Create an organisation

Call the ```OrganisationController``` at ```api/Organisation``` to Create an organisation.

When you create a new organisation you will automatically become the leader of this organisation. You can
change the organisation settings and userroles inside the organisation controller.

__API Call:__ api/Organisation/CreateOrganisation

__Request Body:__ 
```
{
    OrganisationName: string;
    Abbreviation: string;
}
```

> **_NOTE:_**  You have to be logged in to create an organisation!

### Create a project

The organisation can have multiple projects. To create a conference you have to have at least one project
you can assign your conference to. You need to be inside the organisation you want to create the project for and be allowed to create projects.

__API Call:__ api/Conference/CreateProject

__Request Body:__
```
{
    organisationId: string; // requiered!
    name: string;           // requiered!
    abbreviation: string;   // requiered!
}
```

### Create a conference

When creating the conference it will create the TeamRole: Leader and set you as this role. The Leader role has full access to change anything on this conference. Applications for this role are not allowed. By default the Conference is only visible to registered users.

You need to be inside the organisation that holds the project and be allowed to create projects inside this organisation

__API Call:__ api/Conference/CreateConference

__Request Body:__
```
    projectId: string;      // requiered!
    name: string;           // requiered!
    fullName: string        // requiered!
    abbreviation: string;   // requiered!
    startDate: Date;        // optional
    endDate: Date;          // optional
```


## Learn more

* [Conference Roles](Roles.md)