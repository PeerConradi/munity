using Castle.Core.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MUNity.Database.Context;
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

    private ObservableCollection<VirtualCommitteeViewModel> virtualCommittees { get; set; }

    public VirtualCommitteeViewModel GetVirtualCommittee(string id)
    {
        var existing = virtualCommittees.FirstOrDefault(n => n.VirtualCommitteeId == id);
        if (existing == null)
        {
            existing = LoadFromDatabase(id);
        }
        return existing;
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

    public VirtualCommitteeViewModel CreateVirtualCommittee()
    {
        var committee = new VirtualCommitteeViewModel();
        this.virtualCommittees.Add(committee);
        return committee;
    }

    public void AddVirtualCommittee(VirtualCommitteeViewModel viewModel)
    {
        this.virtualCommittees.Add(viewModel);
    }

    public VirtualCommitteeService(IServiceScopeFactory scopeFactory)
    {
        this.virtualCommittees = new ObservableCollection<VirtualCommitteeViewModel>();
        this.scopeFactory = scopeFactory;
    }
}
