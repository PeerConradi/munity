using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MUNityCore.DataHandlers.EntityFramework;
using MUNityCore.Models;
using MUNityCore.Models.User;
using MUNityCore.Services;
using MUNity.Schema.Authentication;
using MUNityTest.TestEnvironment;
using NUnit.Framework;

namespace MUNityTest.Service
{
    [TestFixture]
    public class AuthServiceTest
    {
        private static MunityContext _context;

        [OneTimeSetUp]
        public void Setup()
        {
            Console.WriteLine("Setup Test Environment");
            // Datenbank für den Test erzeugen und falls vorhanden erst einmal leeren und neu erstellen!
            var optionsBuilder = new DbContextOptionsBuilder<MunityContext>();
            optionsBuilder.UseSqlite("Data Source=test_authservice.db");
            _context = new MunityContext(optionsBuilder.Options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        //[Test]
        public void TestGenerateAuthkey()
        {
            var user = new MunityUser { UserName = "testuser", Forename = "Max", Lastname = "Mustermann"};
            var generatedPass = MUNityCore.Util.Hashing.PasswordHashing.InitHashing("password");
            user.PasswordHash = generatedPass.Key;
            user.SecurityStamp = generatedPass.Salt;

            _context.Users.Add(user);
            _context.SaveChanges();
            var settings = new AppSettings() { Secret = "This key needs to be really long to actually work"};
            var options = Options.Create(settings);
            var service = new AuthService(_context, options);

            var model = new AuthenticateRequest {Username = "test", Password = "password"};
            var tokenResult = service.Authenticate(model);
            Assert.NotNull(tokenResult);
            Assert.AreEqual(user.Forename, tokenResult.FirstName);
            Assert.AreEqual(user.Lastname, tokenResult.LastName);
            Assert.AreEqual(user.UserName, tokenResult.Username);
        }

        [Test]
        public void TestFailedLogin()
        {
            var settings = new AppSettings() { Secret = "This key needs to be really long to actually work" };
            var options = Options.Create(settings);
            var service = new AuthService(_context, options);

            var model = new AuthenticateRequest {Username = "dontexist", Password = "password"};
            var tokenResult = service.Authenticate(model);
            Assert.Null(tokenResult);
        }

        [Test]
        public void TestLoginInvalidPassword()
        {
            var settings = new AppSettings() { Secret = "This key needs to be really long to actually work" };
            var options = Options.Create(settings);
            var service = new AuthService(_context, options);
            var userService = new UserService(_context);
            userService.CreateUser("realuser","Max", "Mustermann", "realpass", "mail@rpovider.com", new DateTime(1990, 1, 1));

            var model = new AuthenticateRequest {Username = "realuser", Password = "password"};
            var tokenResult = service.Authenticate(model);
            Assert.Null(tokenResult);
        }

        [Test]
        public void TestUserCanEditConferece()
        {
            // Create the auth Service
            var settings = new AppSettings() { Secret = "This key needs to be really long to actually work" };
            var options = Options.Create(settings);
            var service = new AuthService(_context, options);

            var conferenceService = new ConferenceService(_context);

            // Create Test Conference Environment!
            var env = new ConferenceEnvironment(_context);

            // Let Max Mustermann be the leader
            conferenceService.Participate(env.TestUserMax, env.TestRoleProjectLeader);

            // Max should be able to edit the conference Settings
            Assert.True(service.CanUserEditConference(env.TestUserMax, env.TestConference));

            // Let Mike be a Press Role Participant
            conferenceService.Participate(env.TestUserMike, env.TestPressRole);

            // Mike should not be able to edit the conference Settings
            Assert.False(service.CanUserEditConference(env.TestUserMike, env.TestConference));

            // Milli is has two roles she is the leader and is also a paticipant inside the
            // PressTeam. This is unlikely to ever occur but this should cover that the highest of
            // your roles is considered for authentication!
            conferenceService.Participate(env.TestUserMillie, env.TestRoleProjectLeader);
            conferenceService.Participate(env.TestUserMillie, env.TestPressRole);
            Assert.True(service.CanUserEditConference(env.TestUserMillie, env.TestConference));
        }
    }
}