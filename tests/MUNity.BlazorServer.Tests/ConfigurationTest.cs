using System;
using Bunit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using MUNity.Database.Context;
using MUNity.Database.Models.User;
using MUNity.Services;
using NUnit.Framework;

namespace MUNity.BlazorServer.Tests
{
    public partial class CompleteSystemTest
    {

        [Test]
        [Order(1)]
        public void TestSetupServices()
        {
            var services = new ServiceCollection();

            services.AddDbContext<MunityContext>(options =>
                options.UseSqlite("Data Source=e2e_test.db"));

            //services.AddAuthentication();
            //services.AddIdentity<MunityUser, MunityRole>(options =>
            //    {
            //        options.User.RequireUniqueEmail = true;
            //        options.Password.RequireDigit = true;
            //        options.Password.RequireLowercase = true;
            //        options.Password.RequireUppercase = true;
            //        options.Password.RequireNonAlphanumeric = false;
            //    })
            //    .AddEntityFrameworkStores<MunityContext>()
            //    .AddDefaultUI()
            //    .AddDefaultTokenProviders();

            services.AddScoped<UserConferenceAuthService>();
            services.AddScoped<UserService>();
            services.AddScoped<OrganizationService>();
            services.AddScoped<ProjectService>();
            services.AddScoped<ConferenceService>();
            services.AddScoped<DelegationService>();
            services.AddScoped<ConferenceRoleService>();
            TestEnvironment.Provider = services.BuildServiceProvider();
            Assert.NotNull(TestEnvironment.Provider);
        }

        [Test]
        [Order(2)]
        public void TestMigrateTheDatabase()
        {
            var context = TestEnvironment.Provider.GetRequiredService<MunityContext>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        //[Test]
        //[Order(3)]
        //public void TestRegister()
        //{
        //    using var ctx = new Bunit.TestContext();
        //    var index = ctx.RenderComponent<Register>();
        //    //index.MarkupMatches("");
        //    var containsHello = index.Markup.Contains("<h1>Hallo MUNity.</h1>");
        //    Assert.IsTrue(containsHello);
        //}

        //[Test]
        //public void Test1()
        //{
        //    using var ctx = new Bunit.TestContext();
        //    var index = ctx.RenderComponent<MUNity.BlazorServer.Pages.Index>();
        //    //index.MarkupMatches("");
        //    var containsHello = index.Markup.Contains("<h1>Hallo MUNity.</h1>");
        //    Assert.IsTrue(containsHello);
        //}
    }
}
