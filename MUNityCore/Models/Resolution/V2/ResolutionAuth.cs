using System;
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
        public enum EAmendmentModes
        {
            /// <summary>
            /// Noone can Posts Amendments except the editors
            /// </summary>
            NotAllowed,
            /// <summary>
            /// Everyone can post amendments and they go directly into the list
            /// </summary>
            AllowPublicPost,
            /// <summary>
            /// Everyone can post amendments but they need to be accepted by an editor first.
            /// </summary>
            AllowPublicRequest,
            /// <summary>
            /// Everypne that is part of the conference that this resolution is linked (voer the committee) to is
            /// allowed to post an amendment directly into the document
            /// </summary>
            AllowConferencePost,
            /// <summary>
            /// Everyone that is part of the conference that this resolution is linked (over the committee) to is
            /// allowed to post an amendment that needs to be accepted by an editor before
            /// going into the document.
            /// </summary>
            AllowConferenceRequest,
            /// <summary>
            /// Everyone that is part of the committee that this resolution is linked to can post amendments directly into
            /// the document.
            /// </summary>
            AllowCommitteePost,
            /// <summary>
            /// Everyone that is part of the committee that this resolution is linked to can post amendments that need to
            /// be accepted by an editor before going into the document.
            /// </summary>
            AllowCommitteeRequest
        }

        public string ResolutionId { get; set; }

        public MUNityCore.Models.User.MunityUser CreationUser { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastChangeTime { get; set; }

        public List<ResolutionUser> Users { get; set; }

        public bool AllowPublicRead { get; set; }

        public bool AllowPublicEdit { get; set; }

        public bool AllowConferenceRead { get; set; }

        public bool AllowCommitteeRead { get; set; }

        public EAmendmentModes AmendmentMode { get; set; }

        public string PublicShortKey { get; set; }

        public string EditPassword { get; set; }

        public string ReadPassword { get; set; }

        /// <summary>
        /// Links to a MUN Core Committee
        /// </summary>
        public Conference.Committee Committee { get; set; }


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
