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

        public string ResolutionID { get; set; }

        private ObservableCollection<PreambleParagraphModel> _paragraphs;
        public ObservableCollection<PreambleParagraphModel> Paragraphs { 
            get => _paragraphs;
            set 
            {
                this._paragraphs = value;
                foreach(var e in value)
                {
                    e.ResolutionID = this.ResolutionID;
                }
            } 
        }

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
            foreach(var p in Paragraphs)
            {
                p.ResolutionID = this.ResolutionID;
            }
            
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
            foreach (PreambleParagraphModel paragraph in Paragraphs)
            {
                text += paragraph.Text;
                text += "\n";
            }
            return text;
        }
    }
}
