using System;
using System.Collections.Generic;
using System.Text;
using MUNity.Extensions.ResolutionExtensions;

namespace MUNity.Models.Resolution
{
    /// <summary>
    /// An Amendment to create the Text of an operative paragraph.
    /// </summary>
    public class ChangeAmendment : AbstractAmendment
    {
        /// <summary>
        /// The Text that the operative Paragraph should be changed to.
        /// </summary>
        public string NewText { get; set; }

        /// <summary>
        /// Changes the text and removes the amendment.
        /// </summary>
        /// <param name="parentSection"></param>
        /// <returns></returns>
        public override bool Apply(OperativeSection parentSection)
        {
            parentSection.ChangeAmendments.Remove(this);
            var target = parentSection.FindOperativeParagraph(this.TargetSectionId);
            if (target == null) return false;
            target.Text = this.NewText;
            return true;
        }

        /// <summary>
        /// Removes the amendment.
        /// </summary>
        /// <param name="parentSection"></param>
        /// <returns></returns>
        public override bool Deny(OperativeSection parentSection)
        {
            parentSection.ChangeAmendments.Remove(this);
            return true;
        }
    }
}
