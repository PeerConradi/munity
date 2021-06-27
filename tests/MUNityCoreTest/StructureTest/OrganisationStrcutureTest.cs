using MUNityCore.Models.Organization;
using NUnit.Framework;

namespace MUNityTest.StructureTest
{
    [TestFixture]
    public class OrganisationStrcutureTest
    {
        [Test]
        public void TestCanHaveOrganisation()
        {
            var organisation = new Organization();
            organisation.OrganizationId = "dmun";
            organisation.OrganizationName = "Deutsche Model United Nations e.V.";
            organisation.OrganizationShort = "dmun e.V.";
            Assert.AreEqual("dmun", organisation.OrganizationId);
            Assert.AreEqual("Deutsche Model United Nations e.V.", organisation.OrganizationName);
            Assert.AreEqual("dmun e.V.", organisation.OrganizationShort);
        }
    }
}