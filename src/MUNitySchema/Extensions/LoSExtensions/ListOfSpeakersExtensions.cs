using System;
using System.Collections.Generic;
using System.Text;
using MUNity.Models.ListOfSpeakers;
using System.Linq;
using static MUNity.Models.ListOfSpeakers.ListOfSpeakers;

namespace MUNity.Extensions.LoSExtensions
{


    /// <summary>
    /// This Extension let you control the List of Speaker and fill it with Logic.
    /// </summary>
    public static class ListOfSpeakersExtensions
    {

        /// <summary>
        /// Sets the next speaker as current speaker and removes him/her from the list.
        /// If there is noone in the Speakers the current speaker will be set to null.
        /// This call will also clear the list of Questions and remove the current question.
        /// This will set the speaking mode to stopped.
        /// </summary>
        /// <param name="list"></param>
        public static void NextSpeaker(this ListOfSpeakers list)
        {
            if (list.Speakers.Any(n => n.Mode == Speaker.SpeakerModes.WaitToSpeak))
            {

                // Remove all Questions, Current Speakers and the one currently asking a Question.
                var questions = list.AllSpeakers.Where(n => n.Mode != Speaker.SpeakerModes.WaitToSpeak).ToList();
                questions.ForEach(n => list.AllSpeakers.Remove(n));

                // Remove the current Question
                list.ClearCurrentQuestion();

                // Pick the first speaker in line
                var nextSpeaker = list.AllSpeakers.OrderBy(n => n.OrdnerIndex).First();
                nextSpeaker.Mode = Speaker.SpeakerModes.CurrentlySpeaking;
                list.NotifyPropertyChanged(nameof(list.CurrentSpeaker));
                list.NotifyPropertyChanged(nameof(list.CurrentQuestion));
                list.NotifyPropertyChanged(nameof(list.Questions));
                list.NotifyPropertyChanged(nameof(list.Speakers));
            }
            else
            {
                list.ClearCurrentSpeaker();
            }
            list.Status = EStatus.Stopped;
        }

        /// <summary>
        /// Sets the next one from the list of questions as the current Question. If there
        /// is noone in the list of questions the CurrentQuestion will bet set to zero.
        /// In either case the Mode/Status will be set to STOPPED.
        /// </summary>
        /// <param name="list"></param>
        public static void NextQuestion(this ListOfSpeakers list)
        {
            if (list.Questions.Any())
            {
                // Delete the current Questions (remove all of this type of there is a bug and for some reason two are current Speaker)
                var currentQuestion = list.AllSpeakers.Where(n => n.Mode == Speaker.SpeakerModes.CurrentQuestion).ToList();
                currentQuestion.ForEach(n => list.AllSpeakers.Remove(n));

                var nextQuestion = list.AllSpeakers.Where(n => n.Mode == Speaker.SpeakerModes.WaitForQuesiton).OrderBy(n => n.OrdnerIndex).First();
                nextQuestion.Mode = Speaker.SpeakerModes.CurrentQuestion;
                list.NotifyPropertyChanged(nameof(list.Questions));
                list.NotifyPropertyChanged(nameof(list.CurrentQuestion));
            }
            else
            {
                list.ClearCurrentQuestion();
            }

            if (list.Status == EStatus.Speaking)
            {
                PauseSpeaker(list);
            }
            else
            {
                list.Status = EStatus.Stopped;
            }
            
        }

        /// <summary>
        /// THis will give the Speaker the default SpeakerTime and set the Mode to speaking.
        /// </summary>
        /// <param name="list"></param>
        private static void StartSpeaker(this ListOfSpeakers list)
        {
            if (list.CurrentSpeaker != null)
            {
                list.PausedQuestionTime = list.QuestionTime;
                list.StartSpeakerTime = DateTime.Now.ToUniversalTime();
                list.Status = EStatus.Speaking;
            }
        }

        public static void ResetSpeakerTime(this ListOfSpeakers list)
        {
            list.StartSpeakerTime = DateTime.Now.ToUniversalTime();
        }

        public static void ResetQuestionTime(this ListOfSpeakers list)
        {
            list.StartQuestionTime = DateTime.Now.ToUniversalTime();
        }

        /// <summary>
        /// Sets the SPeaker as answering. The speaker is now getting the question time as a default
        /// value.
        /// </summary>
        /// <param name="list"></param>
        public static void StartAnswer(this ListOfSpeakers list)
        {
            if (list.CurrentSpeaker != null)
            {
                list.PausedQuestionTime = list.QuestionTime;
                list.StartSpeakerTime = DateTime.Now.ToUniversalTime();
                list.Status = EStatus.Answer;
            }
            else
            {
                list.Status = EStatus.Stopped;
            }
        }

        /// <summary>
        /// This will give the Question the full QuestionTime and set the Mode to question.
        /// </summary>
        /// <param name="list"></param>
        private static void StartQuestion(this ListOfSpeakers list)
        {
            if (list.CurrentQuestion != null)
            {
                // Reset the current Speaker time
                list.PausedSpeakerTime = list.SpeakerTime;
                list.StartQuestionTime = DateTime.Now.ToUniversalTime();
                list.Status = EStatus.Question;
            }
        }

        /// <summary>
        /// This will pause the speaker time. And set the Status to Either SpeakerPaused or AnswerPaused depending on the current
        /// state of the speaker. if the speaker is not talking when this method is called it will set the Status to Stopped.
        /// </summary>
        /// <param name="list"></param>
        private static void PauseSpeaker(this ListOfSpeakers list)
        {
            list.PausedSpeakerTime = list.RemainingSpeakerTime;
            if (list.Status == EStatus.Speaking)
                list.Status = EStatus.SpeakerPaused;
            else if (list.Status == EStatus.Answer)
                list.Status = EStatus.AnswerPaused;
        }

        /// <summary>
        /// This will pause the question time. And set the status to Question Paused.
        /// If the Question is not running when this method is called it will set the Status to stopped.
        /// The PauseQuestion Time will be saved in Either case.
        /// </summary>
        /// <param name="list"></param>
        private static void PauseQuestion(this ListOfSpeakers list)
        {

            list.PausedQuestionTime = list.RemainingQuestionTime;
            if (list.Status == EStatus.Question)
                list.Status = EStatus.QuestionPaused;
        }

        public static void Pause(this ListOfSpeakers list)
        {
            if (list.Status == EStatus.Question)
                list.PauseQuestion();
            else if (list.Status == EStatus.Speaking || list.Status == EStatus.Answer)
                list.PauseSpeaker();
        }

        /// <summary>
        /// Lets a speaker continue with the remaining time, depending if he/she is in the Speaking or Answer Mode.
        /// </summary>
        /// <param name="list"></param>
        public static void ResumeSpeaker(this ListOfSpeakers list)
        {
            if (list.CurrentSpeaker != null)
            {
                if (list.Status == EStatus.SpeakerPaused)
                    list.ContinueSpeaker();
                else if (list.Status == EStatus.AnswerPaused)
                    list.ContinueAnswer();
                else
                    list.StartSpeaker();

                // Fixes a small glitch in the Question time!
                list.StartQuestionTime = DateTime.Now.ToUniversalTime();
                
            }
            else
            {
                list.Status = EStatus.Stopped;
            }
        }

        private static void ContinueAnswer(this ListOfSpeakers list)
        {
            list.StartSpeakerTime = DateTime.Now.ToUniversalTime().AddSeconds(list.RemainingSpeakerTime.TotalSeconds - list.QuestionTime.TotalSeconds);
            list.Status = EStatus.Answer;
        }

        private static void ContinueSpeaker(this ListOfSpeakers list)
        {
            list.StartSpeakerTime = DateTime.Now.ToUniversalTime().AddSeconds(list.RemainingSpeakerTime.TotalSeconds - list.SpeakerTime.TotalSeconds);
            list.Status = EStatus.Speaking;
        }

        /// <summary>
        /// Let you continue with the last given Question time.
        /// </summary>
        /// <param name="list"></param>
        public static void ResumeQuestion(this ListOfSpeakers list)
        {
            if (list.CurrentQuestion != null)
            {
                if (list.Status == EStatus.QuestionPaused)
                {
                    list.StartQuestionTime = DateTime.Now.ToUniversalTime().AddSeconds(list.RemainingQuestionTime.TotalSeconds - list.QuestionTime.TotalSeconds);
                }
                else
                {
                    list.StartQuestion();
                }
                
                list.Status = EStatus.Question;
            }
            else
            {
                list.Status = EStatus.Stopped;
            }
        }

        /// <summary>
        /// Creates a new instance of s speaker and adds it to the end of speakers.
        /// The speaker will get an Id from a new Guid.
        /// </summary>
        /// <param name="list">The list of speakers that this should be added to.</param>
        /// <param name="name">The display name of the speaker.</param>
        /// <param name="iso">The iso that could be used to get an icon.</param>
        /// <returns></returns>
        public static Speaker AddSpeaker(this ListOfSpeakers list, string name, string iso = "")
        {
            var newSpeaker = new Speaker()
            {
                Id = Guid.NewGuid().ToString(),
                Iso = iso,
                Name = name,
                ListOfSpeakers = list,
                Mode = Speaker.SpeakerModes.WaitToSpeak
            };
            if (list.Speakers.Any())
            {
                newSpeaker.OrdnerIndex = list.Speakers.Max(n => n.OrdnerIndex) + 1;
            }
            list.AllSpeakers.Add(newSpeaker);
            return newSpeaker;
        }

        /// <summary>
        /// Adds someone to the list of questions.
        /// </summary>
        /// <param name="list">The list that this should be added to.</param>
        /// <param name="name">The display name that should be shown inside the list of questions and the current question.</param>
        /// <param name="iso">The iso that can be used to find an icon.</param>
        /// <returns></returns>
        public static Speaker AddQuestion(this ListOfSpeakers list, string name, string iso = "")
        {
            var newSpeaker = new Speaker()
            {
                Id = Guid.NewGuid().ToString(),
                Iso = iso,
                Name = name,
                ListOfSpeakers = list,
                Mode = Speaker.SpeakerModes.WaitForQuesiton
            };
            if (list.Questions.Any())
            {
                newSpeaker.OrdnerIndex = list.Questions.Max(n => n.OrdnerIndex) + 1;
            }
            list.AllSpeakers.Add(newSpeaker);
            return newSpeaker;
        }

        /// <summary>
        /// Resets the Current Speaker and sets that Status to Stopped if the current Status has something to do with the speaker (talking or paused).
        /// </summary>
        /// <param name="list"></param>
        public static void ClearCurrentSpeaker(this ListOfSpeakers list)
        {
            if (list.Status == EStatus.Speaking || list.Status == EStatus.SpeakerPaused || list.Status == EStatus.Answer || list.Status == EStatus.AnswerPaused)
                list.Status = EStatus.Stopped;
            // Delete the current Speaker (remove all of this type of there is a bug and for some reason two are current Speaker)
            var currentSpeakers = list.AllSpeakers.Where(n => n.Mode == Speaker.SpeakerModes.CurrentlySpeaking).ToList();
            currentSpeakers.ForEach(n => list.AllSpeakers.Remove(n));
            list.NotifyPropertyChanged(nameof(list.CurrentSpeaker));
        }

        /// <summary>
        /// Removes the current Question and sets the status to stopped if the CurrentQuestion was talking of is paused.
        /// </summary>
        /// <param name="list"></param>
        public static void ClearCurrentQuestion(this ListOfSpeakers list)
        {
            if (list.Status == EStatus.Question || list.Status == EStatus.QuestionPaused)
                list.Status = EStatus.Stopped;
            // Delete the current Questions (remove all of this type of there is a bug and for some reason two are current Speaker)
            var currentQuestion = list.AllSpeakers.Where(n => n.Mode == Speaker.SpeakerModes.CurrentQuestion).ToList();
            currentQuestion.ForEach(n => list.AllSpeakers.Remove(n));
            list.NotifyPropertyChanged(nameof(list.CurrentQuestion));
        }

        /// <summary>
        /// Gives the speaker some extra seconds. Use a negativ value to remove seconds.
        /// Note that this does not work when the speaker is paused!
        /// </summary>
        /// <param name="list"></param>
        /// <param name="seconds"></param>
        public static void AddSpeakerSeconds(this ListOfSpeakers list, int seconds)
        {
            list.StartSpeakerTime = list.StartSpeakerTime.AddSeconds(seconds);
        }

        /// <summary>
        /// Gives the one asking a question the given amount of seconds or removes them if you give a negativ value.
        /// Note that this does not work when the Question is Paused!
        /// </summary>
        /// <param name="list"></param>
        /// <param name="seconds"></param>
        public static void AddQuestionSeconds(this ListOfSpeakers list, int seconds)
        {
            list.StartQuestionTime = list.StartQuestionTime.AddSeconds(seconds);
        }
    }
}
