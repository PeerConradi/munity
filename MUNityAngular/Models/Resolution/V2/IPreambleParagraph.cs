using System.Collections.Generic;

namespace MUNityAngular.Models.Resolution.V2
{
    public interface IPreambleParagraph
    {
        string PreambleParagraphId { get; set; }

        string Text { get; set; }

        List<INoticeModel> Notices { get; set; }
    }
}