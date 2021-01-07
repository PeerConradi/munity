using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using MUNity.Models.ListOfSpeakers;
using Microsoft.EntityFrameworkCore;

namespace MUNityCore.Services
{
    public class SpeakerlistService
    {
        Random _rnd = new Random();
        private readonly DataHandlers.EntityFramework.MunityContext _context;

        public ListOfSpeakers CreateSpeakerlist()
        {
            var id = _rnd.Next(100000, 999999).ToString();
            while (_context.ListOfSpeakers.Any(n => n.PublicId == id))
            {
                id = _rnd.Next(100000, 999999).ToString();
            }
            var speakerList = new ListOfSpeakers();
            speakerList.PublicId = id;
            _context.ListOfSpeakers.Add(speakerList);
            this._context.SaveChanges();
            return speakerList;
        }

        public ListOfSpeakers GetSpeakerlist(string id)
        {
            return this._context.ListOfSpeakers.Include(n => n.Speakers).Include(n => n.Questions).FirstOrDefault(n => n.ListOfSpeakersId == id);
        }

        public void OverwriteList(ListOfSpeakers list)
        {
            var original = this._context.ListOfSpeakers.Include(n => n.Speakers).Include(n => n.Questions).FirstOrDefault(n => n.ListOfSpeakersId == list.ListOfSpeakersId);
            this._context.Entry(original).CurrentValues.SetValues(list);
            this._context.SaveChanges();
        }

        public ListOfSpeakers GetSpeakerlistByPublicId(string publicId)
        {
            return this._context.ListOfSpeakers.Include(n => n.Speakers).Include(n => n.Questions).FirstOrDefault(n => n.PublicId == publicId);
        }


        public SpeakerlistService(DataHandlers.EntityFramework.MunityContext context)
        {
            _context = context;
        }
    }
}
