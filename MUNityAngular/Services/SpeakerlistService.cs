using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Services
{
    public class SpeakerlistService
    {
        private List<Models.SpeakerlistModel> Speakerlists;

       

        public void AddSpeakerlist(Models.SpeakerlistModel list)
        {
            Speakerlists.Add(list);
        }

        public Models.SpeakerlistModel GetSpeakerlist(string id)
        {
            return Speakerlists.FirstOrDefault(n => n.ID == id);
        }

        public SpeakerlistService()
        {
            Speakerlists = new List<Models.SpeakerlistModel>();
            Console.WriteLine("Speakerlist Service Started!");
        }
    }
}
