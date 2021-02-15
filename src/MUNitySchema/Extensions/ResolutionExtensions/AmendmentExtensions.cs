using MUNity.Models.Resolution;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using MUNity.Extensions.ObservableCollectionExtensions;

namespace MUNity.Extensions.ResolutionExtensions
{

    /// <summary>
    /// Get some more funcinality for working with amendments on the Resolution. documents
    /// </summary>
    public static class AmendmentExtensions
    {
        /// <summary>
        /// Returns all the Amendments for the operative paragraph with the given Id.
        /// </summary>
        /// <param name="section"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<AbstractAmendment> AmendmentsForOperativeParagraph(this OperativeSection section, string id)
        {
            var result = new List<AbstractAmendment>();

            result.AddRange(section.AddAmendments.Where(n => n.TargetSectionId == id));
            result.AddRange(section.ChangeAmendments.Where(n => n.TargetSectionId == id));
            result.AddRange(section.DeleteAmendments.Where(n => n.TargetSectionId == id));
            result.AddRange(section.MoveAmendments.Where(n => n.TargetSectionId == id));
            return result;
        }

        /// <summary>
        /// Adds a new Amendment into the Amendment list.
        /// </summary>
        /// <param name="section"></param>
        /// <param name="amendment"></param>
        private static void PushAmendment(this OperativeSection section, AbstractAmendment amendment)
        {
            // For now every Amendment has a TargetSectionId this could maybe be different one day
            // Remember to move this function if this day ever comes.
            if (section.FirstOrDefault(n => n.OperativeParagraphId == amendment.TargetSectionId) == null)
                throw new MUNity.Exceptions.Resolution.OperativeParagraphNotFoundException();

            if (amendment is AddAmendment addAmendment)
            {
                section.AddAmendments.Add(addAmendment);
            }
            else if (amendment is ChangeAmendment changeAmendment)
            {
                section.ChangeAmendments.Add(changeAmendment);
            }
            else if (amendment is DeleteAmendment deleteAmendment)
            {
                section.DeleteAmendments.Add(deleteAmendment);
            }
            else if (amendment is MoveAmendment moveAmendment)
            {
                section.MoveAmendments.Add(moveAmendment);
            }
            else
            {
                throw new MUNity.Exceptions.Resolution.UnsupportedAmendmentTypeException();
            }
        }

        /// <summary>
        /// Removes an amentment from its list. Note that this is not the same
        /// as Deny. This will just remove the given instance of the amendment.
        /// </summary>
        /// <param name="section"></param>
        /// <param name="amendment"></param>
        public static void RemoveAmendment(this OperativeSection section, AbstractAmendment amendment)
        {
            if (amendment is AddAmendment addAmendment)
            {
                section.AddAmendments.RemoveAll(n => n.TargetSectionId == amendment.TargetSectionId);
                section.Paragraphs.RemoveAll(n => n.OperativeParagraphId == amendment.TargetSectionId);
            }
            else if (amendment is ChangeAmendment changeAmendment)
            {
                section.ChangeAmendments.Remove(changeAmendment);
            }
            else if (amendment is DeleteAmendment deleteAmendment)
            {
                section.DeleteAmendments.Remove(deleteAmendment);
            }
            else if (amendment is MoveAmendment moveAmendment)
            {
                section.MoveAmendments.Remove(moveAmendment);
                section.Paragraphs.RemoveAll(n => moveAmendment.NewTargetSectionId == n.OperativeParagraphId);
            }
            else
            {
                throw new MUNity.Exceptions.Resolution.UnsupportedAmendmentTypeException();
            }
        }

        /// <summary>
        /// Creates a new Amendment to delete an operative paragraph.
        /// </summary>
        /// <param name="section"></param>
        /// <param name="paragraphId"></param>
        /// <returns></returns>
        public static DeleteAmendment CreateDeleteAmendment(this OperativeSection section, string paragraphId)
        {
            if (section.FindOperativeParagraph(paragraphId) == null)
                throw new MUNity.Exceptions.Resolution.OperativeParagraphNotFoundException();

            DeleteAmendment newAmendment = new DeleteAmendment
            {
                TargetSectionId = paragraphId
            };
            section.PushAmendment(newAmendment);
            return newAmendment;
        }

        /// <summary>
        /// Creates a new Amendment for a TextChange of a paragraph with a given Id.
        /// </summary>
        /// <param name="section">The operative Section of the resolution that this Admendment should be created in and also where the operative paragraph can be found.</param>
        /// <param name="paragraphId">The OperativeParagraphId of the target paragraph that the text should be changed in.</param>
        /// <param name="newText">The new Text that the paragraph should be set to if the created Amendment is Accepted/Applied.</param>
        /// <returns></returns>
        public static ChangeAmendment CreateChangeAmendment(this OperativeSection section, string paragraphId, string newText = "")
        {
            if (section.FindOperativeParagraph(paragraphId) == null)
                throw new MUNity.Exceptions.Resolution.OperativeParagraphNotFoundException();

            var newAmendment = new ChangeAmendment
            {
                TargetSectionId = paragraphId,
                NewText = newText
            };
            section.PushAmendment(newAmendment);
            return newAmendment;
        }


        /// <summary>
        /// Creates a Move Amendment. This will also create a new Operative paragraph at the palce where the amendment should be moved to. Not that this virtual Paragraph
        /// will copy the text once when the amendment is created and once when the amendment is accepted. If the text of the paragraph changed in the mean time the displayed
        /// text of the virtual operative paragraph may differ.
        /// </summary>
        /// <param name="section"></param>
        /// <param name="paragraphId"></param>
        /// <param name="targetIndex"></param>
        /// <param name="parentParagraph"></param>
        /// <returns></returns>
        public static MoveAmendment CreateMoveAmendment(this OperativeSection section, string paragraphId, int targetIndex, OperativeParagraph parentParagraph = null)
        {
            var sourceParagraph = section.FindOperativeParagraph(paragraphId);
            if (sourceParagraph == null)
                throw new MUNity.Exceptions.Resolution.OperativeParagraphNotFoundException();

            var newAmendment = new MoveAmendment
            {
                TargetSectionId = paragraphId
            };
            var virtualParagraph = new OperativeParagraph
            {
                IsLocked = true,
                IsVirtual = true,
                Text = sourceParagraph.Text
            };
            newAmendment.NewTargetSectionId = virtualParagraph.OperativeParagraphId;
            section.InsertIntoRealPosition(virtualParagraph, targetIndex, parentParagraph);
            section.PushAmendment(newAmendment);
            return newAmendment;
        }

        /// <summary>
        /// Creates a new Add Amendment to add a new operative paragraph at a certain position. This will also create a virtual Paragraph at the stop where
        /// the new paragraph may be when it is accepted.
        /// </summary>
        /// <param name="section">The operative Section of the Resolution where this paragraph should be added.</param>
        /// <param name="targetIndex">The index where the paragraph should be added.</param>
        /// <param name="text">The Text of the new paragraph</param>
        /// <param name="parentParagraph">Use this parent paragraph if the new paragraph should be a sub point</param>
        /// <returns></returns>
        public static AddAmendment CreateAddAmendment(this OperativeSection section, int targetIndex, string text = "", OperativeParagraph parentParagraph = null)
        {
            var virtualParagraph = new OperativeParagraph(text)
            {
                IsVirtual = true,
                Visible = false
            };
            section.InsertIntoRealPosition(virtualParagraph, targetIndex, parentParagraph);
            var amendment = new AddAmendment
            {
                TargetSectionId = virtualParagraph.OperativeParagraphId
            };
            section.PushAmendment(amendment);
            return amendment;
        }

        /// <summary>
        /// Creates a new amendment to delete a certain operative paragraph.
        /// </summary>
        /// <param name="section"></param>
        /// <param name="paragraph"></param>
        /// <returns></returns>
        public static DeleteAmendment CreateDeleteAmendment(this OperativeSection section, OperativeParagraph paragraph) => section.CreateDeleteAmendment(paragraph.OperativeParagraphId);

        /// <summary>
        /// Creates a new Move Amendment. The given targetIndex is the position the new Virtual Paragraph will be located.
        /// Type in 0 to move it to the beginning of the list. Note that 1 will also move it to the start!
        /// When you have two Paragraphs (A and B) and want A to move behind B (B, A) you need to set the targetIndex to 2!
        /// </summary>
        /// <param name="section">The Operative Section that you want to crate the Move Amendment at.</param>
        /// <param name="paragraph"></param>
        /// <param name="targetIndex"></param>
        /// <param name="parentParagraph"></param>
        /// <returns></returns>
        public static MoveAmendment CreateMoveAmendment(this OperativeSection section, OperativeParagraph paragraph, int targetIndex, OperativeParagraph parentParagraph = null) =>
            section.CreateMoveAmendment(paragraph.OperativeParagraphId, targetIndex, parentParagraph);

        /// <summary>
        /// Creates a new Text change amendment.
        /// </summary>
        /// <param name="section"></param>
        /// <param name="paragraph"></param>
        /// <param name="newText"></param>
        /// <returns></returns>
        public static ChangeAmendment CreateChangeAmendment(this OperativeSection section, OperativeParagraph paragraph, string newText = "") => section.CreateChangeAmendment(paragraph.OperativeParagraphId, newText);
    }
}
