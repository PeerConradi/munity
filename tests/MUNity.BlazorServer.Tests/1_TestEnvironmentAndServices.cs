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

    [TestFixture, Order(1)]
    public class CompleteSystemTest
    {

        [Test]
        [Order(1)]
        public void TestSetupServices()
        {
            TestEnvironment.EnsureProvider();
        }

        [Test]
        [Order(2)]
        public void TestUserManager()
        {
            var userManager = TestEnvironment.Provider.GetRequiredService<UserManager<MunityUser>>();
            Assert.NotNull(userManager);
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
