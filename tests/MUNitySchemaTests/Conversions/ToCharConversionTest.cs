using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using MUNity.Extensions.Conversion;

namespace MunityNUnitTest.Conversions
{
    /// <summary>
    /// Tests for the index coverter.
    /// </summary>
    public class ToCharConversionTest
    {
        /// <summary>
        /// Test that the index 0 returns a when converted to a Letter.
        /// </summary>
        [Test]
        public void TestZeroReturnsA()
        {
            int i = 0;
            string res = i.ToLetter();
            Assert.AreEqual("a", res);
        }

        /// <summary>
        /// Test that the index 25 (26 entry when starting at zero) returns the letter z
        /// </summary>
        [Test]
        public void TestTwentyFiveIsZ()
        {
            int i = 25;
            string res = i.ToLetter();
            Assert.AreEqual("z", res);
        }

        /// <summary>
        /// Test that the Index 26 (27th entry when starting at zero) returns the combination aa.
        /// </summary>
        [Test]
        public void TestTwentySixIsAA()
        {
            int i = 26;
            string res = i.ToLetter();
            Assert.AreEqual("aa", res);
        }

        /// <summary>
        /// Test that the Index 27 (28 entry when starting at zero) returns the combination ab.
        /// </summary>
        [Test]
        public void TestTwentySevenIsAB()
        {
            int i = 27;
            string res = i.ToLetter();
            Assert.AreEqual("ab", res);
        }

        [Test]
        public void TestToRomainNegativNumber()
        {
            Assert.AreEqual("?", (-1).ToRoman());
        }

        [Test]
        public void TestToRomainForZero()
        {
            Assert.AreEqual("", 0.ToRoman());
        }

        [Test]
        public void TestNumber1000()
        {
            Assert.AreEqual("M", 1000.ToRoman());
        }

        [Test]
        public void TestNumber900()
        {
            Assert.AreEqual("CM", 900.ToRoman());
        }

        [Test]
        public void TestNumber500()
        {
            Assert.AreEqual("D", 500.ToRoman());
        }

        [Test]
        public void TestNumber400()
        {
            Assert.AreEqual("CD", 400.ToRoman());
        }

        [Test]
        public void TestNumber100()
        {
            Assert.AreEqual("C", 100.ToRoman());
        }

        [Test]
        public void TestNumber90()
        {
            Assert.AreEqual("XC", 90.ToRoman());
        }

        [Test]
        public void TestNumber50()
        {
            Assert.AreEqual("L", 50.ToRoman());
        }

        [Test]
        public void TestNumber40()
        {
            Assert.AreEqual("XL", 40.ToRoman());
        }

        [Test]
        public void TestNumber10()
        {
            Assert.AreEqual("X", 10.ToRoman());
        }

        [Test]
        public void TestNumber9()
        {
            Assert.AreEqual("IX", 9.ToRoman());
        }

        [Test]
        public void TestNumber5()
        {
            Assert.AreEqual("V", 5.ToRoman());
        }

        [Test]
        public void TestNumber4()
        {
            Assert.AreEqual("IV", 4.ToRoman());
        }

        [Test]
        public void TestNumber1()
        {
            Assert.AreEqual("I", 1.ToRoman());
        }

    }
}
