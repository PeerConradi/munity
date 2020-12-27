using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using MUNitySchema.Models.ListOfSpeakers;

namespace MUNityCore.Services
{
    public class SpeakerlistService
    {
        Random _rnd = new Random();
        private List<ListOfSpeakers> Speakerlists;

        public ListOfSpeakers CreateSpeakerlist()
        {
            var id = _rnd.Next(100000, 999999).ToString();
            while (Speakerlists.Any(n => n.PublicId == id))
            {
                id = _rnd.Next(100000, 999999).ToString();
            }
            var speakerList = new ListOfSpeakers();
            speakerList.PublicId = id;
            Speakerlists.Add(speakerList);
            return speakerList;
        }

        public ListOfSpeakers GetSpeakerlist(string id)
        {
            return Speakerlists.FirstOrDefault(n => n.ListOfSpeakersId == id);
        }

        public ListOfSpeakers GetSpeakerlistByPublicId(string publicId)
        {
            return Speakerlists.FirstOrDefault(n => n.PublicId == publicId);
        }

        public void FlushSpeakerlist()
        {
            Speakerlists.Clear();
        }


        public SpeakerlistService()
        {
            Speakerlists = new List<ListOfSpeakers>();
            Console.WriteLine("Speakerlist Service Started!");
        }
    }
}
