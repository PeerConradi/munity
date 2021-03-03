namespace MUNity.Schema.Resolution
{
    public class ChangeOperativeParagraphCommentRequest : ResolutionRequest
    {
        public ChangeOperativeParagraphCommentRequest()
        {
        }

        public string OperativeParagraphId { get; set; }

        public string NewText { get; set; }
    }
}
