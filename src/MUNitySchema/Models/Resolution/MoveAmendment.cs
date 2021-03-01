using System;
using System.Collections.Generic;
using System.Text;
using MUNity.Extensions.ResolutionExtensions;

namespace MUNity.Models.Resolution
{
    /// <summary>
    /// An Amendment to move an operative paragraph to a new position.
    /// </summary>
    public class MoveAmendment : AbstractAmendment
    {
        /// <summary>
        /// The Id of the virtual paragraph that represents the new position of the operative paragraph.
        /// </summary>
        public string NewTargetSectionId { get; set; }

        /// <summary>
        /// Will delete the opld amendment and move all its settings to the currently virtual paragraph.
        /// </summary>
        /// <param name="parentSection"></param>
        /// <returns></returns>
        public override bool Apply(OperativeSection parentSection)
        {
            var placeholder = parentSection.FindOperativeParagraph(NewTargetSectionId);
            var target = parentSection.FindOperativeParagraph(TargetSectionId);

            if (target == null || placeholder == null)
                return false;

            placeholder.Children = target.Children;
            placeholder.Corrected = target.Corrected;
            placeholder.IsLocked = false;
            placeholder.IsVirtual = false;
            placeholder.Name = target.Name;
            placeholder.OperativeParagraphId = target.OperativeParagraphId;
            target.OperativeParagraphId = Guid.NewGuid().ToString();
            this.TargetSectionId = target.OperativeParagraphId;
            placeholder.Text = target.Text;
            placeholder.Visible = true;
            parentSection.RemoveOperativeParagraph(target);
            return true;
        }

        /// <summary>
        /// Will remove the amendment and the virtual paragraph.
        /// </summary>
        /// <param name="parentSection"></param>
        /// <returns></returns>
        public override bool Deny(OperativeSection parentSection)
        {
            parentSection.RemoveAmendment(this);
            return true;
        }
    }
}
