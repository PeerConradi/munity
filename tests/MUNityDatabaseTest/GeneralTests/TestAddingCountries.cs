using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MUNity.Database.General;
using MUNityBase;
using MUNityDatabaseTest;
using NUnit.Framework;
using MUNity.Database.FluentAPI;
using MUNity.Database.Extensions;

namespace MUNity.Database.Test.GeneralTests
{
    public class TestAddingCountries : AbstractDatabaseTests
    {
        public TestAddingCountries() : base("countries")
        {
            
        }

        [Test]
        [Order(1)]
        public void TestCreateACountryWithTranslation()
        {
            var country =
                new Country(1, EContinent.Africa, "Afghanistan", "Islamische Republik Afghanistan", "AF", true)
                    .AddTranslation("en-EN", "Afghanistan");
            _context.Countries.Add(country);
            _context.SaveChanges();
        }

        [Test]
        [Order(2)]
        public void TestCountryIsAdded()
        {
            Assert.IsTrue(_context.Countries.Any());
        }

        [Test]
        [Order(3)]
        public void TestTranslationIsAdded()
        {
            Assert.IsTrue(_context.CountryNameTranslations.Any());
        }

        [Test]
        [Order(4)]
        public void TestIdIsSet()
        {
            var country = _context.Countries.First();
            Assert.AreEqual(1, country.CountryId);
        }

        [Test]
        [Order(5)]
        public void TestContinentIsSet()
        {
            var country = _context.Countries.First();
            Assert.AreEqual(EContinent.Africa, country.Continent);
        }

        [Test]
        [Order(6)]
        public void TestNameIsSet()
        {
            var country = _context.Countries.First();
            Assert.AreEqual("Afghanistan", country.Name);
        }

        [Test]
        [Order(7)]
        public void TestFullNameIsSet()
        {
            var country = _context.Countries.First();
            Assert.AreEqual("Islamische Republik Afghanistan", country.FullName);
        }

        [Test]
        [Order(8)]
        public void TestIsIsSet()
        {
            var country = _context.Countries.First();
            Assert.AreEqual("AF", country.Iso);
        }

        [Test]
        [Order(9)]
        public void TestIsAccreditedIsSet()
        {
            var country = _context.Countries.First();
            Assert.IsTrue(country.IsAccredited);
        }

        [Test]
        [Order(11)]
        public void TestTranslationHasCountry()
        {
            var translation = _context.CountryNameTranslations
                .Include(n => n.Country).First();
            Assert.AreEqual(1, translation.Country.CountryId);
        }

        [Test]
        [Order(12)]
        public void TestTranslationHasLanguageCode()
        {
            var translation = _context.CountryNameTranslations
                .Include(n => n.Country).First();
            Assert.AreEqual("en-EN", translation.LanguageCode);
        }

        [Test]
        [Order(13)]
        public void TestTranslationHasName()
        {
            var translation = _context.CountryNameTranslations
                .Include(n => n.Country).First();
            Assert.AreEqual("Afghanistan", translation.TranslatedName);
        }

        [Test]
        [Order(14)]
        public void TestTranslationHasFullName()
        {
            var translation = _context.CountryNameTranslations
                .Include(n => n.Country).First();
            Assert.AreEqual("Afghanistan", translation.TranslatedFullName);
        }

        [Test]
        [Order(15)]
        public void TestRemovingCountry()
        {
            var country = _context.Countries.First();
            _context.Countries.Remove(country);
            _context.SaveChanges();
            Assert.IsFalse(_context.Countries.Any());
            Assert.IsFalse(_context.CountryNameTranslations.Any());
        }

        [Test]
        [Order(16)]
        public void TestAddBaseCountries()
        {
            foreach (var baseCountry in MUNity.Database.BaseData.Countries.BaseCountries)
            {
                if (_context.Countries.All(n => n.CountryId != baseCountry.CountryId))
                {
                    _context.Countries.Add(baseCountry);
                }
            }

            _context.SaveChanges();

            Assert.IsTrue(_context.Countries.Any());
        }
    }
}
