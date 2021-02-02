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

       
    }
}
