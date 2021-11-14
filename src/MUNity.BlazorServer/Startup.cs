using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MUNity.BlazorServer.Areas.Identity;
using MUNity.Database.Context;
using MUNity.Database.Models.User;
using MUNity.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Radzen;

namespace MUNity.BlazorServer;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
        // Blazor stuff
        services.AddRazorPages();
        services.AddServerSideBlazor();
        //Trigger deploy
        // Database
        // To get more output add: .EnableSensitiveDataLogging().LogTo(Console.WriteLine) after Use...()
        // Set the Data Source to: ../../tests/MUNityDatabaseTest/bin/Debug/net6.0/testmunbw.db
        // For demo use the name demo.db
        // to use the latest result of the MUNBW test 
        services.AddDbContext<MunityContext>(options =>
            options.UseSqlite("Data Source=demo.db"));

        // Identity
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
            .AddDefaultUI()
            .AddDefaultTokenProviders();



        //services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<MunityUser>>();
        services.AddDatabaseDeveloperPageExceptionFilter();

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
        services.AddScoped<ConferenceApplicationService>();
        services.AddScoped<IMailService, MailService>();
        services.AddScoped<ConferenceWebsiteService>();

        services.AddLogging();

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapBlazorHub();
            endpoints.MapFallbackToPage("/_Host");
        });

        MigrateDatabase(app);
    }

    private void MigrateDatabase(IApplicationBuilder app)
    {
        var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<MunityContext>();
        context.Database.EnsureCreated();
        context.Database.Migrate();
    }
}
