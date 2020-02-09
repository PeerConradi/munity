using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUNityAngular.Util.Extenstions;

namespace MUNityAngular.Models.Resolution
{
    public class PreambleParagraphModel
    {
        public string ID { get; set; }

        public string Text { get; set; }

        public string ResolutionID { get; set; }

        [JsonIgnore]
        [BsonIgnore]
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
            ID = Guid.NewGuid().ToString();
            this.ResolutionID = preamble.ResolutionID;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
