using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Util.Extenstions
{
    public static class ConvertExtensions
    {
        public static int ToIntOrDefault(this string s, int defaultValue)
        {
            if (int.TryParse(s, out int res)) {
                return res;
            }
            return defaultValue;
        }
    }
}
