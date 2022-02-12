using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MUNity.Database.Context;
using MUNity.Database.Models.LoS;
using MUNity.Services.Extensions.CastExtensions;
using MUNity.ViewModels.ListOfSpeakers;
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
