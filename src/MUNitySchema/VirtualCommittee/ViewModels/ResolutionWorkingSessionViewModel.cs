using MUNity.Schema.General;
using System.Collections.ObjectModel;

namespace MUNity.Schema.VirtualCommittee.ViewModels
{
    public class ResolutionWorkingSessionViewModel : AbstractViewModel
    {
        public string ResolutionWorkingSessionId { get; set; }

        public Models.Resolution.Resolution Resolution { get; set; }

        public ObservableCollection<VirtualCommitteeSlotViewModel> WorkingMembers { get; set; }
    }

    
}
