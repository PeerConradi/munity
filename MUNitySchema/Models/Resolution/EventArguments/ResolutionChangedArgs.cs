using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Models.Resolution.EventArguments
{
    /// <summary>
    /// Event arguments that are called, when the resolution itself has changed in
    /// extreme. Or without a possible track.
    /// </summary>
    public class ResolutionChangedArgs : EventArgs
    {
        /// <summary>
        /// The transaction key that is given to this change
        /// </summary>
        public string Tan { get; set; }

        /// <summary>
        /// The new Resolution object with all the changes.
        /// </summary>
        public Resolution Resolution { get; set; }

        public ResolutionChangedArgs(string tan, Resolution resolution)
        {
            this.Tan = tan;
            this.Resolution = resolution;
        }
    }

    public abstract class ResolutionEventArgs : EventArgs
    {
        public string Tan { get; set; }

        public string ResolutionId { get; set; }
    }

    public class PreambleParagraphChangedArgs : ResolutionEventArgs
    {

        public PreambleParagraph Paragraph { get; set; }

        public PreambleParagraphChangedArgs(string tan, string resolutionId, PreambleParagraph paragraph)
        {
            Tan = tan;
            ResolutionId = resolutionId;
            Paragraph = paragraph;
        }
    }

    public class OperativeParagraphChangedEventArgs : ResolutionEventArgs
    {

        public OperativeParagraph Paragraph { get; set; }

        public OperativeParagraphChangedEventArgs (string tan, string resolutionId, OperativeParagraph paragraph)
        {
            Tan = tan;
            ResolutionId = resolutionId;
            Paragraph = paragraph;
        }
    }

    public class AmendmentActivatedChangedEventArgs : ResolutionEventArgs
    {

        public string AmendmentId { get; set; }

        public bool Activated { get; set; }
    }

    public class PreambleParagraphTextChangedEventArgs : ResolutionEventArgs
    { 
        public string ParagraphId { get; set; }

        public string Text { get; set; }

        public PreambleParagraphTextChangedEventArgs(string resolutionId, string paragraphId, string text)
        {
            this.ResolutionId = resolutionId;
            this.ParagraphId = paragraphId;
            this.Text = text;
        }
    }

    public class OperativeParagraphTextChangedEventArgs : ResolutionEventArgs
    {

        public string ParagraphId { get; set; }

        public string Text { get; set; }
    }

    public class PreambleParagraphAddedEventArgs : ResolutionEventArgs
    {

        PreambleParagraph Paragraph { get; set; }

        public PreambleParagraphAddedEventArgs(string resolutionId, PreambleParagraph paragraph)
        {
            this.ResolutionId = resolutionId;
            this.Paragraph = paragraph;
        }
    }

    public class HeaderStringPropChangedEventArgs : ResolutionEventArgs
    {
        public string Text { get; set; }

        public HeaderStringPropChangedEventArgs(string resolutionId, string text)
        {
            this.ResolutionId = resolutionId;
            this.Text = text;
        }
    }
}
