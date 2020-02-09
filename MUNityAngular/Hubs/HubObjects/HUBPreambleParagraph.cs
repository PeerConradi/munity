﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.Models.Resolution;

namespace MUNityAngular.Hubs.HubObjects
{
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
