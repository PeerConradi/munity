using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Moq;
using MUNityCore.Controllers;
using MUNityCore.DataHandlers.EntityFramework;
using MUNityCore.Hubs;
using MUNityCore.Schema.Request.Simulation;
using MUNityCore.Services;
using NUnit.Framework;

namespace MUNityTest.SimulationTest
{
    [TestFixture]
    public class SimulationControllerTest
    {
        private MunityContext _context;

        [OneTimeSetUp]
        public void Setup()
        {
            // Datenbank für den Test erzeugen und falls vorhanden erst einmal leeren und neu erstellen!
            var optionsBuilder = new DbContextOptionsBuilder<MunityContext>();
            optionsBuilder.UseSqlite("Data Source=test_simulation.db");
            _context = new MunityContext(optionsBuilder.Options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        [Test]
        [Order(0)]
        public void TestCreatingASimulation()
        {
            var mockHub = new Mock<IHubContext<SimulationHub, ITypedSimulationHub>>();
            var service = new SimulationService(_context);
            var controller = new SimulationController(mockHub.Object, service);
            var request = new SimulationRequests.CreateSimulation() {Name=  "Test", Password = "Password" };
            var result = controller.CreateSimulation(request);
            Assert.NotNull(result);
            
        }

    }
}