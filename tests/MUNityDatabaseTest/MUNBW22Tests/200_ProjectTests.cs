using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Database.Test.MUNBW22Tests
{
    public partial class FullMUNBW22Tests
    {

        [Test]
        [Order(200)]
        public void TestAddMUNBWProject()
        {
            var tonyStark = _context.Users.FirstOrDefault(n => n.UserName == "tonystark");
            Assert.NotNull(tonyStark);
            var project = _context.Fluent.Project.AddProject(options =>
                options.WithShort("MUNBW")
                    .WithName("Model United Nations Baden-Würrtemberg")
                    .WithOrganization("dmunev")
                    .WithCreationUser(tonyStark));
            Assert.NotNull(project);
            Assert.AreEqual(1, _context.Projects.Count());
            Assert.IsTrue(_context.Projects.Any(n => n.ProjectId == "munbw"));
        }

        [Test]
        [Order(201)]
        public void TestLoadingDmunContainsProject()
        {
            var dmun = _context.Organizations
                .Include(n => n.Projects)
                .FirstOrDefault(n => n.OrganizationId == "dmunev");
            Assert.AreEqual(1, dmun.Projects.Count);
        }
    }
}
