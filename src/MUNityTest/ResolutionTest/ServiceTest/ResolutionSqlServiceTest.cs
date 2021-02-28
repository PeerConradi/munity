using MUNityCore.DataHandlers.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;

namespace MUNityCoreTest.ResolutionTest.ServiceTest
{
    public class ResolutionSqlServiceTest
    {
        private MunityContext _context;

        private MUNityCore.Services.SqlResolutionService _service;

        [OneTimeSetUp]
        public void Setup()
        {
            // Datenbank für den Test erzeugen und falls vorhanden erst einmal leeren und neu erstellen!
            var optionsBuilder = new DbContextOptionsBuilder<MunityContext>();
            optionsBuilder.UseSqlite("Data Source=test_resolutions.db");
            _context = new MunityContext(optionsBuilder.Options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            _service = new MUNityCore.Services.SqlResolutionService(_context);
        }

        [Test]
        [Order(0)]
        public async Task TestCreateNew()
        {
            await _service.CreatePublicResolution("Test");
            Assert.IsTrue(_context.Resolutions.Any());
            Assert.IsTrue(_context.Resolutions.Any(n => n.Topic == "Test"));
        }
    }
}
