using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNityAngular.Models
{
    public class PreambleParagraphModel
    {
        public string ID { get; set; }

        private string _text;
        public string Text { get; set; }

        [JsonIgnore]
        public PreambleModel Preamble { get; set; }

        public void MoveUp()
        {
            int oldIndex = Preamble.Paragraphs.IndexOf(this);
            if (oldIndex > 0)
            {
                Preamble.Paragraphs.Move(oldIndex, oldIndex - 1);
            }
        }

        public void MoveDown()
        {
            int oldIndex = Preamble.Paragraphs.IndexOf(this);
            if (oldIndex + 1 < Preamble.Paragraphs.Count)
            {
                Preamble.Paragraphs.Move(oldIndex, oldIndex + 1);
            }
        }

        public void Remove()
        {
            Preamble.Paragraphs.Remove(this);
        }

        public PreambleParagraphModel(PreambleModel preamble)
        {
            Preamble = preamble;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
