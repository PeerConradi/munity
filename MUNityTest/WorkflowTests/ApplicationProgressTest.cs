using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MUNityCore.DataHandlers.EntityFramework;
using MUNityCore.Models.Conference;
using MUNityTest.TestEnvironment;
using NUnit.Framework;

namespace MUNityTest.WorkflowTests
{
    [TestFixture]
    public class ApplicationProgressTest
    {
        private static MunCoreContext _context;

        private TestEnvironment.ConferenceEnvironment _environment;

        [OneTimeSetUp]
        public void Setup()
        {
            Console.WriteLine("Setup Test Environment");
            // Datenbank für den Test erzeugen und falls vorhanden erst einmal leeren und neu erstellen!
            var optionsBuilder = new DbContextOptionsBuilder<MunCoreContext>();
            optionsBuilder.UseSqlite("Data Source=test_application.db");
            _context = new MunCoreContext(optionsBuilder.Options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            _environment = new ConferenceEnvironment(_context);
        }

        /// <summary>
        /// With this test you can see how a user can apply directly for a desired role inside
        /// the conference.
        /// </summary>
        [Test]
        public void TestCanMaxApplyForSecretaryLeader()
        {
            var application = new RoleApplication();
            application.User = _environment.TestUserMax;
            application.Role = _environment.TestRoleSecretaryLeader;
            application.Title = "Bewerbung für Sekretariatsleitung";
            application.Content =
                "Hallo Projektleitung, \n hiermit möchte ich mich für die Rolle der Sekretariatsleitung bewerben. Bla bla bin sehr gut bla.";
            application.ApplyDate = DateTime.Now;
            _context.RoleApplications.Add(application);
            _context.SaveChanges();
            Assert.AreEqual(1, _context.RoleApplications.Count());
        }

        /// <summary>
        /// This test shows, that two users can get together to apply for the same Role.
        /// In this example for a Role of a Print Journalist.
        /// They share the same application Letter.
        /// </summary>
        [Test]
        public void TestCanMillieAndFinnApplyForPressRole()
        {
            var application = new GroupApplication();
            application.Role = _environment.TestPressRole;
            application.Users.Add(_environment.TestUserMillie);
            application.Users.Add(_environment.TestUserFinn);
            application.ApplicationDate = DateTime.Now;
            application.Title = "Gruppenbewerbung für Print-Presse";
            application.Content =
                "Hallo wir sind Millie und Finn und wollen gerne zusammen als Presseteilnehmer an der Konferenz teilnehmen.";
            _context.GroupApplications.Add(application);
            _context.SaveChanges();
            Assert.AreEqual(1, _context.GroupApplications.Count());
            var _reloadApplication = _context.GroupApplications.First();
            Assert.NotNull(_reloadApplication);
            Assert.AreEqual(2, _reloadApplication.Users.Count);
        }

        [OneTimeTearDown]
        public void BurnTheEvidence()
        {
            _context?.Database?.EnsureDeleted();
        }
    }
}