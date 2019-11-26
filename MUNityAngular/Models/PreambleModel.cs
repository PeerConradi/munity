using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNityAngular.Models
{
    public class PreambleModel
    {
        public string ID { get; set; }

        public ObservableCollection<PreambleParagraphModel> Paragraphs { get; set; }

        public PreambleModel()
        {
            Paragraphs = new ObservableCollection<PreambleParagraphModel>();
            Paragraphs.CollectionChanged += Paragraphs_CollectionChanged;
        }

        private void Paragraphs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var l in e.NewItems.OfType<PreambleParagraphModel>())
                {
                    l.Preamble = this;
                }
            }
            
        }

        public PreambleParagraphModel AddParagraph(string text = "")
        {
            PreambleParagraphModel paragraph = new PreambleParagraphModel(this);
            paragraph.Text = text;
            Paragraphs.Add(paragraph);
            return paragraph;
        }

        public override string ToString()
        {
            string text = "";
            foreach (PreambleParagraphModel paragraph in Paragraphs)
            {
                text += paragraph.Text;
                text += "\n";
            }
            return text;
        }
    }
}
