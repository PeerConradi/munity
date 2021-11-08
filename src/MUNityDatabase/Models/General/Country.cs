using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;
using MUNityBase;

namespace MUNity.Database.General;

/// <summary>
/// a country is one of the countries of the United Nations
///
/// https://www.worldometers.info/united-nations/
/// </summary>
public class Country
{

    public short CountryId { get; set; }

    public EContinent Continent { get; set; }

    [MaxLength(250)]
    public string Name { get; set; }

    [MaxLength(350)]
    public string FullName { get; set; }

    [MaxLength(3)]
    public string Iso { get; set; }

    public bool IsAccredited { get; set; }

    public ICollection<CountryNameTranslation> Translations { get; set; }

    public Country(short id, EContinent continent, string name, string fullName, string iso, bool isAccredited = true)
    {
        this.CountryId = id;
        this.Continent = continent;
        this.Name = name;
        this.FullName = fullName;
        this.Iso = iso;
        this.IsAccredited = isAccredited;
        this.Translations = new List<CountryNameTranslation>();
    }

    public Country(EContinent continent, string name, string fullName, string iso, bool isAccredited = true)
    {
        this.Continent = continent;
        this.Name = name;
        this.FullName = fullName;
        this.Iso = iso;
        this.IsAccredited = isAccredited;
        this.Translations = new List<CountryNameTranslation>();
    }

    public Country()
    {
        this.Translations = new List<CountryNameTranslation>();
    }
}
