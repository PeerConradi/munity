using MUNityAngular.DataHandlers.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Conference
{

    [Obsolete("Use DataHandlers.EntityFramework.Models.ConferenceUserAuth")]
    public class ConferenceUserAuthModel
    {

        [DatabaseSave("conferenceid")]
        [PrimaryKey]
        public string ConferenceId { get; set; }

        [DatabaseSave("userid")]
        public string UserId { get; set; }

        [DatabaseSave("CanOpen")]
        public bool CanOpen { get; set; }

        [DatabaseSave("CanEdit")]
        public bool CanEdit { get; set; }

        [DatabaseSave("CanRemove")]
        public bool CanRemove { get; set; }
    }
}
