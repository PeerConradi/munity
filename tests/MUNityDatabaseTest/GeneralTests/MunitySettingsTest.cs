using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUNity.Database.Context;
using MUNity.Database.Models;
using NUnit.Framework;

namespace MUNity.Database.Test.GeneralTests;

public class MunitySettingsTest
{
    private MunityContext _context;

    [SetUp]
    public void SetUp()
    {
        this._context = MunityContext.FromSqlLite("settingTest");
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
    }

    [Test]
    public void TestCanCreateInstalledSetting()
    {
        _context.Settings.Add(new MunitySetting()
        {
            SetttingName = "Installed",
            SettingValue = "1",
            ChangeDate = new DateTime(2021, 1, 1, 12, 0, 0),
            SetBy = null
        });
        _context.SaveChanges();
        var recallSetting = _context.Settings.FirstOrDefault(n => n.SetttingName == "Installed");
        Assert.NotNull(recallSetting);
        Assert.AreEqual("Installed", recallSetting.SetttingName);
        Assert.AreEqual("1", recallSetting.SettingValue);
        Assert.AreEqual(new DateTime(2021, 1, 1, 12, 0, 0), recallSetting.ChangeDate);
        Assert.IsNull(recallSetting.SetBy);
    }
}
