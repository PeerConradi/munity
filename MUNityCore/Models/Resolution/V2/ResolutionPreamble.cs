using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Models.Resolution.V2
{
    public class ResolutionPreamble : IPreamble
    {
        public string PreambleId { get; set; }
        public List<PreambleParagraph> Paragraphs { get; set; }

        public ResolutionPreamble()
        {
            PreambleId = Util.Tools.IdGenerator.RandomString(36);
            Paragraphs = new List<PreambleParagraph>();
        }
    }
}
