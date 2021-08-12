using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MUNity.Util
{
    public class IdGenerator
    {
        private static readonly Random _random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Turns the text into a lower case verison and removes all special characters
        /// so the value can be used as primary key (of string) that can also be used for an URL
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string AsPrimaryKey(string input)
        {
            input = input.ToLower();
            input = input.Replace("ä", "ae");
            input = input.Replace("ö", "oe");
            input = input.Replace("ü", "ue");
            input = Regex.Replace(input, "[^a-z0-9]", "");
            return input;
        }
    }
}
