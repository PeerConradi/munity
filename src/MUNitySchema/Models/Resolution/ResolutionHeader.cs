using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MUNity.Models.Resolution
{
    /// <summary>
    /// The header and information of a resoltion.
    /// </summary>
    public partial class ResolutionHeader
    {

        public string ResolutionHeaderId { get; set; }

        /// <summary>
        /// The name to find the document inside a register or list. This should not be used as the display name
        /// inside the resolution use the topic property for that case.
        /// The name could also be a number you give like: 1244
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A longer version of the name for example: United Nations Security Council Resolution 1244
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// The topic of the resolution. This should be displayed as the header of a resolution.
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// The agenda item of the resolution.
        /// </summary>
        public string AgendaItem { get; set; }

        /// <summary>
        /// The session for example: 4011th meeting
        /// or in case of your conference it could be: Day 2/Sess: 1
        /// </summary>
        public string Session { get; set; }


        /// <summary>
        /// The name of the Submitter of the resolution.
        /// </summary>
        public string SubmitterName { get; set; }

        /// <summary>
        /// The Name of the committee, for example: The Security Council
        /// </summary>
        public string CommitteeName { get; set; }

        /// <summary>
        /// List of the names of supporters for this document.
        /// </summary>
        public List<ResolutionSupporter> Supporters { get; set; }

        /// <summary>
        /// Creates a new resolution Header.
        /// </summary>
        public ResolutionHeader()
        {
            this.ResolutionHeaderId = Guid.NewGuid().ToString();
            Supporters = new List<ResolutionSupporter>();
        }

    }
}
