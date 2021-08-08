using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using MUNity.Extensions.LoSExtensions;
using MUNity.Schema.ListOfSpeakers;
using MUNity.Database.Context;
using MUNity.Database.Models.LoS;
using MUNityBase;
using MUNityBase.Interfances;

namespace MUNity.Services
{
    public class SpeakerlistService
    {
        Random _rnd = new Random();
        private readonly MunityContext _context;

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

        public async Task<ISpeaker> AddSpeaker(AddSpeakerBody body)
        {
            var list = await _context.ListOfSpeakers
                .Include(n => n.AllSpeakers)
                .FirstOrDefaultAsync(n => n.ListOfSpeakersId == body.ListOfSpeakersId);
            if (list == null) return null;

            var existing = list.AllSpeakers.FirstOrDefault(n => n.Iso == body.Iso &&
            n.Name == body.Name && n.Mode == SpeakerModes.WaitToSpeak);

            if (existing != null) return existing;

            var mdl = list.AddSpeaker(body.Name, body.Iso);

            await _context.SaveChangesAsync();
            return mdl;
        }

        public ISpeaker AddSpeaker(string listId, string iso, string name)
        {
            var list = _context.ListOfSpeakers
                .Include(n => n.AllSpeakers)
                .FirstOrDefault(n => n.ListOfSpeakersId == listId);
            if (list == null) return null;

            return AddSpeaker(list, iso, name);
        }

        public ISpeaker AddSpeaker(ListOfSpeakers list, string iso, string name)
        {
            var existing = list.AllSpeakers.FirstOrDefault(n => n.Iso == iso &&
            n.Name == name && n.Mode == SpeakerModes.WaitToSpeak);

            if (existing != null)
                return existing;

            var mdl = list.AddSpeaker(name, iso);
            //list.AllSpeakers.Add(mdl);
            var logEntry = EnsureLogEntry(list, iso, name);
            logEntry.TimesOnSpeakerlist += 1;
            _context.SaveChanges();
            return mdl;
        }

        private ListOfSpeakersLog EnsureLogEntry(ListOfSpeakers speakerlist, string iso, string name)
        {
            var entry = _context.ListOfSpeakersLog.FirstOrDefault(n => n.Speakerlist == speakerlist && n.SpeakerName == name);
            if (entry == null)
                entry = CreateLogEntry(speakerlist, iso, name);
            return entry;
        }

        private ListOfSpeakersLog CreateLogEntry(ListOfSpeakers speakerlist, string iso, string name)
        {
            var entry = new ListOfSpeakersLog()
            {
                SpeakerIso = iso,
                Speakerlist = speakerlist,
                SpeakerName = name
            };
            _context.ListOfSpeakersLog.Add(entry);
            _context.SaveChanges();
            return entry;
        }

        public async Task<ISpeaker> AddQuestion(AddSpeakerBody body)
        {
            var list = await _context.ListOfSpeakers
                .Include(n => n.AllSpeakers)
                .FirstOrDefaultAsync(n => n.ListOfSpeakersId == body.ListOfSpeakersId);
            if (list == null) return null;

            var existing = list.AllSpeakers.FirstOrDefault(n => n.Iso == body.Iso &&
            n.Name == body.Name && n.Mode == SpeakerModes.WaitForQuesiton);

            if (existing != null) 
                return existing;

            var mdl = list.AddQuestion(body.Name, body.Iso);
            //list.AllSpeakers.Add(mdl);
            await _context.SaveChangesAsync();
            return mdl;
        }

        public ISpeaker AddQuestion(string listId, string iso, string name)
        {
            var list = _context.ListOfSpeakers
                .Include(n => n.AllSpeakers)
                .FirstOrDefault(n => n.ListOfSpeakersId == listId);
            if (list == null) return null;

            return AddQuestion(list, iso, name);
        }

        public ISpeaker AddQuestion(ListOfSpeakers list, string iso, string name)
        {
            var existing = list.AllSpeakers.FirstOrDefault(n => n.Iso == iso &&
            n.Name == name && n.Mode == SpeakerModes.WaitForQuesiton);

            if (existing != null)
                return existing;

            var mdl = list.AddQuestion(name, iso);
            var logEntry = EnsureLogEntry(list, iso, name);
            logEntry.TimesOnQuestions += 1;
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
            var currentQuestion = _context.Speakers.Include(n => n.ListOfSpeakers)
                .FirstOrDefault(n => n.ListOfSpeakers.ListOfSpeakersId == listId && n.Mode == SpeakerModes.CurrentQuestion);
            if (currentQuestion == null) return false;

            var logEntry = EnsureLogEntry(currentQuestion.ListOfSpeakers, currentQuestion.Iso, currentQuestion.Name);
            logEntry.PermitedQuestionsSeconds += seconds;

            currentQuestion.ListOfSpeakers.AddQuestionSeconds(seconds);
            this._context.SaveChanges();
            return true;
        }

        internal bool AddSpeakerSeconds(AddSpeakerSeconds body)
        {
            return AddSpeakerSeconds(body.ListOfSpeakersId, body.Seconds);
        }

        internal bool AddSpeakerSeconds(string listId, int seconds)
        {
            var currentSpeaker = _context.Speakers.Include(n => n.ListOfSpeakers)
                .FirstOrDefault(n => n.ListOfSpeakers.ListOfSpeakersId == listId && n.Mode == SpeakerModes.CurrentQuestion);
            if (currentSpeaker == null) return false;

            var logEntry = EnsureLogEntry(currentSpeaker.ListOfSpeakers, currentSpeaker.Iso, currentSpeaker.Name);
            logEntry.PermitedQuestionsSeconds += seconds;

            currentSpeaker.ListOfSpeakers.AddSpeakerSeconds(seconds);
            this._context.SaveChanges();
            return true;
        }

        public ListOfSpeakers GetSpeakerlistByPublicId(string publicId)
        {
            return this._context.ListOfSpeakers.Include(n => n.AllSpeakers).FirstOrDefault(n => n.PublicId == publicId);
        }

        public SpeakerlistService(MunityContext context)
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

            // Save time of speaker
            if (list.CurrentSpeaker != null)
            {
                var speakerEntry = EnsureLogEntry(list, list.CurrentSpeaker.Iso, list.CurrentSpeaker.Name);
                if (speakerEntry != null)
                {
                    speakerEntry.UsedSpeakerSeconds += (long)(list.RemainingSpeakerTime.TotalSeconds - list.SpeakerTime.TotalSeconds) * -1;
                }
            }

            var result = list.NextSpeaker();
            var logNewSpeaker = EnsureLogEntry(list, result.Iso, result.Name);
            if (logNewSpeaker != null)
            {
                logNewSpeaker.TimesSpeaking += 1;
                logNewSpeaker.PermitedSpeakingSeconds += (long)list.SpeakerTime.TotalSeconds;
            }
                
            _context.SaveChanges();
            return true;
        }

        internal bool NextQuestion(string listid)
        {
            var list = this._context.ListOfSpeakers
                .Include(n => n.AllSpeakers)
                .FirstOrDefault(n => n.ListOfSpeakersId == listid);
            if (list == null) return false;

            // Save time of speaker
            if (list.CurrentQuestion != null)
            {
                var speakerEntry = EnsureLogEntry(list, list.CurrentQuestion.Iso, list.CurrentQuestion.Name);
                if (speakerEntry != null)
                {
                    speakerEntry.UsedQuestionSeconds += (long)(list.RemainingQuestionTime.TotalSeconds - list.QuestionTime.TotalSeconds) * -1;
                }
            }

            var result = list.NextQuestion();

            var logNewSpeaker = EnsureLogEntry(list, result.Iso, result.Name);
            if (logNewSpeaker != null)
            {
                logNewSpeaker.TimesQuestioning += 1;
                logNewSpeaker.PermitedQuestionsSeconds += (long)list.QuestionTime.TotalSeconds;
            }
            _context.SaveChanges();
            return true;
        }

        internal DateTime? ResumeSpeaker(ListOfSpeakersRequest body)
        {
            return this.ResumeSpeaker(body.ListOfSpeakersId);
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

        internal TimeSpan? SetSpeakerTime(string listId, string timeString)
        {
            var list = this._context.ListOfSpeakers
                .FirstOrDefault(n => n.ListOfSpeakersId == listId);
            if (list == null) return null;

            TimeSpan speakerTime;
            if (!TimeSpan.TryParseExact(timeString, @"mm\:ss", null, out speakerTime))
                return null;

            list.SpeakerTime = speakerTime;
            _context.SaveChanges();
            return speakerTime;
        }

        internal TimeSpan? SetQuestionTime(string listId, string timeString)
        {
            var list = this._context.ListOfSpeakers
                .FirstOrDefault(n => n.ListOfSpeakersId == listId);
            if (list == null) return null;

            TimeSpan questionTime;
            if (!TimeSpan.TryParseExact(timeString, @"mm\:ss", null, out questionTime))
                return null;

            list.QuestionTime = questionTime;
            _context.SaveChanges();
            return questionTime;
        }


        internal bool ClearSpeaker(ListOfSpeakersRequest body)
        {
            return ClearSpeaker(body.ListOfSpeakersId);
        }

        internal bool ClearSpeaker(string listId)
        {
            var speaker = _context.Speakers.Include(n => n.ListOfSpeakers).FirstOrDefault(n => n.ListOfSpeakers.ListOfSpeakersId == listId &&
            n.Mode == SpeakerModes.CurrentlySpeaking);
            if (speaker != null)
            {
                var speakerEntry = EnsureLogEntry(speaker.ListOfSpeakers, speaker.Iso, speaker.Name);
                if (speakerEntry != null)
                {
                    speakerEntry.UsedSpeakerSeconds += (long)(speaker.ListOfSpeakers.RemainingSpeakerTime.TotalSeconds - speaker.ListOfSpeakers.SpeakerTime.TotalSeconds) * -1;
                }

                _context.Speakers.Remove(speaker);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        internal bool ClearQuestion(ListOfSpeakersRequest body)
        {
            return ClearQuestion(body.ListOfSpeakersId);
        }

        internal bool ClearQuestion(string listId)
        {

            var speaker = _context.Speakers.Include(n => n.ListOfSpeakers).FirstOrDefault(n => n.ListOfSpeakers.ListOfSpeakersId == listId &&
            n.Mode == SpeakerModes.CurrentQuestion);
            if (speaker != null)
            {
                var speakerEntry = EnsureLogEntry(speaker.ListOfSpeakers, speaker.Iso, speaker.Name);
                if (speakerEntry != null)
                {
                    speakerEntry.UsedQuestionSeconds += (long)(speaker.ListOfSpeakers.RemainingQuestionTime.TotalSeconds - speaker.ListOfSpeakers.QuestionTime.TotalSeconds) * -1;
                }

                _context.Speakers.Remove(speaker);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        internal bool Pause(ListOfSpeakersRequest body)
        {
            return this.Pause(body.ListOfSpeakersId);
        }

        internal bool Pause(string listId)
        {
            var list = _context.ListOfSpeakers
                .Include(n => n.AllSpeakers)
                .FirstOrDefault(n => n.ListOfSpeakersId == listId);
            if (list == null) return false;
            list.Pause();
            _context.SaveChanges();
            return true;
        }

        internal bool CloseList(ListOfSpeakersRequest body)
        {
            return CloseList(body.ListOfSpeakersId);
        }

        internal bool CloseList(string listId)
        {
            var list = _context.ListOfSpeakers.FirstOrDefault(n => n.ListOfSpeakersId == listId);
            if (list == null)
                return false;
            list.ListClosed = true;
            _context.SaveChanges();
            return true;
        }

        internal bool OpenList(ListOfSpeakersRequest body)
        {
            return OpenList(body.ListOfSpeakersId);
        }

        internal bool OpenList(string listId)
        {
            var list = _context.ListOfSpeakers.FirstOrDefault(n => n.ListOfSpeakersId == listId);
            if (list == null)
                return false;
            list.ListClosed = false;
            _context.SaveChanges();
            return true;
        }

        internal bool CloseQuestions(ListOfSpeakersRequest body)
        {
            return CloseQuestions(body.ListOfSpeakersId);
        }

        internal bool CloseQuestions(string listId)
        {
            var list = _context.ListOfSpeakers.FirstOrDefault(n => n.ListOfSpeakersId == listId);
            if (list == null)
                return false;
            list.QuestionsClosed = true;
            _context.SaveChanges();
            return true;
        }

        internal bool OpenQuestions(ListOfSpeakersRequest body)
        {
            return OpenQuestions(body.ListOfSpeakersId);
        }

        internal bool OpenQuestions(string listId)
        {
            var list = _context.ListOfSpeakers.FirstOrDefault(n => n.ListOfSpeakersId == listId);
            if (list == null)
                return false;
            list.QuestionsClosed = false;
            _context.SaveChanges();
            return true;
        }

        internal List<Speaker> GetQuestions(string listId)
        {
            return _context.Speakers
                .Where(n => n.Mode == SpeakerModes.WaitForQuesiton && n.ListOfSpeakers.ListOfSpeakersId == listId)
                .AsNoTracking()
                .ToList();
        }

        internal List<Speaker> GetSpeakers(string listId)
        {
            return _context.Speakers
                .Where(n => n.Mode == SpeakerModes.WaitToSpeak && n.ListOfSpeakers.ListOfSpeakersId == listId)
                .AsNoTracking()
                .ToList();
        }

        internal List<ListOfSpeakersLog> GetLog(string listOfSpeakersId)
        {
            return _context.ListOfSpeakersLog.Where(n => n.Speakerlist.ListOfSpeakersId == listOfSpeakersId).OrderBy(n => n.SpeakerName).ToList();
        }
    }
}
