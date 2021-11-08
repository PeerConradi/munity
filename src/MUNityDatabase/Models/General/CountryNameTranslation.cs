using System.ComponentModel.DataAnnotations.Schema;

namespace MUNity.Database.General;

public class CountryNameTranslation
{

    public CountryNameTranslation(Country country, string languageCode, string translation, string translatedFullName = null)
    {
        this.Country = country;
        this.LanguageCode = languageCode;
        this.TranslatedName = translation;
        this.TranslatedFullName = translatedFullName ?? translation;
    }

    public CountryNameTranslation()
    {

    }

    public short CountryId { get; set; }

    public string LanguageCode { get; set; }

    [ForeignKey(nameof(CountryId))]
    public Country Country { get; set; }

    public string TranslatedName { get; set; }

    public string TranslatedFullName { get; set; }
}
