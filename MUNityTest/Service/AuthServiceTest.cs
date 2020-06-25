using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MUNityAngular.DataHandlers.EntityFramework;
using MUNityAngular.Models;
using MUNityAngular.Models.Core;
using MUNityAngular.Schema.Request.Authentication;
using MUNityAngular.Services;
using NUnit.Framework;

namespace MUNityTest.Service
{
    [TestFixture]
    public class AuthServiceTest
    {
        private static MunCoreContext _context;

        [OneTimeSetUp]
        public void Setup()
        {
            Console.WriteLine("Setup Test Environment");
            // Datenbank für den Test erzeugen und falls vorhanden erst einmal leeren und neu erstellen!
            var optionsBuilder = new DbContextOptionsBuilder<MunCoreContext>();
            optionsBuilder.UseSqlite("Data Source=test_authservice.db");
            _context = new MunCoreContext(optionsBuilder.Options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        [Test]
        public void TestGenerateAuthkey()
        {
            var user = new User();
            user.Username = "test";
            user.Forename = "Max";
            user.Lastname = "Mustermann";
            var generatedPass = MUNityAngular.Util.Hashing.PasswordHashing.InitHashing("password");
            user.Password = generatedPass.Key;
            user.Salt = generatedPass.Salt;

            _context.Users.Add(user);
            _context.SaveChanges();
            var settings = new AppSettings() { Secret = "This key needs to be really long to actually work"};
            var options = Options.Create(settings);
            var service = new AuthService(_context, options);

            var model = new AuthenticateRequest();
            model.Username = "test";
            model.Password = "password";
            var tokenResult = service.Authenticate(model);
            Assert.NotNull(tokenResult);
            Assert.AreEqual(user.Forename, tokenResult.FirstName);
            Assert.AreEqual(user.Lastname, tokenResult.LastName);
            Assert.AreEqual(user.Username, tokenResult.Username);
        }

        [Test]
        public void TestFailedLogin()
        {
            var settings = new AppSettings() { Secret = "This key needs to be really long to actually work" };
            var options = Options.Create(settings);
            var service = new AuthService(_context, options);

            var model = new AuthenticateRequest();
            model.Username = "dontexist";
            model.Password = "password";
            var tokenResult = service.Authenticate(model);
            Assert.Null(tokenResult);
        }
    }
}