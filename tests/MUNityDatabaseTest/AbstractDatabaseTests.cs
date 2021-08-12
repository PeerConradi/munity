using Microsoft.EntityFrameworkCore;
using MUNity.Database.Context;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNityDatabaseTest
{
    public abstract class AbstractDatabaseTests
    {
        public MunityContext _context;

        string dbName = "munity_test.db";

        [OneTimeSetUp]
        public void Setup()
        {
            // Datenbank für den Test erzeugen und falls vorhanden erst einmal leeren und neu erstellen!
            var optionsBuilder = new DbContextOptionsBuilder<MunityContext>();
            optionsBuilder.UseSqlite("Data Source=test_conference.db");
            _context = new MunityContext(optionsBuilder.Options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        public AbstractDatabaseTests(string dataSourceName)
        {
            this.dbName = dataSourceName;
        }
    }
}
