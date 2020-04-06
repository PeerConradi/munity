using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.DataHandlers.EntityFramework.Models;

namespace MUNityAngular.Util.Extenstions
{
    public static class ConvertExtensions
    {
        public static int ToIntOrDefault(this string s, int defaultValue = -1)
        {
            if (int.TryParse(s, out int res)) {
                return res;
            }
            return defaultValue;
        }

        public static TimeSpan? ToTimeSpan(this string s)
        {
            if (s.ToCharArray().Count(n => n == ':') == 2)
            {
                int hours = 0;
                int minutes = 0;
                int seconds = 0;
                int.TryParse(s.Split(':')[0], out hours);
                int.TryParse(s.Split(':')[1], out minutes);
                int.TryParse(s.Split(':')[2], out seconds);
                return new TimeSpan(hours, minutes, seconds);
            }

            return null;
        }
    }
}
