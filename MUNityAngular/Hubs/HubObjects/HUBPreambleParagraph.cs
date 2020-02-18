using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.Models.Resolution;

namespace MUNityAngular.Hubs.HubObjects
{
    /// <summary>
    /// The HUBPreambleParagraph is a one on one copy of the PreambleParagraphModel.
    /// The only difference is:
    /// It only stores Data and has absolutly no logic. We need to create this class
    /// because SignalR will otherwise send Data that is tagged with [JsonIgnore].
    /// </summary>
    public class HUBPreambleParagraph
    {
        public string ID { get; set; }

        public string Text { get; set; }

        public string ResolutionID { get; set; }

        public HUBPreambleParagraph(PreambleParagraphModel p)
        {
            this.ID = p.ID;
            this.Text = p.Text;
            this.ResolutionID = p.ResolutionID;
        }

        public HUBPreambleParagraph()
        {

        }
    }
}
