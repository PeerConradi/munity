using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MUNity.Models.Resolution
{

    /// <summary>
    /// An operative Paragraph is a paragraph inside the operative section. You can create amendments for this type of
    /// paragraph and they can also have child paragraphs.
    /// </summary>
    public class OperativeParagraph
    {

        /// <summary>
        /// The Id of the operativee Paragraph.
        /// </summary>
        public string OperativeParagraphId { get; set; }

        /// <summary>
        /// The name of the paragraph if you want to identify it by a given name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Is the paragraph marked as locked. This will not effect the logic you can still submit amendments
        /// or apply amendments to it. This may change in future implementations!
        /// </summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// Virtual is true when the Operative Paragraph comes from an AddAmendment and doesn't really count as an
        /// paragraph or if it is from a move amendment and is the paragraph where the orignal should be moved to.
        /// </summary>
        public bool IsVirtual { get; set; }

        
        /// <summary>
        /// The text of the operative Paragraph.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Is the operative Paragraph visible inside the views.
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// Is the operative paragraph marked as corrected. Note that this
        /// does not interact with any form of logic, if the Text is changed it will
        /// still be marked as corrected.
        /// </summary>
        public bool Corrected { get; set; }

        /// <summary>
        /// Child paragraphs of this operative paragraph.
        /// </summary>
        public List<OperativeParagraph> Children { get; set; }

        public string Comment { get; set; }

        /// <summary>
        /// Creates a new Operative Paragraph and will give it an id.
        /// </summary>
        /// <param name="text"></param>
        public OperativeParagraph(string text = "")
        {
            this.Text = text;
            this.OperativeParagraphId = Guid.NewGuid().ToString();
            this.Children = new List<OperativeParagraph>();
        }

        public OperativeParagraph()
        {
            this.OperativeParagraphId = Guid.NewGuid().ToString();
            this.Children = new List<OperativeParagraph>();
        }
    }
}
