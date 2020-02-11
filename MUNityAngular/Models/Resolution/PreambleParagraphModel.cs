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
            
            ID = Guid.NewGuid().ToString();
            //At this moment the preamble could be null when deserialzed
            //Because the Paragraphs are stored inside an observable list they will
            //be givin their parent later...
            if (preamble != null)
            {
                Preamble = preamble;
                this.ResolutionID = preamble.ResolutionID;
            }
           
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
