namespace MUNity.Database.Models.Website
{
    public class WebPageTextElement : AbstractConferenceWebPageElement
    {
        public string Content { get; set; }

        public string TextRaw { get; set; }

        public string NormalizedTextRaw { get; set; }
    }
}
