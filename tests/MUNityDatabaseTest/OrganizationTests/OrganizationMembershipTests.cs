using Microsoft.EntityFrameworkCore;
using MUNity.Database.Models.Organization;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNityDatabaseTest.OrganizationTests
{
    public class OrganizationMembershipTests : AbstractDatabaseTests
    {
        [Test]
        [Order(0)]
        public void TestBaseOrganizationCreated()
        {
            var organization = TestOrganization;
            Assert.NotNull(organization);
            Assert.AreEqual(1, _context.Organizations.Count());
            Assert.AreEqual("Deutsche Model United Nations e.V.", organization.OrganizationName);
            Assert.AreEqual("DMUN e.V.", organization.OrganizationShort);
        }

        [Test]
        [Order(1)]
        public void TestCreateOrganizationRole()
        {
            var role = new OrganizationRole()
            {
                CanCreateProject = true,
                Organization = TestOrganization,
                RoleName = "Member"
            };
            _context.OrganizationRoles.Add(role);
            _context.SaveChanges();
            Assert.AreEqual(1, _context.Organizations
                .Where(n => n.OrganizationId == TestOrganization.OrganizationId)
                .Select(n => n.Roles).Count());
        }

        [Test]
        [Order(2)]
        public void AddUserToOrganization()
        {
            var user = CreateATestUser("testUser");
            var role = _context.OrganizationRoles
                .FirstOrDefault(n => n.Organization.OrganizationId == TestOrganization.OrganizationId);
            var membership = new OrganizationMember()
            {
                JoinedDate = DateTime.Now,
                Organization = TestOrganization,
                Role = role,
                User = user
            };
            _context.OrganizationMember.Add(membership);
            _context.SaveChanges();
            Assert.NotNull(membership);
        }

        [Test]
        [Order(3)]
        public void TestGetUsersOfOrganization()
        {
            var users = _context.OrganizationMember.Where(n => n.Organization.OrganizationId == TestOrganization.OrganizationId);
            Assert.AreEqual(1, users.Count());
        }

        [Test]
        [Order(4)]
        public void TestGetRolesByOrganization()
        {
            var organizationWithRoles = _context.Organizations.Include(n => n.Roles).FirstOrDefault();
            Assert.AreEqual(1, organizationWithRoles.Roles.Count);
        }

        [Test]
        [Order(5)]
        public void TestGetMembersByOrganization()
        {
            var organizationWithMembers = _context.Organizations.Include(n => n.Member).FirstOrDefault();
            Assert.AreEqual(1, organizationWithMembers.Member.Count);
        }

        [Test]
        [Order(6)]
        public void TestMember()
        {
            var member = _context.OrganizationMember
                .Include(n => n.Organization)
                .Include(n => n.User)
                .Include(n => n.Role)
                .FirstOrDefault();
            Assert.AreEqual(1, member.OrganizationMemberId);
            Assert.NotNull(member.User);
            Assert.NotNull(member.Organization);
            Assert.NotNull(member.Role);
            Assert.GreaterOrEqual((DateTime.Now - member.JoinedDate).TotalMinutes, 0);
        }

        [Test]
        [Order(7)]
        public void TestRole()
        {
            var role = _context.OrganizationRoles
                .Include(n => n.Organization)
                .Include(n => n.MembersWithRole)
                .FirstOrDefault();
            Assert.NotNull(role);
            Assert.AreEqual(1, role.OrganizationRoleId);
            Assert.AreEqual("Member", role.RoleName);
            Assert.NotNull(role.Organization);
            Assert.AreEqual(1, role.MembersWithRole.Count);
        }

        public OrganizationMembershipTests() : base ("organizationtest.db")
        {

        }
    }
}
