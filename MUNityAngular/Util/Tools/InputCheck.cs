using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MUNityAngular.Util.Tools
{
    public class InputCheck
    {
        public static bool IsOnlyCharsAndNumbers(string input)
        {
            return Regex.IsMatch(input, @"^[a-zA-Z0-9]+$");
        }
    }
}
