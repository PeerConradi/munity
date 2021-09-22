using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUNity.Database.Context;
using MUNity.Database.Extensions;
using MUNity.Database.Models.Organization;
using NUnit.Framework;

namespace MUNity.Database.Test.FluentTest
{
    public class OrganizationFluentTest
    {
        [Test]
        public void TestCanCreateOrganization()
        {
            var context = MunityContext.FromSqlLite("testfluent");
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            Assert.Zero(context.Organizations.Count());
            var orga = context.AddOrganization(orga =>
                orga.WithName("Deutsche Model United Nations e.V.")
                    .WithShort("DMUN e.V.")
                    .UseEasyId());
            Assert.NotZero(context.Organizations.Count());
            Assert.NotNull(orga);
        }


    }
}
