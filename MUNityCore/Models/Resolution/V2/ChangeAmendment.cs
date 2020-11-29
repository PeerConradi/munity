using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Models.Resolution.V2
{

    /// <summary>
    /// The Change amendment is for changing the text of an operative paragraph.
    /// The amendment contains a value of NewText that contains the whole new Text.
    /// </summary>
    public class ChangeAmendment : IChangeAmendment
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string TargetSectionId { get; set; }
        public bool Activated { get; set; }
        public string SubmitterName { get; set; }
        public DateTime SubmitTime { get; set; }
        public string Type { get; set; }
        public bool Apply(ResolutionV2 resolution)
        {
            throw new NotImplementedException();
        }

        public bool Deny(ResolutionV2 resolution)
        {
            throw new NotImplementedException();
        }

        public string NewText { get; set; }
    }
}
