using System.Collections.Generic;

namespace MUNityAngular.Models.Resolution.V2
{
    public interface IPreamble
    {
        string PreambleId { get; set; }

        List<PreambleParagraph> Paragraphs { get; set; }
    }
}