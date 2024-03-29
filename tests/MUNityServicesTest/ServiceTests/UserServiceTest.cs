﻿using Microsoft.Extensions.DependencyInjection;
using MUNity.Database.Context;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MUNity.Services.Test.ServiceTests;

public class UserServiceTest
{
    private IServiceProvider provider;

    [OneTimeSetUp]
    public void Setup()
    {
        provider = TestHelpers.GetCompleteProvider();
    }

    [Test]
    [Order(0)]
    public async Task TestCreateAnAccount()
    {
        var service = provider.GetRequiredService<UserService>();
        var user = await service.CreateUser("tester", "test@mail.com", "password");
        Assert.NotNull(user);
    }

    [Test]
    [Order(1)]
    public void TestUserIsInDatabase()
    {
        var context = provider.GetRequiredService<MunityContext>();
        var user = context.Users.FirstOrDefault(n => n.UserName == "tester");
        Assert.NotNull(user);
        Assert.IsFalse(string.IsNullOrEmpty(user.PasswordHash));
    }

    [Test]
    [Order(2)]
    public void TestUserCanLogin()
    {
        var userService = provider.GetRequiredService<UserService>();
        var result = userService.LoginValid("tester", "password");
        Assert.IsTrue(result);
    }
}
