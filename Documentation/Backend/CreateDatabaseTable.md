# Create new Database Table (Object)

MUNity uses the EntityFramework as an ORM.

## Create the Model

You first need to create a model for your new table. Create Columns by creating different properties. You can create these Models inside the folder Models, create a new folder if you want to add multiple things.

You can add different attribute do set how the model is saved. The most importand is the ```[MaxLength(x)]``` attribute. This sets the max Length of saved nvarchar. MUNity mostly uses ```[MaxLength(250)]``` for long strings (full name) and ```[MaxLength(150)]``` for short string (name).

Its also importand that you either name your PrimaryKey field your classname + "Id" (if your class is called Car your Primary Key proeprty should be called ```CarId```) or you mark the Primary Key with the ```[Key]``` attribute.

You can also set the Primary Key with the FluentAPI inside the Database Context.

## Register Model inside the Database Context

You can find the Context inside the folder __DataHandlers/EntityFramework__

To add you model add it as new DbSet

MUNityCore.DataHandlers.EntityFramework.MunityContext
```csharp
// Table Names are mostly the classname as plural.
// For example if the classname is Car the Dataset should be called
// Cars.
public DbSet<YourClass> TableName {get; set;}

```

You can also add functionality to the OnModelCreate to create Foreign Keys or other constrains.

### Adding a custom Primary Key

```csharp
 protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Set a Property as Primary Key
    // This is not needed if your primary key is your classname + Id
    // Classname: Car, Primary Key Proeprty: CarId.
    modelBuilder.Entity<YourClassname>().HasKey(n => n.YourKeyPropertyName);

    // You can also set HasNoKey if your Model doesn't has a primary key
    modelBuilder.Entity<YouClassname>().HasNoKey();
}

```

## Refresh the Context

You can update the Migration by opening the Package-Manager Console and type

```
Add-Migration MIGRATIONNAME -Context MunityContext
```
_if you have added to the MunityContext_

or

```
Add-Migration MIGRATIONNAME -Context MunCoreContext
```

_if you have added to the Core Context.
