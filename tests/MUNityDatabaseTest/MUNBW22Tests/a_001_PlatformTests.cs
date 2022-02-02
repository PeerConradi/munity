using MUNity.Database.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Database.Test.MUNBW22Tests;

public partial class FullDMUN2022Tests
{
    [Test]
    [Order(1)]
    public void TestSetupCountries()
    {
        var recaff = _context.SetupBaseCountries();
        Assert.NotZero(recaff);
        Assert.NotZero(_context.Countries.Count());
        Assert.NotZero(_context.CountryNameTranslations.Count());
    }

    [Test]
    [Order(2)]
    public void TestSetupBaseRoles()
    {
        var recaff = _context.SetupBaseRoles();
        Assert.NotZero(recaff);
        Assert.NotZero(_context.Roles.Count());
    }
}
