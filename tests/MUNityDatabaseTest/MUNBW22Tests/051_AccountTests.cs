using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MUNity.Database.Extensions;
using MUNity.Database.Models.User;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Database.Test.MUNBW22Tests;

public partial class FullMUNBW22Tests
{
    [Test]
    [Order(51)]
    public async Task TestRegisterUsers()
    {
        var userManager = _serviceProvider.GetRequiredService<UserManager<MunityUser>>();

        foreach (var user in TestUsers.AllUsers)
        {
            var creationResult = await userManager.CreateAsync(user, "Passwort123");
            Assert.IsTrue(creationResult.Succeeded);
        }
    }

    [Test]
    [Order(52)]
    public async Task TestCreateShadowAccount()
    {
        var userManager = _serviceProvider.GetRequiredService<UserManager<MunityUser>>();

        var result = await userManager.CreateShadowUser("invited@mail.com");
        Assert.IsTrue(result.Result.Succeeded);
        Assert.NotNull(result.User.UserName);
        Assert.IsTrue(result.User.IsShadowUser, "Expected this user to be virtual.");
        Assert.IsFalse(string.IsNullOrEmpty(result.User.InviteSecret), "Epected an invite key to be created, when creating a shadow user, so it can be send for the first login.");
    }
}
