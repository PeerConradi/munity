using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MUNityCore.Util.Tools
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
