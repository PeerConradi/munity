using System.Collections.Generic;

namespace MUNityCore.Models.Resolution.V2
{
    public interface IPreamble
    {
        string PreambleId { get; set; }

        List<PreambleParagraph> Paragraphs { get; set; }
    }
}