using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MUNity.Database.Context;
using MUNity.Database.Models.LoS;
using MUNity.Services.Extensions.CastExtensions;
using MUNity.ViewModels.ListOfSpeakers;
using MUNityBase.Interfances;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Services
{
    public class ListOfSpeakersService
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        private readonly ObservableCollection<ListOfSpeakersViewModel> cachedLists;

        private ILogger<ListOfSpeakersService> logger;

        public ListOfSpeakersViewModel GetViewModel(string id)
        {
            var existing = cachedLists.FirstOrDefault(n => n.ListOfSpeakersId == id);
            if (existing == null)
            {
                existing = LoadFromDatabase(id);
                if (existing != null)
                    cachedLists.Add(existing);
            }
            return existing;
        }

        private ListOfSpeakersViewModel LoadFromDatabase(string id)
        {
            using var scope = serviceScopeFactory.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<MunityContext>();

            var viewModel = context.ListOfSpeakers
                .Where(n => n.ListOfSpeakersId == id)
                .FirstOrDefault()?
                .ToViewModel();
            return viewModel;
        }

        public ListOfSpeakersViewModel CreateList(string id = null)
        {
            using var scope = serviceScopeFactory.CreateScope();
            using var service = scope.ServiceProvider.GetRequiredService<ListOfSpeakersDatabaseService>();
            var list = service.CreateList(id ?? Guid.NewGuid().ToString());
            var viewModel = list.ToViewModel();
            this.cachedLists.Add(viewModel);
            return viewModel;
        }

        public async Task<ListOfSpeakersViewModel> CreateListAsync()
        {
            using var scope = serviceScopeFactory.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<MunityContext>();
            var databaseModel = new ListOfSpeakers();
            context.ListOfSpeakers.Add(databaseModel);
            await context.SaveChangesAsync();
            var viewModel = databaseModel.ToViewModel();
            lock (this.cachedLists)
            {
                this.cachedLists.Add(viewModel);
            }
            return viewModel;
        }

        public bool AddSpeaker(string speakerListId, string name, string iso)
        {
            var listViewModel = GetViewModel(speakerListId);
            if (listViewModel == null)
                return false;

            using var scope = serviceScopeFactory.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<MunityContext>();

            context.Speakers.Add(new Speaker()
            {
                Iso = iso,
                Name = name,
                ListOfSpeakers = context.ListOfSpeakers.Find(speakerListId),
                Mode = Base.SpeakerModes.WaitToSpeak,
                OrdnerIndex = context.Speakers.Count(n => n.ListOfSpeakers.ListOfSpeakersId == speakerListId && n.Mode == Base.SpeakerModes.WaitToSpeak)
            });
            var changes = context.SaveChanges();

            listViewModel.AddSpeaker(name, iso);

            if (changes != 1)
            {
                logger?.LogError($"Expected one (1) change to be performed when a new speaker should have been added to list {speakerListId} but was {changes}.");
            }

            return changes == 1;
        }

        public bool MoveSpeakerUp(string listId, Speaker speaker)
		{
            var listViewModel = GetViewModel(listId);

            using var scope = serviceScopeFactory.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<MunityContext>();

            var swappingEntry = listViewModel.AllSpeakers.FirstOrDefault(n => n.Mode == speaker.Mode && n.OrdnerIndex == speaker.OrdnerIndex - 1);
            context.Update(swappingEntry);
            context.Update(speaker);

            swappingEntry.OrdnerIndex++;
            speaker.OrdnerIndex--;

            context.SaveChanges();
            listViewModel.NotifyPropertyChanged(nameof(ListOfSpeakersViewModel.Speakers));
            listViewModel.NotifyPropertyChanged(nameof(ListOfSpeakersViewModel.Questions));

            return false;
		}

        public bool MoveSpeakerUp(string listId, SpeakerViewModel speaker)
        {
            var listViewModel = GetViewModel(listId);

            using var scope = serviceScopeFactory.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<MunityContext>();

            var swappingEntry = listViewModel.AllSpeakers.FirstOrDefault(n => n.Mode == speaker.Mode && n.OrdnerIndex == speaker.OrdnerIndex - 1);
            var original = context.Speakers.FirstOrDefault(n => n.Id == speaker.Id);
            var swapEntry = context.Speakers.FirstOrDefault(n => n.Id == swappingEntry.Id);

            speaker.OrdnerIndex--;
            swappingEntry.OrdnerIndex++;

            if (original != null)
                original.OrdnerIndex--;

            if (swapEntry != null)
                swapEntry.OrdnerIndex++;
            //swapEntry.OrdnerIndex++;
            //original.OrdnerIndex--;

            //context.SaveChanges();
            listViewModel.NotifyPropertyChanged(nameof(ListOfSpeakersViewModel.Speakers));
            listViewModel.NotifyPropertyChanged(nameof(ListOfSpeakersViewModel.Questions));

            return false;
        }

        public bool MoveSpeakerDown(string listId, SpeakerViewModel speaker)
        {
            if (speaker == null)
                return false;

            var listViewModel = GetViewModel(listId);

            using var scope = serviceScopeFactory.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<MunityContext>();

            var swappingEntry = listViewModel.AllSpeakers.FirstOrDefault(n => n.Mode == speaker.Mode && n.OrdnerIndex == speaker.OrdnerIndex + 1);
            if (swappingEntry == null)
                return false;

            var original = context.Speakers.FirstOrDefault(n => n.Id == speaker.Id);
            var swapEntry = context.Speakers.FirstOrDefault(n => n.Id == swappingEntry.Id);

            speaker.OrdnerIndex++;
            swappingEntry.OrdnerIndex--;

            if (original != null)
                original.OrdnerIndex++;

            if (swapEntry != null)
                swapEntry.OrdnerIndex--;
            //swapEntry.OrdnerIndex++;
            //original.OrdnerIndex--;

            //context.SaveChanges();
            listViewModel.NotifyPropertyChanged(nameof(ListOfSpeakersViewModel.Speakers));
            listViewModel.NotifyPropertyChanged(nameof(ListOfSpeakersViewModel.Questions));

            return false;
        }

        public bool UpdateSpeakerTime(string speakerListId, TimeSpan newTime)
        {
            var listViewModel = GetViewModel(speakerListId);
            if (listViewModel == null)
                return false;

            using var scope = serviceScopeFactory.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<MunityContext>();
            
            listViewModel.SpeakerTime = newTime;
            var sourceList = context.ListOfSpeakers.FirstOrDefault(n => n.ListOfSpeakersId.Equals(speakerListId));
            if (sourceList != null)
            {
                sourceList.SpeakerTime = newTime;
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateQuestionsTime(string speakerListId, TimeSpan newTime)
        {
            var listViewModel = GetViewModel(speakerListId);
            if (listViewModel == null)
                return false;

            using var scope = serviceScopeFactory.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<MunityContext>();

            listViewModel.QuestionTime = newTime;
            var sourceList = context.ListOfSpeakers.FirstOrDefault(n => n.ListOfSpeakersId.Equals(speakerListId));
            if (sourceList != null)
            {
                sourceList.QuestionTime = newTime;
                context.SaveChanges();
                return true;
            }
            else
            {
                logger?.LogError("Was unable  to update the questions time to {0} because the list of speakers '{1}' was not found",newTime, speakerListId);
                return false;
            }
        }

        public bool AddQuestion(string speakerListId, string name, string iso)
        {
            var listViewModel = GetViewModel(speakerListId);
            if (listViewModel == null)
                return false;

            listViewModel.AddQuestion(name, iso);
            return true;
        }

        public ListOfSpeakersService(IServiceScopeFactory scopeFactory)
        {
            this.serviceScopeFactory = scopeFactory;
            cachedLists = new ObservableCollection<ListOfSpeakersViewModel>();

            using var scope = scopeFactory.CreateScope();
            this.logger = scope.ServiceProvider.GetRequiredService<ILogger<ListOfSpeakersService>>();
        }
    }
}
