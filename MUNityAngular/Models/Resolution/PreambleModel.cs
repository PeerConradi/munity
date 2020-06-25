using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Resolution
{
    public class PreambleModel
    {
        public string ID { get; set; }

        public string ResolutionID { get; set; }

        public List<PreambleParagraphModel> Paragraphs { get; set; }

        public PreambleModel()
        {
            Paragraphs = new List<PreambleParagraphModel>();
        }

        public PreambleParagraphModel AddParagraph(string text = "")
        {
            PreambleParagraphModel paragraph = new PreambleParagraphModel(this);
            paragraph.Text = text;
            paragraph.ResolutionID = this.ResolutionID;
            Paragraphs.Add(paragraph);
            return paragraph;
        }

        public override string ToString()
        {
            string text = "";
            Paragraphs.ForEach(n => text += n.Text + "\n");
            return text;
        }
    }
}
