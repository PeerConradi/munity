﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Resolution.V2
{
    public class PreambleParagraph : IPreambleParagraph
    {
        public string PreambleParagraphId { get; set; }
        public string Text { get; set; }
        public List<INoticeModel> Notices { get; set; }

        public PreambleParagraph()
        {
            PreambleParagraphId = Util.Tools.IdGenerator.RandomString(36);
            Notices = new List<INoticeModel>();
        }
    }
}