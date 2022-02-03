using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MUNity.Services;
using Radzen;
using Microsoft.AspNetCore.Identity;
using MUNity.Database.Context;
using MUNity.Database.Models.User;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Blazor stuff
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

//Trigger deploy
// Database
// To get more output add: .EnableSensitiveDataLogging().LogTo(Console.WriteLine) after Use...()
// Set the Data Source to: ../../tests/MUNityDatabaseTest/bin/Debug/net6.0/testmunbw.db
// For demo use the name demo.db
// to use the latest result of the MUNBW test 
builder.Services.AddDbContext<MunityContext>(options =>
    options.UseSqlite("Data Source=demo.db"));

// Identity
builder.Services.AddAuthentication();
builder.Services.AddIdentity<MunityUser, MunityRole>(options =>
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
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Radzen.Blazor
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();

// MUNity Services
builder.Services.AddScoped<UserConferenceAuthService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<OrganizationService>();
builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<ConferenceService>();
builder.Services.AddScoped<DelegationService>();
builder.Services.AddScoped<ConferenceRoleService>();
builder.Services.AddScoped<UserNotificationService>();
builder.Services.AddScoped<ConferenceApplicationService>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<ConferenceWebsiteService>();
builder.Services.AddSingleton<ConferenceApplicationSortingService>();
builder.Services.AddSingleton<ListOfSpeakersService>();
builder.Services.AddScoped<ListOfSpeakersDatabaseService>();

builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
