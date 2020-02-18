using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.Util.Extenstions;
using MUNityAngular.Models.Resolution;

namespace MUNityAngular.Hubs.HubObjects
{

    /// <summary>
    /// The HUBPreambleis a one on one copy of the PreambleModel.
    /// The only difference is:
    /// It only stores Data and has absolutly no logic. We need to create this class
    /// because SignalR will otherwise send Data that is tagged with [JsonIgnore].
    /// </summary>
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
