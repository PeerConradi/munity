using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MUNity.Schema.VirtualCommittee.ViewModels
{
    public class VirtualCommitteeConferenceViewModel
    {
        public string ConferenceId { get; set; }

        public string ConferenceName { get; set; }

        public ObservableCollection<VirtualCommitteeViewModel> VirtualCommittees { get; set; }

        public VirtualCommitteeConferenceViewModel()
        {
            this.ConferenceId = Guid.NewGuid().ToString();
            this.VirtualCommittees = new ObservableCollection<VirtualCommitteeViewModel>();
        }
    }

}
