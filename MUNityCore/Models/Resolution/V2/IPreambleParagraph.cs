using System.Collections.Generic;

namespace MUNityCore.Models.Resolution.V2
{
    public interface IPreambleParagraph
    {
        string PreambleParagraphId { get; set; }

        string Text { get; set; }

        List<Notice> Notices { get; set; }
    }
}