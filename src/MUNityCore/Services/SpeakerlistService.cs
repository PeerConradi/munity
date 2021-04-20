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

        public async Task<ListOfSpeakers> GetSpeakerlistAsync(string id)
        {
            return await this._context.ListOfSpeakers.Include(n => n.AllSpeakers).FirstOrDefaultAsync(n => n.ListOfSpeakersId == id);
        }

        public ListOfSpeakers GetSpeakerlistUntracked(string id)
        {
            return this._context.ListOfSpeakers.AsNoTracking().Include(n => n.AllSpeakers).FirstOrDefault(n => n.ListOfSpeakersId == id);
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

        public async Task<Speaker> AddSpeaker(string listId, string iso, string name)
        {
            var list = await _context.ListOfSpeakers
                .Include(n => n.AllSpeakers)
                .FirstOrDefaultAsync(n => n.ListOfSpeakersId == listId);
            if (list == null) return null;

            var existing = list.AllSpeakers.FirstOrDefault(n => n.Iso == iso &&
            n.Name == name && n.Mode == Speaker.SpeakerModes.WaitToSpeak);

            if (existing != null)
                return existing;

            var mdl = list.AddSpeaker(name, iso);
            //list.AllSpeakers.Add(mdl);
            await _context.SaveChangesAsync();
            return mdl;
        }

        public Speaker AddSpeaker(ListOfSpeakers list, string iso, string name)
        {
            var existing = list.AllSpeakers.FirstOrDefault(n => n.Iso == iso &&
            n.Name == name && n.Mode == Speaker.SpeakerModes.WaitToSpeak);

            if (existing != null)
                return existing;

            var mdl = list.AddSpeaker(name, iso);
            //list.AllSpeakers.Add(mdl);
            _context.SaveChanges();
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

        public async Task<Speaker> AddQuestion(string listId, string iso, string name)
        {
            var list = await _context.ListOfSpeakers
                .Include(n => n.AllSpeakers)
                .FirstOrDefaultAsync(n => n.ListOfSpeakersId == listId);
            if (list == null) return null;

            var existing = list.AllSpeakers.FirstOrDefault(n => n.Iso == iso &&
            n.Name == name && n.Mode == Speaker.SpeakerModes.WaitForQuesiton);

            if (existing != null)
                return existing;

            var mdl = list.AddQuestion(name, iso);
            //list.AllSpeakers.Add(mdl);
            await _context.SaveChangesAsync();
            return mdl;
        }

        public Speaker AddQuestion(ListOfSpeakers list, string iso, string name)
        {
            var existing = list.AllSpeakers.FirstOrDefault(n => n.Iso == iso &&
            n.Name == name && n.Mode == Speaker.SpeakerModes.WaitForQuesiton);

            if (existing != null)
                return existing;

            var mdl = list.AddQuestion(name, iso);
            //list.AllSpeakers.Add(mdl);
            _context.SaveChanges();
            return mdl;
        }

        public bool IsListClosed(ListOfSpeakersRequest request)
        {
            return IsListClosed(request.ListOfSpeakersId);
        }

        public bool IsListClosed(string listId)
        {
            return this._context.ListOfSpeakers.FirstOrDefault(n => n.ListOfSpeakersId == listId)?.ListClosed ?? true;
        }

        public bool IsQuestionsClosed(ListOfSpeakersRequest request)
        {
            return this._context.ListOfSpeakers
                .FirstOrDefault(n => n.ListOfSpeakersId == request.ListOfSpeakersId)?
                .QuestionsClosed ?? true;
        }

        public bool IsQuestionsClosed(string listId)
        {
            return this._context.ListOfSpeakers
                .FirstOrDefault(n => n.ListOfSpeakersId == listId)?
                .QuestionsClosed ?? true;
        }


        internal bool AddQuestionSeconds(AddSpeakerSeconds body)
        {
            return AddQuestionSeconds(body.ListOfSpeakersId, body.Seconds);
        }

        internal bool AddQuestionSeconds(string listId, int seconds)
        {
            var list = _context.ListOfSpeakers
                .FirstOrDefault(n => n.ListOfSpeakersId == listId);
            if (list == null) return false;
            list.AddQuestionSeconds(seconds);
            this._context.SaveChanges();
            return true;
        }

        internal bool AddSpeakerSeconds(AddSpeakerSeconds body)
        {
            return AddSpeakerSeconds(body.ListOfSpeakersId, body.Seconds);
        }

        internal bool AddSpeakerSeconds(string listId, int seconds)
        {
            var list = _context.ListOfSpeakers
                .FirstOrDefault(n => n.ListOfSpeakersId == listId);
            if (list == null) return false;
            list.AddSpeakerSeconds(seconds);
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

        internal bool RemoveSpeaker(string listId, string speakerId)
        {
            var speakerToRemove = _context.Speakers.FirstOrDefault(n => n.ListOfSpeakers.ListOfSpeakersId == listId &&
            n.Id == speakerId);
            if (speakerToRemove == null)
                return false;
            _context.Speakers.Remove(speakerToRemove);
            _context.SaveChanges();
            return true;
        }

        internal async Task<bool> RemoveSpeaker(Speaker speaker)
        {
            var speakerToRemove = await _context.Speakers
                .FirstOrDefaultAsync(n => n.Id == speaker.Id);
            if (speakerToRemove == null) return false;
            _context.Speakers.Remove(speakerToRemove);
            await _context.SaveChangesAsync();
            return true;
        }

        internal async Task<bool> NextSpeakerAsync(string listid)
        {
            var list = await this._context.ListOfSpeakers.Include(n => n.AllSpeakers).FirstOrDefaultAsync(n => n.ListOfSpeakersId == listid);
            if (list == null) return false;
            list.NextSpeaker();
            await _context.SaveChangesAsync();
            return true;
        }

        internal bool NextSpeaker(string listId)
        {
            var list = this._context.ListOfSpeakers.Include(n => n.AllSpeakers).FirstOrDefault(n => n.ListOfSpeakersId == listId);
            if (list == null) return false;
            list.NextSpeaker();
            _context.SaveChanges();
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

        internal DateTime? ResumeSpeaker(string listId)
        {
            var list = this._context.ListOfSpeakers
                .Include(n => n.AllSpeakers)
                .FirstOrDefault(n => n.ListOfSpeakersId == listId);
            if (list == null) return null;
            list.ResumeSpeaker();
            this._context.SaveChanges();
            return list.StartSpeakerTime.ToUniversalTime();
        }

        internal DateTime? ResumeAnswer(ListOfSpeakersRequest body)
        {
            return ResumeAnswer(body.ListOfSpeakersId);
        }

        internal DateTime? ResumeAnswer(string listId)
        {
            var list = this._context.ListOfSpeakers
                .Include(n => n.AllSpeakers)
                .FirstOrDefault(n => n.ListOfSpeakersId == listId);
            if (list == null) return null;
            list.StartAnswer();
            this._context.SaveChanges();
            return list.StartSpeakerTime.ToUniversalTime();
        }

        internal DateTime? ResumeQuestion(ListOfSpeakersRequest body)
        {
            return ResumeQuestion(body.ListOfSpeakersId);
        }

        internal DateTime? ResumeQuestion(string listId)
        {
            var list = this._context.ListOfSpeakers
                .Include(n => n.AllSpeakers)
                .FirstOrDefault(n => n.ListOfSpeakersId == listId);
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
            return ClearSpeaker(body.ListOfSpeakersId);
        }

        internal bool ClearSpeaker(string listId)
        {
            var list = _context.ListOfSpeakers
                .FirstOrDefault(n => n.ListOfSpeakersId == listId);
            if (list == null) return false;
            list.ClearCurrentSpeaker();
            _context.SaveChanges();
            return true;
        }

        internal bool ClearQuestion(ListOfSpeakersRequest body)
        {
            return ClearQuestion(body.ListOfSpeakersId);
        }

        internal bool ClearQuestion(string listId)
        {
            var list = _context.ListOfSpeakers
                .FirstOrDefault(n => n.ListOfSpeakersId == listId);
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

        internal bool Pause(string listId)
        {
            var list = _context.ListOfSpeakers
                .FirstOrDefault(n => n.ListOfSpeakersId == listId);
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

        internal List<Speaker> GetQuestions(string listId)
        {
            return _context.Speakers
                .Where(n => n.Mode == Speaker.SpeakerModes.WaitForQuesiton && n.ListOfSpeakers.ListOfSpeakersId == listId)
                .AsNoTracking()
                .ToList();
        }

        internal List<Speaker> GetSpeakers(string listId)
        {
            return _context.Speakers
                .Where(n => n.Mode == Speaker.SpeakerModes.WaitToSpeak && n.ListOfSpeakers.ListOfSpeakersId == listId)
                .AsNoTracking()
                .ToList();
        }
    }
}
