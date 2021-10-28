using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MUNity.Database.Context;
using MUNity.Database.Models.User;
using MUNity.Services;
using Radzen;

namespace MUNity.BlazorServer.Tests
{
    public static class TestEnvironment
    {
        public static ServiceProvider Provider;

        public static ServiceProvider EnsureProvider()
        {
            if (Provider != null)
                return Provider;

            var services = new ServiceCollection();

            services.AddDbContext<MunityContext>(options =>
                options.UseSqlite("Data Source=e2e_test.db"));

            

            services.AddAuthentication();
            services.AddIdentity<MunityUser, MunityRole>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<MunityContext>()
                .AddDefaultTokenProviders();

            // Radzen.Blazor
            services.AddScoped<DialogService>();
            services.AddScoped<NotificationService>();
            services.AddScoped<TooltipService>();
            services.AddScoped<ContextMenuService>();

            // MUNity Services
            services.AddScoped<UserConferenceAuthService>();
            services.AddScoped<UserService>();
            services.AddScoped<OrganizationService>();
            services.AddScoped<ProjectService>();
            services.AddScoped<ConferenceService>();
            services.AddScoped<DelegationService>();
            services.AddScoped<ConferenceRoleService>();
            services.AddScoped<UserNotificationService>();

            services.AddLogging();

            Provider = services.BuildServiceProvider();

            var context = TestEnvironment.Provider.GetRequiredService<MunityContext>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            return Provider;
        }
    }
}