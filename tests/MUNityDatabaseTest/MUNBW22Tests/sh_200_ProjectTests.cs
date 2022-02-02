using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace MUNity.Database.Test.MUNBW22Tests;

public partial class FullDMUN2022Tests
{
    [Test]
    [Order(2200)]
    public void TestAddMUNSHProhject()
    {
        var tonyStark = TestUsers.Avangers.FoundingMembers.TonyStark;
        var project = _context.Fluent.Project.AddProject(options =>
            options.WithShort("MUN-SH")
                .WithName("Model United Nations Schleswig-Holstein")
                .WithOrganization("dmunev")
                .WithCreationUser(tonyStark));
        Assert.NotNull(project);
        Assert.AreEqual(2, _context.Projects.Count());
        Assert.IsTrue(_context.Projects.Any(n => n.ProjectId == "munsh"));
    }
}
