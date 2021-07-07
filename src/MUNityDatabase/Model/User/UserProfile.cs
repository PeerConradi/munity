using MUNity.Database.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Models.User
{
    public class UserProfile
    {
        public MunityUser User { get; set; }

        public List<string> Interests { get; set; }

        /// <summary>
        /// Conferences this user participated in that are not Munity Conferences.
        /// </summary>
        public List<string> ConferenceHistoryManual { get; set; }
    }
}
