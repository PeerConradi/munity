﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.IO;
using MongoDB.Driver.Core.Operations;

namespace MUNityCore.Models.Resolution.V2
{

    /// <summary>
    /// A ResolutionAuth saves a resolution inside the Database and manages
    /// the access to this element. Note that this Logic is seperated from the
    /// MUN-Core this is due to a discussion that resulted in a clear split
    /// between basic MUN functions and advanced editors.
    /// </summary>
    public class ResolutionAuth
    {

        public string ResolutionId { get; set; }

        public string Name { get; set; }

        public MUNityCore.Models.User.MunityUser CreationUser { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastChangeTime { get; set; }

        public List<ResolutionUser> Users { get; set; }

        public bool AllowPublicRead { get; set; }

        public bool AllowPublicEdit { get; set; }

        public bool AllowConferenceRead { get; set; }

        public bool AllowCommitteeRead { get; set; }

        public bool AllowOnlineAmendments { get; set; }

        public string PublicShortKey { get; set; }

        public string EditPassword { get; set; }

        public string ReadPassword { get; set; }

        /// <summary>
        /// Links to a MUN Core Committee
        /// </summary>
        public Conference.Committee Committee { get; set; }

        public Simulation.Simulation Simulation { get; set; }


        public ResolutionAuth(MUNity.Models.Resolution.Resolution resolution)
        {
            ResolutionId = resolution.ResolutionId;
            CreationDate = DateTime.Now;
            LastChangeTime = DateTime.Now;
        }

        public ResolutionAuth()
        {
            
        }
    }
}
