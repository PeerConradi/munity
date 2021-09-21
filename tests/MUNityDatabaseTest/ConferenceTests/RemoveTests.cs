using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MUNity.Database.Context;
using MUNity.Database.Models.Conference;
using MUNity.Database.Models.Organization;
using NUnit.Framework;

namespace MUNityDatabaseTest.ConferenceTests
{
    public class RemoveTests
    {
        private MunityContext _context;

        [SetUp]
        public void Setup()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MunityContext>();
            optionsBuilder.UseSqlite("Data Source=test_conference.db");
            _context = new MunityContext(optionsBuilder.Options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        [Test]
        [Description("Deleting Organizations this way should only set them as IsDeleted but not really delete them.")]
        public void TestRemovingAnOrganization()
        {
            var organization = new Organization();
            _context.Organizations.Add(organization);
            _context.SaveChanges();
            
            Assert.AreEqual(1, _context.Organizations.Count());

            _context.Organizations.Remove(organization);
            _context.SaveChanges();
            Assert.AreEqual(1, _context.Organizations.Count(n => n.IsDeleted));
        }

        [Test]
        public void TestFinalRemoveAnOrganization()
        {
            var organization = new Organization();
            _context.Organizations.Add(organization);
            _context.SaveChanges();

            Assert.AreEqual(1, _context.Organizations.Count());

            _context.Organizations.Remove(organization);
            _context.SaveChangesWithoutSoftDelete();
            Assert.AreEqual(0, _context.Organizations.Count());
        }

        [Test]
        public void TestRemovingAnOrganizationRemovesProjects()
        {
            var organization = new Organization();
            var project = new Project();
            organization.Projects.Add(project);
            _context.Organizations.Add(organization);
            _context.SaveChanges();

            Assert.AreEqual(1, _context.Organizations.Count());
            Assert.AreEqual(1, _context.Projects.Count());

            _context.Organizations.Remove(organization);
            _context.SaveChanges();
            Assert.AreEqual(1, _context.Organizations.Where(n => n.IsDeleted).Count());
            Assert.AreEqual(1, _context.Projects.Where(n => n.IsDeleted).Count());
        }
    }
}
