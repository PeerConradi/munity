using System;
using System.Collections.Generic;
using System.Text;
using MUNity.Extensions.ResolutionExtensions;

namespace MUNity.Models.Resolution
{
    /// <summary>
    /// An Add Amendment is a type of amendment that is creating a new Operative Paragraph on a given position when accpeted.
    /// This logic will create a virtual operative paragraph at the point where the new Amendment will be added when it is accepted.
    /// </summary>
    public class AddAmendment : AbstractAmendment
    {
        /// <summary>
        /// Sets the Virtual Paragraph to a real paragraph and remove the amendment.
        /// </summary>
        /// <param name="parentSection"></param>
        /// <returns></returns>
        public override bool Apply(OperativeSection parentSection)
        {
            var targetParagraph = parentSection.FindOperativeParagraph(this.TargetSectionId);
            if (targetParagraph == null)
                return false;

            targetParagraph.IsVirtual = false;
            targetParagraph.Visible = true;
            parentSection.AddAmendments.Remove(this);
            return true;
        }

        /// <summary>
        /// will delete the amendment and the virtual paragraph.
        /// </summary>
        /// <param name="parentResolution"></param>
        /// <returns></returns>
        public override bool Deny(OperativeSection parentResolution)
        {
            parentResolution.RemoveAmendment(this);
            return true;
        }
    }
}
