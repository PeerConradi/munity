using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MUNityCore.Util.Extenstions
{
    public static class HttpHelperExtensions
    {
        public static string DecodeUrl(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            var realtext = System.Web.HttpUtility.UrlDecode(input);
            if (realtext == null)
                return string.Empty;

            if (realtext.EndsWith("|"))
                realtext = realtext.Substring(0, realtext.Length - 1);

            return realtext;
        }

        public static string ToUrlValid(this string input)
        {
            string customid = input.ToLower();  
            customid = customid.Replace(" ", "");
            customid = customid.Replace("ä", "ae");
            customid = customid.Replace("ö", "oe");
            customid = customid.Replace("ü", "ue");

            var possible = Regex.IsMatch(customid, @"^[a-z0-9]+$");
            if (possible)
            {
                return customid;
            }

            return null;
        }
    }
}
