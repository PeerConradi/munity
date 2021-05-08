using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace MUNityCore.Models.Conference.Roles
{
    /// <summary>
    /// If the conference contains the role of press you can define different types
    /// of press Roles.
    /// For example Print for things like: Newspapers and magazines
    /// TV for VLogs, Newsshows etc.
    /// Online if you want to seperate your newspaper into an paper and online version.
    /// </summary>
    public class ConferencePressRole : AbstractConferenceRole
    {
        public enum EPressCategories
        {
            /// <summary>
            /// Any type of printed newspaper, magazine etc.
            /// </summary>
            Print,
            /// <summary>
            /// Anything video news format.
            /// </summary>
            TV,
            /// <summary>
            /// For Word-press websites etc.
            /// </summary>
            Online
        }

        public EPressCategories PressCategory { get; set; }

    }
}
