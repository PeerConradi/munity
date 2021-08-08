using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using MUNity.ViewModels.ListOfSpeakers;
using MUNityBase;

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
        public static (SpeakerViewModel last, SpeakerViewModel newSpeaker) NextSpeaker(this ListOfSpeakersViewModel list)
        {
            SpeakerViewModel lastSpeaker = list.CurrentSpeaker;
            if (list.Speakers.Any(n => n.Mode == SpeakerModes.WaitToSpeak))
            {

                // Remove all Questions, Current Speakers and the one currently asking a Question.
                var questions = list.AllSpeakers.Where(n => n.Mode != SpeakerModes.WaitToSpeak).ToList();
                questions.ForEach(n => list.AllSpeakers.Remove(n));

                // Remove the current Question
                list.ClearCurrentQuestion();

                // Pick the first speaker in line
                var nextSpeaker = list.AllSpeakers.OrderBy(n => n.OrdnerIndex).First();
                nextSpeaker.Mode = SpeakerModes.CurrentlySpeaking;
                list.NotifyPropertyChanged(nameof(list.CurrentSpeaker));
                list.NotifyPropertyChanged(nameof(list.CurrentQuestion));
                list.NotifyPropertyChanged(nameof(list.Questions));
                list.NotifyPropertyChanged(nameof(list.Speakers));
            }
            else
            {
                list.ClearCurrentSpeaker();
            }
            list.Status = ESpeakerListStatus.Stopped;
            return (lastSpeaker, list.CurrentSpeaker);
        }

        /// <summary>
        /// Sets the next one from the list of questions as the current Question. If there
        /// is noone in the list of questions the CurrentQuestion will bet set to zero.
        /// In either case the Mode/Status will be set to STOPPED.
        /// </summary>
        /// <param name="list"></param>
        public static (SpeakerViewModel last, SpeakerViewModel newSpeaker) NextQuestion(this ListOfSpeakersViewModel list)
        {
            var lastQuestion = list.CurrentQuestion;
            if (list.Questions.Any())
            {
                // Delete the current Questions (remove all of this type of there is a bug and for some reason two are current Speaker)
                var currentQuestion = list.AllSpeakers.Where(n => n.Mode == SpeakerModes.CurrentQuestion).ToList();
                currentQuestion.ForEach(n => list.AllSpeakers.Remove(n));

                var nextQuestion = list.AllSpeakers.Where(n => n.Mode == SpeakerModes.WaitForQuesiton).OrderBy(n => n.OrdnerIndex).FirstOrDefault();
                nextQuestion.Mode = SpeakerModes.CurrentQuestion;
                list.NotifyPropertyChanged(nameof(list.Questions));
                list.NotifyPropertyChanged(nameof(list.CurrentQuestion));
            }
            else
            {
                list.ClearCurrentQuestion();
            }

            if (list.Status == ESpeakerListStatus.Speaking)
            {
                PauseSpeaker(list);
            }
            else
            {
                list.Status = ESpeakerListStatus.Stopped;
            }
            return (lastQuestion, list.CurrentQuestion);
        }

        /// <summary>
        /// THis will give the Speaker the default SpeakerTime and set the Mode to speaking.
        /// </summary>
        /// <param name="list"></param>
        private static void StartSpeaker(this ListOfSpeakersViewModel list)
        {
            if (list.CurrentSpeaker != null)
            {
                list.PausedQuestionTime = list.QuestionTime;
                list.StartSpeakerTime = DateTime.Now.ToUniversalTime();
                list.Status = ESpeakerListStatus.Speaking;
            }
        }

        public static void ResetSpeakerTime(this ListOfSpeakersViewModel list)
        {
            list.StartSpeakerTime = DateTime.Now.ToUniversalTime();
        }

        public static void ResetQuestionTime(this ListOfSpeakersViewModel list)
        {
            list.StartQuestionTime = DateTime.Now.ToUniversalTime();
        }

        /// <summary>
        /// Sets the SPeaker as answering. The speaker is now getting the question time as a default
        /// value.
        /// </summary>
        /// <param name="list"></param>
        public static void StartAnswer(this ListOfSpeakersViewModel list)
        {
            if (list.CurrentSpeaker != null)
            {
                list.PausedQuestionTime = list.QuestionTime;
                list.StartSpeakerTime = DateTime.Now.ToUniversalTime();
                list.Status = ESpeakerListStatus.Answer;
            }
            else
            {
                list.Status = ESpeakerListStatus.Stopped;
            }
        }

        public static void StartAnswer(this ListOfSpeakersViewModel list, DateTime startTime)
        {
            list.StartSpeakerTime = startTime;
            list.StartAnswer();
        }

        /// <summary>
        /// This will give the Question the full QuestionTime and set the Mode to question.
        /// </summary>
        /// <param name="list"></param>
        private static void StartQuestion(this ListOfSpeakersViewModel list)
        {
            if (list.CurrentQuestion != null)
            {
                // Reset the current Speaker time
                list.PausedSpeakerTime = list.SpeakerTime;
                list.StartQuestionTime = DateTime.Now.ToUniversalTime();
                list.Status = ESpeakerListStatus.Question;
            }
        }

        /// <summary>
        /// This will pause the speaker time. And set the Status to Either SpeakerPaused or AnswerPaused depending on the current
        /// state of the speaker. if the speaker is not talking when this method is called it will set the Status to Stopped.
        /// </summary>
        /// <param name="list"></param>
        private static void PauseSpeaker(this ListOfSpeakersViewModel list)
        {
            list.PausedSpeakerTime = list.RemainingSpeakerTime;
            if (list.Status == ESpeakerListStatus.Speaking)
                list.Status = ESpeakerListStatus.SpeakerPaused;
            else if (list.Status == ESpeakerListStatus.Answer)
                list.Status = ESpeakerListStatus.AnswerPaused;
        }

        /// <summary>
        /// This will pause the question time. And set the status to Question Paused.
        /// If the Question is not running when this method is called it will set the Status to stopped.
        /// The PauseQuestion Time will be saved in Either case.
        /// </summary>
        /// <param name="list"></param>
        private static void PauseQuestion(this ListOfSpeakersViewModel list)
        {

            list.PausedQuestionTime = list.RemainingQuestionTime;
            if (list.Status == ESpeakerListStatus.Question)
                list.Status = ESpeakerListStatus.QuestionPaused;
        }

        public static ESpeakerListStatus Pause(this ListOfSpeakersViewModel list)
        {
            var oldState = list.Status;
            if (list.Status == ESpeakerListStatus.Question)
                list.PauseQuestion();
            else if (list.Status == ESpeakerListStatus.Speaking || list.Status == ESpeakerListStatus.Answer)
                list.PauseSpeaker();

            return oldState;
        }

        /// <summary>
        /// Lets a speaker continue with the remaining time, depending if he/she is in the Speaking or Answer Mode.
        /// </summary>
        /// <param name="list"></param>
        public static void ResumeSpeaker(this ListOfSpeakersViewModel list)
        {
            if (list.CurrentSpeaker != null)
            {
                if (list.Status == ESpeakerListStatus.SpeakerPaused)
                    list.ContinueSpeaker();
                else if (list.Status == ESpeakerListStatus.AnswerPaused)
                    list.ContinueAnswer();
                else
                    list.StartSpeaker();

                // Fixes a small glitch in the Question time!
                list.StartQuestionTime = DateTime.Now.ToUniversalTime();
                
            }
            else
            {
                list.Status = ESpeakerListStatus.Stopped;
            }
        }

        public static void ResumeSpeaker(this ListOfSpeakersViewModel list, DateTime startTime)
        {
            list.StartSpeakerTime = startTime;
            list.ResumeSpeaker();
        }

        private static void ContinueAnswer(this ListOfSpeakersViewModel list)
        {
            list.StartSpeakerTime = DateTime.Now.ToUniversalTime().AddSeconds(list.RemainingSpeakerTime.TotalSeconds - list.QuestionTime.TotalSeconds);
            list.Status = ESpeakerListStatus.Answer;
        }

        private static void ContinueSpeaker(this ListOfSpeakersViewModel list)
        {
            list.StartSpeakerTime = DateTime.Now.ToUniversalTime().AddSeconds(list.RemainingSpeakerTime.TotalSeconds - list.SpeakerTime.TotalSeconds);
            list.Status = ESpeakerListStatus.Speaking;
        }

        /// <summary>
        /// Let you continue with the last given Question time.
        /// </summary>
        /// <param name="list"></param>
        public static void ResumeQuestion(this ListOfSpeakersViewModel list)
        {
            if (list.CurrentQuestion != null)
            {
                if (list.Status == ESpeakerListStatus.QuestionPaused)
                {
                    list.StartQuestionTime = DateTime.Now.ToUniversalTime().AddSeconds(list.RemainingQuestionTime.TotalSeconds - list.QuestionTime.TotalSeconds);
                }
                else
                {
                    list.StartQuestion();
                }
                
                list.Status = ESpeakerListStatus.Question;
            }
            else
            {
                list.Status = ESpeakerListStatus.Stopped;
            }
        }

        public static void ResumeQuestion(this ListOfSpeakersViewModel list, DateTime startTime)
        {
            list.StartQuestionTime = startTime;
            list.ResumeQuestion();
        }

        

        /// <summary>
        /// Resets the Current Speaker and sets that Status to Stopped if the current Status has something to do with the speaker (talking or paused).
        /// </summary>
        /// <param name="list"></param>
        public static void ClearCurrentSpeaker(this ListOfSpeakersViewModel list)
        {
            if (list.Status == ESpeakerListStatus.Speaking || 
                list.Status == ESpeakerListStatus.SpeakerPaused || 
                list.Status == ESpeakerListStatus.Answer || 
                list.Status == ESpeakerListStatus.AnswerPaused)
                list.Status = ESpeakerListStatus.Stopped;
            // Delete the current Speaker (remove all of this type of there is a bug and for some reason two are current Speaker)
            var currentSpeakers = list.AllSpeakers.Where(n => n.Mode == SpeakerModes.CurrentlySpeaking).ToList();
            currentSpeakers.ForEach(n => list.AllSpeakers.Remove(n));
            list.NotifyPropertyChanged(nameof(list.CurrentSpeaker));
        }

        /// <summary>
        /// Removes the current Question and sets the status to stopped if the CurrentQuestion was talking of is paused.
        /// </summary>
        /// <param name="list"></param>
        public static void ClearCurrentQuestion(this ListOfSpeakersViewModel list)
        {
            if (list.Status == ESpeakerListStatus.Question || 
                list.Status == ESpeakerListStatus.QuestionPaused)
                list.Status = ESpeakerListStatus.Stopped;
            // Delete the current Questions (remove all of this type of there is a bug and for some reason two are current Speaker)
            var currentQuestion = list.AllSpeakers.Where(n => n.Mode == SpeakerModes.CurrentQuestion).ToList();
            currentQuestion.ForEach(n => list.AllSpeakers.Remove(n));
            list.NotifyPropertyChanged(nameof(list.CurrentQuestion));
        }

        /// <summary>
        /// Gives the speaker some extra seconds. Use a negativ value to remove seconds.
        /// Note that this does not work when the speaker is paused!
        /// </summary>
        /// <param name="list"></param>
        /// <param name="seconds"></param>
        public static void AddSpeakerSeconds(this ListOfSpeakersViewModel list, int seconds)
        {
            list.StartSpeakerTime = list.StartSpeakerTime.AddSeconds(seconds);
        }

        /// <summary>
        /// Gives the one asking a question the given amount of seconds or removes them if you give a negativ value.
        /// Note that this does not work when the Question is Paused!
        /// </summary>
        /// <param name="list"></param>
        /// <param name="seconds"></param>
        public static void AddQuestionSeconds(this ListOfSpeakersViewModel list, int seconds)
        {
            list.StartQuestionTime = list.StartQuestionTime.AddSeconds(seconds);
        }
    }
}
