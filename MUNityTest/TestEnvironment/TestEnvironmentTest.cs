using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MUNityCore.DataHandlers.EntityFramework;
using NUnit.Framework;

namespace MUNityTest.TestEnvironment
{
    [TestFixture]
    public class TestEnvironmentTest
    {
        private static MunCoreContext _context;

        [OneTimeSetUp]
        public void Setup()
        {
            // Datenbank für den Test erzeugen und falls vorhanden erst einmal leeren und neu erstellen!
            var optionsBuilder = new DbContextOptionsBuilder<MunCoreContext>();
            optionsBuilder.UseSqlite("Data Source=test_testenvironment.db");
            _context = new MunCoreContext(optionsBuilder.Options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        [Test]
        public void TestConferenceEnvironment()
        {
            var environment = new ConferenceEnvironment(_context);
            Assert.NotNull(environment);
            Assert.NotNull(environment.TestOrganization);
            Assert.NotNull(environment.TestProject);
            Assert.IsTrue(_context.Organisations.Contains(environment.TestOrganization));
            Assert.AreEqual(2, environment.TestConference.Committees.Count);
            Assert.AreEqual(2, _context.TeamRoles.Count(n => n.Conference == environment.TestConference));
            Assert.AreEqual(4, _context.Users.Count());
        }
    }
}