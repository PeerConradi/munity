using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MUNityCore.DataHandlers.EntityFramework;
using NUnit.Framework;

namespace MUNityCoreTest.ResolutionTest.ServiceTest
{
    public class TestChangeAndGet
    {
        private MunityContext _context;

        private MUNityCore.Services.SqlResolutionService _service;

        private string resolutionId;

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
            await _service.CreatePublicResolutionAsync("Test");
            Assert.IsTrue(_context.Resolutions.Any());
            Assert.IsTrue(_context.Resolutions.Any(n => n.Topic == "Test"));
            this.resolutionId = _context.Resolutions.FirstOrDefault()?.ResaElementId;
            Assert.NotNull(resolutionId);
            var dto = await _service.GetResolutionDtoAsync(this.resolutionId);
            Assert.NotNull(dto);
        }
    }
}
