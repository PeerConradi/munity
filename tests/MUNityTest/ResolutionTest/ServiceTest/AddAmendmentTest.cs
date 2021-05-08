using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUNityCore.DataHandlers.EntityFramework;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using MUNityCore.Models.Resolution.SqlResa;

namespace MUNityCoreTest.ResolutionTest.ServiceTest
{
    public class AddAmendmentTest
    {
        private MunityContext _context;

        private MUNityCore.Services.SqlResolutionService _service;

        [SetUp]
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
        public async Task TestVirtualParagraphIsBetween()
        {
            string resolutionId = await _service.CreatePublicResolutionAsync("Test");
            _service.CreateOperativeParagraph(resolutionId, "Paragraph 1");
            _service.CreateOperativeParagraph(resolutionId, "Paragraph 2");
            _service.CreateAddAmendment(resolutionId, 1, "Tester", "Virtual Paragraph after Paragraph 1");
            var reloadParagraphs = await _service.GetOperativeParagraphs(resolutionId);
            Assert.AreEqual(3, reloadParagraphs.Count);
            Assert.IsFalse(reloadParagraphs[0].IsVirtual);
            Assert.IsTrue(reloadParagraphs[1].IsVirtual);
            Assert.IsFalse(reloadParagraphs[2].IsVirtual);
        }


    }
}
