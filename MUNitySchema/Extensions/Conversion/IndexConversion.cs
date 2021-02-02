using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Extensions.Conversion
{

    /// <summary>
    /// Extension Methods to Convert the Index of preamble Paragraphs or operative paragraphs to the default
    /// Schemas for example: 1.a.i, 1.a.ii.
    /// You could also use this Extensions to get all roman letters or numbers.
    /// </summary>
    public static class IndexConversion
    {

        /// <summary>
        /// Converts a given number between 1 and 3999 to the roman code.
        /// </summary>
        /// <param name="number">Integer between 1 and 3999</param>
        /// <returns></returns>
        public static string ToRoman(this int number)
        {
            if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");
            if (number < 1) return string.Empty;
            if (number >= 1000) return "M" + ToRoman(number - 1000);
            if (number >= 900) return "CM" + ToRoman(number - 900);
            if (number >= 500) return "D" + ToRoman(number - 500);
            if (number >= 400) return "CD" + ToRoman(number - 400);
            if (number >= 100) return "C" + ToRoman(number - 100);
            if (number >= 90) return "XC" + ToRoman(number - 90);
            if (number >= 50) return "L" + ToRoman(number - 50);
            if (number >= 40) return "XL" + ToRoman(number - 40);
            if (number >= 10) return "X" + ToRoman(number - 10);
            if (number >= 9) return "IX" + ToRoman(number - 9);
            if (number >= 5) return "V" + ToRoman(number - 5);
            if (number >= 4) return "IV" + ToRoman(number - 4);
            if (number >= 1) return "I" + ToRoman(number - 1);
            throw new ArgumentOutOfRangeException("something bad happened");
        }

        /// <summary>
        /// Returns a given array of numbers to the path of operative amendments.
        /// For example: 1, 1.a, 1.a.i 1.a.ii, 1.a.ii.1 etc.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToPathname(this int[] input)
        {
            var path = "";

            for (int i = 0; i < input.Length; i++)
            {
                // [1].a.ii
                if (i == 0 || i % 3 == 0)
                {
                    path += "." + (input[i] + 1).ToString();
                }
                else if (i == 1 || i % 3 == 1)
                {
                    path += "." + input[i].ToLetter();
                }
                else
                {
                    path += "." + (input[i] + 1).ToRoman().ToLower();
                }
            }
            if (path.StartsWith("."))
                path = path.Substring(1);

            return path;
        }

        /// <summary>
        /// returns a given number into a letter for example 0 = a, 1=b, 26=aa, 27=ab...
        /// </summary>
        /// <param name="number">The input number that should be converted it has to be zero or higher</param>
        /// <returns></returns>
        public static string ToLetter(this int number)
        {
            return NumberAsLetter(number);
        }

        private static string NumberAsLetter(int index)
        {
            int quotient = index / 26;
            if (quotient > 0)
                return NumberAsLetter(quotient - 1) + chars[index % chars.Length].ToString();
            else
                return chars[index % chars.Length].ToString();
        }

        private static readonly char[] chars = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
    }
}
