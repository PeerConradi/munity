using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MUNityAngular.DataHandlers.EntityFramework;
using MUNityAngular.Services;
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
            var user = service.CreateUser("MikeLitoris", "123456", "test@mail.com", new DateTime(1990, 1, 1));
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
    }
}