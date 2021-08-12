using Microsoft.EntityFrameworkCore;
using MUNity.Database.Models.User;
using MUNityCore.Models.User;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNityDatabaseTest.UserTests
{
    [TestFixture]
    public class MunityUserTests : AbstractDatabaseTests
    {

        [Test]
        [Order(0)]
        public void TestNoUserExists()
        {
            var hasUser = _context.Users.Any();
            Assert.IsFalse(hasUser);
        }

        [Test]
        [Order(1)]
        public void TestCreateARole()
        {
            var role = new MunityRole()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Head-Admin",
                NormalizedName = "HEAD-ADMIN"
            };

            _context.Roles.Add(role);
            _context.SaveChanges();
            Assert.AreEqual(1, _context.Roles.Count());
        }

        [Test]
        [Order(2)]
        public void TestCreateAnUser()
        {
            var user = new MunityUser()
            {
                Birthday = new DateTime(1990, 1, 1),
                Email = "test@mail.com",
                Forename = "Mike",
                Lastname = "Litoris",
                Gender = "-",
                UserName = "mikelitoris",
                UserState = MunityUser.EUserState.OK,
                NormalizedUserName = "MIKELITORIS",
                NormalizedEmail = "TEST@MAIL.COM"
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            Assert.AreEqual(1, _context.Users.Count());
        }

        [Test]
        [Order(3)]
        public void TestAddUserToRole()
        {
            var user = _context.Users.Include(n => n.UserRoles).ThenInclude(a => a.Role).FirstOrDefault();
            var role = _context.Roles.FirstOrDefault();
            Assert.NotNull(user);
            Assert.NotNull(role);
            user.UserRoles.Add(new MunityUserRole()
            {
                User = user,
                UserId = user.Id,
                Role = role,
                RoleId = role.Id
            });
            _context.SaveChanges();
        }

        [Test]
        [Order(4)]
        public void TestUserHasARole()
        {
            var user = _context.Users.Include(n => n.UserRoles).ThenInclude(a => a.Role).FirstOrDefault();
            Assert.IsTrue(user.UserRoles.Any());
            Assert.AreEqual("Head-Admin", user.UserRoles[0].Role.Name);
        }

        public MunityUserTests() : base ("user_tests.db")
        {

        }
    }
}
