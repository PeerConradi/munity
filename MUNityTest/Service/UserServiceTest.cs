using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MUNityCore.DataHandlers.EntityFramework;
using MUNityCore.Services;
using NUnit.Framework;

namespace MUNityTest.Service
{
    [TestFixture]
    public class UserServiceTest
    {
        private static MunCoreContext _context;

        [OneTimeSetUp]
        public void Setup()
        {
            Console.WriteLine("Setup Test Environment");
            // Datenbank für den Test erzeugen und falls vorhanden erst einmal leeren und neu erstellen!
            var optionsBuilder = new DbContextOptionsBuilder<MunCoreContext>();
            optionsBuilder.UseSqlite("Data Source=test_userservice.db");
            _context = new MunCoreContext(optionsBuilder.Options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        [Test]
        [Order(1)]
        [Author("Peer Conradi")]
        [Description("Test Creating a new User")]
        public void TestCreateUser()
        {
            var service = new UserService(_context);
            var user = service.CreateUser("MikeLitoris", "Mike", "Litoris", "123456", "test@mail.com", new DateTime(1990, 1, 1));
            Assert.NotNull(user);
        }

        [Test]
        [Order(2)]
        [Author("Peer Conradi")]
        [Description("Test to get the User by a lower case written username")]
        public async Task TestGetUserByUsername()
        {
            var service = new UserService(_context);
            var user = await service.GetUserByUsername("mikelitoris");
            Assert.NotNull(user);
        }

        [Test]
        [Order(3)]
        [Author("Peer Conradi")]
        [Description("Getting the privacy settings of the user now should return null")]
        public async Task TestGetPrivacySettings()
        {
            var service = new UserService(_context);
            var user = await service.GetUserByUsername("mikelitoris");
            Assert.NotNull(user);
            var privacySettings = service.GetUserPrivacySettings(user);
            Assert.IsNull(privacySettings);
        }

        [Test]
        [Order(4)]
        [Author("Peer Conradi")]
        [Description("Init privacy settings should create a privacy settings element")]
        public async Task TestInitPrivacySettings()
        {
            var service = new UserService(_context);
            var user = await service.GetUserByUsername("mikelitoris");
            Assert.NotNull(user);
            var initSettings = service.InitUserPrivacySettings(user);
            Assert.NotNull(initSettings);
        }

        [Test]
        [Order(5)]
        [Author("Peer Conradi")]
        [Description("Getting the privacy settings now should return something")]
        public async Task TestGetPrivacySettingsAgain()
        {
            var service = new UserService(_context);
            var user = await service.GetUserByUsername("mikelitoris");
            Assert.NotNull(user);
            var privacySettings = service.GetUserPrivacySettings(user);
            Assert.NotNull(privacySettings);
        }
    }
}