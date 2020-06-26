using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Conference
{
    public class Country
    {
        public enum EContinent
        {
            Africa,
            Antarctica,
            Asia,
            Australia,
            Europe,
            NorthAmerica,
            SouthAmerica
        }

        public int CountryId { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }

        public string Iso { get; set; }
    }
}
