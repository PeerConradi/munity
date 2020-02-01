using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Hubs.HubObjects
{
    public class HUBPreambleParagraph
    {
        public string ID { get; set; }

        public string Text { get; set; }

        public HUBPreambleParagraph(Models.PreambleParagraphModel p)
        {
            this.ID = p.ID;
            this.Text = p.Text;
        }
    }
}
