using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MUNity.Base;
using MUNity.Database.Context;
using MUNity.Database.General;

namespace MUNity.Database.Extensions;

public static class CountryExtensions
{
    public static Country AddTranslation(this Country country, string languageCode, string translation, string longName = null)
    {
        country.Translations.Add(new CountryNameTranslation(country, languageCode, translation, longName));
        return country;
    }

    /// <summary>
    /// Adds the countries that are given to the Database. If a country already has the given name it
    /// will just update the country.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="countries"></param>
    /// <returns></returns>
    public static int AddBaseCountries(this MunityContext context, IEnumerable<Country> countries)
    {
        foreach (var country in countries)
        {
            var matchingCountry = context.Countries.Include(n => n.Translations).FirstOrDefault(n =>
                n.Name == country.Name);
            if (matchingCountry != null)
            {
                // Update the country
                matchingCountry.FullName = country.FullName;
                matchingCountry.Iso = country.Iso;
                if (country.Continent != EContinent.NotSet)
                    matchingCountry.Continent = country.Continent;

                if (country.Translations.Count > 0)
                {
                    foreach (var translation in country.Translations)
                    {
                        var foundTranslation =
                            matchingCountry.Translations.FirstOrDefault(n =>
                                n.LanguageCode == translation.LanguageCode);
                        if (foundTranslation != null)
                        {
                            foundTranslation.TranslatedName = translation.TranslatedName;
                            foundTranslation.TranslatedFullName = translation.TranslatedFullName;
                        }
                    }
                }
            }
            else
            {
                context.Countries.Add(country);
            }
        }

        return context.SaveChanges();
    }


}
