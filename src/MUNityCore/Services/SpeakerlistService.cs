using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using MUNity.Models.ListOfSpeakers;
using Microsoft.EntityFrameworkCore;
using MUNity.Extensions.LoSExtensions;
using MUNity.Schema.ListOfSpeakers;

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

        public Task<bool> IsOnline(string id)
        {
            return this._context.ListOfSpeakers.AnyAsync(n => n.ListOfSpeakersId == id);
        }

        public async Task<Speaker> AddSpeaker(AddSpeakerBody body)
        {
            var list = await _context.ListOfSpeakers
                .Include(n => n.AllSpeakers)
                .FirstOrDefaultAsync(n => n.ListOfSpeakersId == body.ListOfSpeakersId);
            if (list == null) return null;

            var existing = list.AllSpeakers.FirstOrDefault(n => n.Iso == body.Iso &&
            n.Name == body.Name && n.Mode == Speaker.SpeakerModes.WaitToSpeak);

            if (existing != null) return existing;

            var mdl = list.AddSpeaker(body.Name, body.Iso);
            await _context.SaveChangesAsync();
            return mdl;
        }

        public async Task<Speaker> AddQuestion(AddSpeakerBody body)
        {
            var list = await _context.ListOfSpeakers
                .Include(n => n.AllSpeakers)
                .FirstOrDefaultAsync(n => n.ListOfSpeakersId == body.ListOfSpeakersId);
            if (list == null) return null;

            var existing = list.AllSpeakers.FirstOrDefault(n => n.Iso == body.Iso &&
            n.Name == body.Name && n.Mode == Speaker.SpeakerModes.WaitForQuesiton);

            if (existing != null) 
                return existing;

            var mdl = list.AddQuestion(body.Name, body.Iso);
            //list.AllSpeakers.Add(mdl);
            await _context.SaveChangesAsync();
            return mdl;
        }

        public bool IsListClosed(ListOfSpeakersRequest request)
        {
            return this._context.ListOfSpeakers.FirstOrDefault(n => n.ListOfSpeakersId == request.ListOfSpeakersId)?.ListClosed ?? true;
        }

        public bool IsQuestionsClosed(ListOfSpeakersRequest request)
        {
            return this._context.ListOfSpeakers
                .FirstOrDefault(n => n.ListOfSpeakersId == request.ListOfSpeakersId)?
                .QuestionsClosed ?? true;
        }

        internal bool AddQuestionSeconds(AddSpeakerSeconds body)
        {
            var list = _context.ListOfSpeakers
                .FirstOrDefault(n => n.ListOfSpeakersId == body.ListOfSpeakersId);
            if (list == null) return false;
            list.AddQuestionSeconds(body.Seconds);
            this._context.SaveChanges();
            return true;
        }

        internal bool AddSpeakerSeconds(AddSpeakerSeconds body)
        {
            var list = _context.ListOfSpeakers
                .FirstOrDefault(n => n.ListOfSpeakersId == body.ListOfSpeakersId);
            if (list == null) return false;
            list.AddSpeakerSeconds(body.Seconds);
            this._context.SaveChanges();
            return true;
        }

        [Obsolete("You should not call this from outside the service")]
        public void SaveChanges()
        {
            this._context.SaveChanges();
        }

        public ListOfSpeakers GetSpeakerlistByPublicId(string publicId)
        {
            return this._context.ListOfSpeakers.Include(n => n.AllSpeakers).FirstOrDefault(n => n.PublicId == publicId);
        }


        public SpeakerlistService(DataHandlers.EntityFramework.MunityContext context)
        {
            _context = context;
        }

        internal async Task<bool> RemoveSpeaker(RemoveSpeakerBody body)
        {
            var speakerToRemove = await _context.Speakers
                .FirstOrDefaultAsync(n => n.ListOfSpeakers.ListOfSpeakersId == body.ListOfSpeakersId &&
            n.Id == body.SpeakerId);
            if (speakerToRemove == null) return false;
            _context.Speakers.Remove(speakerToRemove);
            await _context.SaveChangesAsync();
            return true;
        }

        internal async Task<bool> NextSpeaker(string listid)
        {
            var list = await this._context.ListOfSpeakers.Include(n => n.AllSpeakers).FirstOrDefaultAsync(n => n.ListOfSpeakersId == listid);
            if (list == null) return false;
            list.NextSpeaker();
            await _context.SaveChangesAsync();
            return true;
        }

        internal async Task<bool> NextQuestion(string listid)
        {
            var list = await this._context.ListOfSpeakers
                .Include(n => n.AllSpeakers)
                .FirstOrDefaultAsync(n => n.ListOfSpeakersId == listid);
            if (list == null) return false;
            list.NextQuestion();
            await _context.SaveChangesAsync();
            return true;
        }

        internal DateTime? ResumeSpeaker(ListOfSpeakersRequest body)
        {
            var list = this._context.ListOfSpeakers
                .Include(n => n.AllSpeakers)
                .FirstOrDefault(n => n.ListOfSpeakersId == body.ListOfSpeakersId);
            if (list == null) return null;
            list.ResumeSpeaker();
            this._context.SaveChanges();
            return list.StartSpeakerTime.ToUniversalTime();
        }

        internal DateTime? ResumeAnswer(ListOfSpeakersRequest body)
        {
            var list = this._context.ListOfSpeakers
                .Include(n => n.AllSpeakers)
                .FirstOrDefault(n => n.ListOfSpeakersId == body.ListOfSpeakersId);
            if (list == null) return null;
            list.StartAnswer();
            this._context.SaveChanges();
            return list.StartSpeakerTime.ToUniversalTime();
        }

        internal DateTime? ResumeQuestion(ListOfSpeakersRequest body)
        {
            var list = this._context.ListOfSpeakers
                .Include(n => n.AllSpeakers)
                .FirstOrDefault(n => n.ListOfSpeakersId == body.ListOfSpeakersId);
            if (list == null) return null;
            list.ResumeQuestion();
            this._context.SaveChanges();
            return list.StartQuestionTime.ToUniversalTime();
        }

        internal bool SetSettings(SetListSettingsBody body)
        {
            var list = this._context.ListOfSpeakers
                .FirstOrDefault(n => n.ListOfSpeakersId == body.ListOfSpeakersId);
            if (list == null) return false;
            TimeSpan speakerTime;
            TimeSpan questionTime;
            if (!TimeSpan.TryParseExact(body.SpeakerTime, @"mm\:ss", null, out speakerTime))
                return false;

            if (!TimeSpan.TryParseExact(body.QuestionTime, @"mm\:ss", null, out questionTime))
                return false;

            list.SpeakerTime = speakerTime;
            list.QuestionTime = questionTime;
            _context.SaveChanges();
            return true;
        }

        internal bool ClearSpeaker(ListOfSpeakersRequest body)
        {
            var list = _context.ListOfSpeakers
                .FirstOrDefault(n => n.ListOfSpeakersId == body.ListOfSpeakersId);
            if (list == null) return false;
            list.ClearCurrentSpeaker();
            _context.SaveChanges();
            return true;
        }

        internal bool ClearQuestion(ListOfSpeakersRequest body)
        {
            var list = _context.ListOfSpeakers
                .FirstOrDefault(n => n.ListOfSpeakersId == body.ListOfSpeakersId);
            if (list == null) return false;
            list.ClearCurrentQuestion();
            _context.SaveChanges();
            return true;
        }

        internal bool Pause(ListOfSpeakersRequest body)
        {
            var list = _context.ListOfSpeakers
                .FirstOrDefault(n => n.ListOfSpeakersId == body.ListOfSpeakersId);
            if (list == null) return false;
            list.Pause();
            _context.SaveChanges();
            return true;
        }

        internal bool CloseList(ListOfSpeakersRequest body)
        {
            var list = _context.ListOfSpeakers.FirstOrDefault(n => n.ListOfSpeakersId == body.ListOfSpeakersId);
            if (list == null)
                return false;
            list.ListClosed = true;
            _context.SaveChanges();
            return true;
        }

        internal bool OpenList(ListOfSpeakersRequest body)
        {
            var list = _context.ListOfSpeakers.FirstOrDefault(n => n.ListOfSpeakersId == body.ListOfSpeakersId);
            if (list == null)
                return false;
            list.ListClosed = false;
            _context.SaveChanges();
            return true;
        }

        internal bool CloseQuestions(ListOfSpeakersRequest body)
        {
            var list = _context.ListOfSpeakers.FirstOrDefault(n => n.ListOfSpeakersId == body.ListOfSpeakersId);
            if (list == null)
                return false;
            list.QuestionsClosed = true;
            _context.SaveChanges();
            return true;
        }

        internal bool OpenQuestions(ListOfSpeakersRequest body)
        {
            var list = _context.ListOfSpeakers.FirstOrDefault(n => n.ListOfSpeakersId == body.ListOfSpeakersId);
            if (list == null)
                return false;
            list.QuestionsClosed = false;
            _context.SaveChanges();
            return true;
        }
    }
}
