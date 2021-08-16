# MUNity Database

## About

MUNityDatabase is a project that is meant to only hold the Database-Model and Context for Entity-Framework.

The Model is splitted into different parts like: conference, organization, resolution, session, simulation, speakerlist and user.

You are allowed to add new categories to this system.

## Add A new Model

To add a new Database-Table and Model just look for the category you want to create it in. If no category fits your needs create a new one. Then create a new Table by creating a C#-Class.

Set the Namespace to ```MUNity.Database.Models.CATEGORYNAME```

to make use of some of the Entity-Framework Features you might want to add ```using System.ComponentModel.DataAnnotations;```.

To ad the new Model to the Database open the MunityContext.cs file and add your new Model as a Database-Set

```c#
public DbSet<NewModel> NewModels { get; set; }
```

> Models are named with singular form and the Tables are named with Plural.

Check out hte ```OnModelCreating``` Method inside the Context if you need to customize your model with the Fluent-API.

Then create a Database-Migration if needed.
Open up the terminal inside the Project and type in:
Add-Migration Migration-Name

Learn more: https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli

## Remove a Column

By default you should not remove Properties/Columns from