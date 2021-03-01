using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MUNity.Models.Resolution
{

    /// <summary>
    /// A Preamble Paragraphs. This type of paragraph cannot have amendments or child paragraphs.
    /// </summary>
    public class PreambleParagraph
    {

        /// <summary>
        /// The Id of the Preamble Paragraph.
        /// </summary>
        public string PreambleParagraphId { get; set; }

        /// <summary>
        /// The Text (content) of the paragraph.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// is the paragraph marked as locked. This will not effect the Text Property you can still change the Text or comments
        /// event if this property is set to true (locked).
        /// </summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// Marks the paragraph as corrected. Note that this property will still keep its value even if the text is changed.
        /// </summary>
        public bool Corrected { get; set; }

        public string Comment { get; set; }

        /// <summary>
        /// Creates a new Preamble paragraph.
        /// </summary>
        public PreambleParagraph()
        {
            PreambleParagraphId = Guid.NewGuid().ToString();
        }
    }
}
