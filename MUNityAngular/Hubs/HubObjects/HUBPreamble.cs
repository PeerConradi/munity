using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.Util.Extenstions;
using MUNityAngular.Models.Resolution;

namespace MUNityAngular.Hubs.HubObjects
{
    public class HUBPreamble
    {
        public string ID { get; set; }

        public List<HUBPreambleParagraph> Paragraphs { get; set; }

        public HUBPreamble(PreambleModel preamble)
        {
            this.ID = preamble.ID;
            this.Paragraphs = preamble.Paragraphs.ToHubParagraphs();
        }

        public HUBPreamble()
        {
            Paragraphs = new List<HUBPreambleParagraph>();
        }
    }
}
