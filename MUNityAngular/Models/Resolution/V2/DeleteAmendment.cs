using System;
using System.Linq;
using MongoDB.Bson.IO;
using MongoDB.Driver.Core.Operations;

namespace MUNityAngular.Models.Resolution.V2
{
    public class DeleteAmendment : IDeleteAmendment
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
            var paragraph = resolution.OperativeSection.Paragraphs.FirstOrDefault(n =>
                n.OperativeParagraphId == TargetSectionId);

            if (resolution?.OperativeSection == null)
                return false;

            if (!resolution.OperativeSection.Paragraphs.Contains(paragraph))
                return false;

            resolution.OperativeSection?.Paragraphs.Remove(paragraph);
            resolution.OperativeSection.DeleteAmendments.Remove(this);
            return true;
        }

        public bool Deny(ResolutionV2 resolution)
        {
            if (resolution.OperativeSection == null)
                return false;

            var count = resolution.OperativeSection.DeleteAmendments.RemoveAll(n =>
                n.TargetSectionId == this.TargetSectionId);

            if (count > 0)
                return true;

            return false;
        }

        public DeleteAmendment()
        {
            this.SubmitTime = DateTime.Now;
            this.Id = Util.Tools.IdGenerator.RandomString(25);
        }
    }
}