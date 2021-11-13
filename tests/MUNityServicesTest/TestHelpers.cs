using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MUNity.Database.Context;
using MUNity.Database.Models.User;
using Moq;

namespace MUNity.Services.Test;

public class TestHelpers
{
    private static Mock<IMailService> mockedMailService;

    public static Mock<IMailService> MockedMailService
    {
        get
        {
            if (mockedMailService == null)
            {
                mockedMailService = new Mock<IMailService>();
                mockedMailService.Setup(n => n.SendMail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
            }
            return mockedMailService;
        }
    }

    public static MunityContext CreateSQLiteContext(string name)
    {
        // Datenbank für den Test erzeugen und falls vorhanden erst einmal leeren und neu erstellen!
        var optionsBuilder = new DbContextOptionsBuilder<MunityContext>();
        optionsBuilder.UseSqlite($"Data Source={name}.db");
        var context = new MunityContext(optionsBuilder.Options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        return context;
    }

    public static ServiceProvider GetCompleteProvider()
    {
        var mail = MockedMailService.Object;

        var services = new ServiceCollection();
        //this.context = TestHelpers.CreateSQLiteContext("orga_tests");
        services.AddDbContext<MunityContext>(options =>
        {
            options.UseSqlite("Data Source=testdb.db");
        });

        var orgaLogger = new Helpers.TestLogger<OrganizationService>();
        services.AddSingleton(orgaLogger);
        services.AddScoped<UserService>();
        services.AddScoped<OrganizationService>();
        services.AddSingleton(mail);
        services.AddIdentityCore<MunityUser>()
            .AddRoles<MunityRole>()
            .AddEntityFrameworkStores<MunityContext>();
        services.AddLogging();
        var provider = services.BuildServiceProvider();

        var context = provider.GetRequiredService<MunityContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        return provider;
    }
}
