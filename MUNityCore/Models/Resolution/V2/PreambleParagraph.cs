using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Models.Resolution.V2
{
    public class PreambleParagraph : IPreambleParagraph
    {
        public string PreambleParagraphId { get; set; }
        public string Text { get; set; }

        public bool IsLocked { get; set; }

        public bool Corrected { get; set; }
        public List<Notice> Notices { get; set; }

        public PreambleParagraph()
        {
            PreambleParagraphId = Util.Tools.IdGenerator.RandomString(36);
            Notices = new List<Notice>();
        }
    }
}
