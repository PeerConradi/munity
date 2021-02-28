using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MUNity.Models.Resolution
{
    /// <summary>
    /// The Operative Section of a resolution containing the Paragraphs and the Amendments.
    /// </summary>
    public class OperativeSection
    {
        /// <summary>
        /// The Id of the operative Section
        /// </summary>
        public string OperativeSectionId { get; set; }

        /// <summary>
        /// Operative Paragraphs of this Section. These are only the top Level Paragrapghs, Paragraphs can have Children.
        /// </summary>
        public List<OperativeParagraph> Paragraphs { get; set; }

        /// <summary>
        /// All Amendments to change the text of an operative paragraph.
        /// </summary>
        public List<ChangeAmendment> ChangeAmendments { get; set; }

        /// <summary>
        /// All Alemdnemts to add a new operative paragraph
        /// </summary>
        public List<AddAmendment> AddAmendments { get; set; }

        /// <summary>
        /// All amendments to move an operative paragraph to a new position.
        /// </summary>
        public List<MoveAmendment> MoveAmendments { get; set; }

        /// <summary>
        /// All amendments to delete an operative paragraph.
        /// </summary>
        public List<DeleteAmendment> DeleteAmendments { get; set; }

        /// <summary>
        /// Creates a new Operative Section and generates a new Guid for it and will init all list of paragraphs
        /// as new empty lists.
        /// </summary>
        public OperativeSection()
        {
            OperativeSectionId = Guid.NewGuid().ToString();
            Paragraphs = new List<OperativeParagraph>();
            ChangeAmendments = new List<ChangeAmendment>();
            AddAmendments = new List<AddAmendment>();
            MoveAmendments = new List<MoveAmendment>();
            DeleteAmendments = new List<DeleteAmendment>();
        }
    }
}
