using Castle.Core.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MUNity.Database.Context;
using MUNity.Database.Models.VirtualCommittees;
using MUNity.Schema.VirtualCommittee;
using MUNity.Schema.VirtualCommittee.ViewModels;
using MUNity.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Services;

public class VirtualCommitteeService
{
    private readonly IServiceScopeFactory scopeFactory;

    private ObservableCollection<VirtualCommitteeConferenceViewModel> cachedConferences;

    public VirtualCommitteeViewModel GetVirtualCommittee(string id)
    {
        var existing = cachedConferences.SelectMany(n => n.VirtualCommittees).FirstOrDefault(n => n.VirtualCommitteeId == id);
        if (existing == null)
        {
            existing = LoadFromDatabase(id);
        }
        return existing;
    }

    public VirtualCommitteeConferenceViewModel CreateGroup(string v)
    {
        var databaseModel = new VirtualCommitteeGroup()
        {
            GroupName = v
        };
        var scope = scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<MunityContext>();
        context.VirtualCommitteeGroups.Add(databaseModel);
        context.SaveChanges();
        var viewModel = new VirtualCommitteeConferenceViewModel()
        {
            ConferenceId = databaseModel.VirtualCommitteeGroupId,
            ConferenceName = databaseModel.GroupName
        };
        cachedConferences.Add(viewModel);
        return viewModel;
    }

    public VirtualCommitteeConferenceViewModel GetGroupByName(string name)
    {
        var result = cachedConferences.FirstOrDefault(n => n.ConferenceName == name);
        if (result == null)
        {
            using var scope = scopeFactory.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<MunityContext>();
            result = context.VirtualCommitteeGroups.Where(n => n.GroupName == name)
                .Select(n => new VirtualCommitteeConferenceViewModel()
                {
                    ConferenceId = n.VirtualCommitteeGroupId,
                    ConferenceName= n.GroupName
                })
                .FirstOrDefault();

            foreach(var committeeId in context.VirtualCommittees.Where(n => n.Group.VirtualCommitteeGroupId == result.ConferenceId).Select(n => n.VirtualCommitteeId))
            {
                var committeeViewModel = LoadFromDatabase(committeeId);
                result.VirtualCommittees.Add(committeeViewModel);
            }
            cachedConferences.Add(result);
        }
        return result;
    }

    public VirtualCommitteeSlotViewModel AddSlot(VirtualCommitteeViewModel gvViewModel, string iso, string name)
    {
        using var scope = scopeFactory.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<MunityContext>();

        var dbModel = new VirtualCommitteeSlot()
        {
            RoleName = name,
            Iso = iso
        };
        context.VirtualCommitteeSlots.Add(dbModel);
        context.SaveChanges();
        var viewModel = new VirtualCommitteeSlotViewModel()
        {
            RoleName = name,
            VirtualCommitteeSlotId = dbModel.VirtualCommitteeSlotId
        };

        gvViewModel.Slots.Add(viewModel);
        return viewModel;
    }

    private VirtualCommitteeViewModel LoadFromDatabase(string id)
    {
        using var scope = scopeFactory.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<MunityContext>();

        return context.VirtualCommittees.Select(n => new VirtualCommitteeViewModel()
        {
            VirtualCommitteeId = n.VirtualCommitteeId,
            Name = n.Name,
            AdminPassword = n.AdminPassword,
            JoinPassword = n.JoinPassword,

            Slots = n.VirtualCommitteeSlots.Select(a => new VirtualCommitteeSlotViewModel()
            {
                DisplayUserName = a.DisplayUserName,
                RoleName = a.RoleName,
                UserToken = a.UserToken,
                VirtualCommitteeSlotId = a.VirtualCommitteeSlotId,
            }).ToObservableCollection(),

            ListOfSpeakers = new ViewModels.ListOfSpeakers.ListOfSpeakersViewModel()
            {
                AllSpeakers = n.ListOfSpeakers.AllSpeakers.Select(a => new ViewModels.ListOfSpeakers.SpeakerViewModel()
                {
                    Id = a.Id,
                    Iso = a.Iso,
                    Mode = a.Mode,
                    Name = a.Name,
                    OrdnerIndex = a.OrdnerIndex,
                }).ToObservableCollection(),
                ListClosed = n.ListOfSpeakers.ListClosed,
                Name = n.Name,
                ListOfSpeakersId = n.ListOfSpeakers.ListOfSpeakersId,
                PausedQuestionTime = n.ListOfSpeakers.PausedSpeakerTime,
                PausedSpeakerTime = n.ListOfSpeakers.PausedSpeakerTime,
                PublicId = n.ListOfSpeakers.PublicId,
                QuestionsClosed = n.ListOfSpeakers.QuestionsClosed,
                SpeakerTime = n.ListOfSpeakers.SpeakerTime,
                QuestionTime = n.ListOfSpeakers.QuestionTime,
                StartQuestionTime = n.ListOfSpeakers.StartQuestionTime,
                StartSpeakerTime = n.ListOfSpeakers.StartSpeakerTime,
                Status = n.ListOfSpeakers.Status,
            }
        }).FirstOrDefault(n => n.VirtualCommitteeId == id);
    }

    public VirtualCommitteeViewModel CreateVirtualCommittee(VirtualCommitteeConferenceViewModel conference, string committeeName, string adminPassword)
    {
        using var scope = scopeFactory.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<MunityContext>();
        var databaseModel = new VirtualCommittee();
        databaseModel.Name = committeeName;
        databaseModel.Group = context.VirtualCommitteeGroups.Find(conference.ConferenceId);
        databaseModel.AdminPassword = adminPassword;
        context.VirtualCommittees.Add(databaseModel);
        context.SaveChanges();

        var committee = new VirtualCommitteeViewModel()
        {
            AdminPassword = adminPassword,
            VirtualCommitteeId = databaseModel.VirtualCommitteeId,
        };
        conference.VirtualCommittees.Add(committee);
        return committee;
    }


    public VirtualCommitteeService(IServiceScopeFactory scopeFactory)
    {
        this.cachedConferences = new ObservableCollection<VirtualCommitteeConferenceViewModel>();
        this.scopeFactory = scopeFactory;
    }
}
