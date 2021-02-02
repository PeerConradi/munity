using System;
using System.Collections.Generic;
using System.Text;
using MUNity.Extensions.ResolutionExtensions;
using MUNity.Extensions.ObservableCollectionExtensions;

namespace MUNity.Models.Resolution
{

    /// <summary>
    /// The Delete Amendment is a type of amendment to remove an operative paragraph from the resolution.
    /// </summary>
    public class DeleteAmendment : AbstractAmendment
    {
        /// <summary>
        /// Removes the operative section with all children and amendments that are on it.
        /// </summary>
        /// <param name="parentSection"></param>
        /// <returns></returns>
        public override bool Apply(OperativeSection parentSection)
        {
            var paragraph = parentSection.FindOperativeParagraph(this.TargetSectionId);

            if (!parentSection.Paragraphs.Contains(paragraph))
                return false;

            parentSection.Paragraphs.Remove(paragraph);

            parentSection.AmendmentsForOperativeParagraph(this.TargetSectionId).ForEach(n => parentSection.RemoveAmendment(n));
            return true;
        }

        /// <summary>
        /// Removes the delete amendment and all other delete amendments that are targeting the same operative paragraph.
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public override bool Deny(OperativeSection section)
        {
            var count = section.DeleteAmendments.RemoveAll(n =>
                n.TargetSectionId == this.TargetSectionId);

            return count > 0;
        }
    }
}
