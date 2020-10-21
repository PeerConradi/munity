using System;

namespace MUNityCore.Models.Resolution.V2
{
    public interface IAmendment
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string TargetSectionId { get; set; }

        public bool Activated { get; set; }

        public string SubmitterName { get; set; }

        public DateTime SubmitTime { get; set; }

        public string Type { get; set; }

        public bool Apply(ResolutionV2 resolution);

        public bool Deny(ResolutionV2 resolution);
    }
}