using MUNity.Schema.General;
using MUNity.ViewModels.ListOfSpeakers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MUNity.Schema.VirtualCommittee.ViewModels
{
    public class VirtualCommitteeViewModel : AbstractViewModel
    {

        public string VirtualCommitteeId { get; set; }

        public string Name { get; set; }

        public string AdminPassword { get; set; }

        public string JoinPassword { get; set; }

        public ObservableCollection<VirtualCommitteeSlotViewModel> Slots { get; set; }

        public ListOfSpeakersViewModel ListOfSpeakers { get; set; }

        public VirtualCommitteeViewModel()
        {
            Slots = new ObservableCollection<VirtualCommitteeSlotViewModel>();
            ListOfSpeakers = new ListOfSpeakersViewModel();
        }
    }

    
}
