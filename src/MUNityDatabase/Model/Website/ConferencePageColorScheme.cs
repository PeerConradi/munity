using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Database.Model.Website
{
    public class ConferencePageColorScheme
    {
        public int ConferencePageColorSchemeId { get; set; }

        public Models.Conference.Conference Conference { get; set; }

        /// <summary>
        /// Hex Color Code of the primary Color. #FF00FF
        /// </summary>
        [MaxLength(7)]
        public string PrimaryColor { get; set; }

        [MaxLength(7)]
        public string SecondaryColor { get; set; }

        [MaxLength(7)]
        public string MenuBackColor { get; set; }

        [MaxLength(7)]
        public string MenuForeColor { get; set; }
    }
}
