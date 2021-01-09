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
            return this._context.ListOfSpeakers.Include(n => n.AllSpeakers).FirstOrDefault(n => n.ListOfSpeakersId == id);
        }

        public void SaveChanges()
        {
            this._context.SaveChanges();
        }

        public void OverwriteList(ListOfSpeakers original, ListOfSpeakers newList)
        {
            try
            {
                foreach (var speaker in newList.AllSpeakers)
                {
                    var inList = original.AllSpeakers.FirstOrDefault(n => n.Id == speaker.Id);
                    if (inList != null)
                    {
                        if (inList.Name != speaker.Name) inList.Name = speaker.Name;
                        if (inList.Mode != speaker.Mode) inList.Mode = speaker.Mode;
                        if (inList.OrdnerIndex != speaker.OrdnerIndex) inList.OrdnerIndex = speaker.OrdnerIndex;
                        if (inList.Iso != speaker.Iso) inList.Iso = speaker.Iso;
                    }
                    else
                    {
                        speaker.ListOfSpeakers = original;
                        original.AllSpeakers.Add(speaker);
                    }
                }
                var removedSpeakers = original.AllSpeakers.Where(n => newList.AllSpeakers.All(a => a.Id != n.Id)).ToList();
                removedSpeakers.ForEach(n => original.AllSpeakers.Remove(n));
                if (original.ListClosed != newList.ListClosed) original.ListClosed = newList.ListClosed;
                if (original.Name != newList.Name) original.Name = newList.Name;
                if (original.PausedQuestionTime != newList.PausedQuestionTime) original.PausedQuestionTime = newList.PausedQuestionTime;
                if (original.PausedSpeakerTime != newList.PausedSpeakerTime) original.PausedSpeakerTime = newList.PausedSpeakerTime;
                if (original.QuestionsClosed != newList.QuestionsClosed) original.QuestionsClosed = newList.QuestionsClosed;
                if (original.QuestionTime != newList.QuestionTime) original.QuestionTime = newList.QuestionTime;
                if (original.SpeakerTime != newList.SpeakerTime) original.SpeakerTime = newList.SpeakerTime;
                if (original.StartQuestionTime != newList.StartQuestionTime) original.StartQuestionTime = newList.StartQuestionTime;
                if (original.StartSpeakerTime != newList.StartSpeakerTime) original.StartSpeakerTime = newList.StartSpeakerTime;
                if (original.Status != newList.Status) original.Status = newList.Status;
                this._context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Tracker error: " + ex.Message + ex.StackTrace);
            }
            
        }

        public ListOfSpeakers GetSpeakerlistByPublicId(string publicId)
        {
            return this._context.ListOfSpeakers.Include(n => n.AllSpeakers).FirstOrDefault(n => n.PublicId == publicId);
        }


        public SpeakerlistService(DataHandlers.EntityFramework.MunityContext context)
        {
            _context = context;
        }
    }
}
