using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUNity.Database.General;

namespace MUNity.Database.Extensions
{
    public static class CountryExtensions
    {
        public static Country AddTranslation(this Country country, string languageCode, string translation, string longName = null)
        {
            country.Translations.Add(new CountryNameTranslation(country, languageCode, translation, longName));
            return country;
        }
    }
}
