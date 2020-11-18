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
    [DataContract]
    public class PressRole : AbstractRole
    {
        public enum EPressCategories
        {
            Print,
            TV,
            Online
        }

        [DataMember]
        public EPressCategories PressCategory { get; set; }

    }
}
