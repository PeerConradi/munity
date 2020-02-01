using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.Util.Extenstions;

namespace MUNityAngular.Hubs.HubObjects
{
    public class HUBPreamble
    {
        public string ID { get; set; }

        public List<HUBPreambleParagraph> Paragraphs { get; set; }

        public HUBPreamble(Models.PreambleModel preamble)
        {
            this.ID = preamble.ID;
            this.Paragraphs = preamble.Paragraphs.ToHubParagraphs();
        }
    }
}
