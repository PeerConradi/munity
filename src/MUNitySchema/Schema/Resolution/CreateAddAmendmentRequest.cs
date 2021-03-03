using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Resolution
{
    public class CreateAddAmendmentRequest : ResolutionRequest
    {
        public string ParentParagraphId { get; set; }

        public string SubmitterName { get; set; }

        public int Index { get; set; }

        public string Text { get; set; }
    }

    public class CreateChangeAmendmentRequest : ResolutionRequest
    {
        public string ParagraphId { get; set; }

        public string SubmitterName { get; set; }

        public string NewText { get; set; }
    }

    public class CreateDeleteAmendmentRequest : ResolutionRequest
    {
        public string ParagraphId { get; set; }

        public string SubmitterName { get; set; }
    }

    public class CreateMoveAmendmentRequest : ResolutionRequest
    {
        public string ParagraphId { get; set; }

        public string SubmitterName { get; set; }

        public int NewIndex { get; set; }
    }
}
