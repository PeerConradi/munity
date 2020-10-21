using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Schema.Request.Resolution
{
    public class CreateResolutionRequest
    {
        public enum CreationType
        {
            PublicResolution,
            PrivateResolution,
            ConferenceResolution,
            CommitteeResolution
        }
        
        public string Title { get; set; }

        public CreationType AccessType { get; set; }

        /// <summary>
        /// The Access Value is bound to the Access Type.
        /// For an AccessType like Conference the AccessValue needs to be the
        /// conferenceId. When using CommitteeResolution you need to specify
        /// the committee Id.
        /// </summary>
        public string AccessValue { get; set; }
    }
}
