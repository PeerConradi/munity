using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Resolution
{
    public class TagModel
    {

        /// <summary>
        /// A Tag has a type, for example: success, danger, error etc.
        /// The types are inside a string and not an enum because they are a string
        /// and not an enum.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The Text of the Tag, should be short but is not limited by default.
        /// </summary>
        public string Text { get; set; }
    }
}
