using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace MUNityCore.Services
{
    public class SpeakerlistService
    {
        Random _rnd = new Random();
        private List<Models.SpeakerlistModel> Speakerlists;

        public Models.SpeakerlistModel CreateSpeakerlist()
        {
            int id = _rnd.Next(100000, 999999);
            while (Speakerlists.Any(n => n.PublicId == id))
            {
                id = _rnd.Next(100000, 999999);
            }
            var speakerList = new Models.SpeakerlistModel();
            speakerList.PublicId = id;
            Speakerlists.Add(speakerList);
            return speakerList;
        }

        public Models.SpeakerlistModel GetSpeakerlist(string id)
        {
            return Speakerlists.FirstOrDefault(n => n.Id == id);
        }

        public Models.SpeakerlistModel GetSpeakerlistByPublicId(int publicId)
        {
            return Speakerlists.FirstOrDefault(n => n.PublicId == publicId);
        }

        public void FlushSpeakerlist()
        {
            Speakerlists.Clear();
        }


        public SpeakerlistService()
        {
            Speakerlists = new List<Models.SpeakerlistModel>();
            Console.WriteLine("Speakerlist Service Started!");
        }
    }
}
