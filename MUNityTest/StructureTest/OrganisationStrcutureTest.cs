using MUNityAngular.Models.Organisation;
using NUnit.Framework;

namespace MUNityTest.StructureTest
{
    [TestFixture]
    public class OrganisationStrcutureTest
    {
        [Test]
        public void TestCanHaveOrganisation()
        {
            var organisation = new Organisation();
            organisation.OrganisationId = "dmun";
            organisation.OrganisationName = "Deutsche Model United Nations e.V.";
            organisation.OrganisationAbbreviation = "dmun e.V.";
            Assert.AreEqual("dmun", organisation.OrganisationId);
            Assert.AreEqual("Deutsche Model United Nations e.V.", organisation.OrganisationName);
            Assert.AreEqual("dmun e.V.", organisation.OrganisationAbbreviation);
        }
    }
}