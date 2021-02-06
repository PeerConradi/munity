using Microsoft.AspNetCore.SignalR.Client;
using MUNity.Hubs;
using MUNity.Models.Resolution;
using MUNity.Models.Resolution.EventArguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNity.Extensions.ResolutionExtensions;

namespace MUNityClient.ViewModel
{
    /// <summary>
    /// The ResaSocketHandler is connecting with the SignalR Socket of the MUNity API.
    /// </summary>
    public class ResolutionViewModel
    {

        public enum SyncModes
        {
            PingingServer,
            Offline,
            OnlineButNotSyncing,
            Syncing,
            Updated,
            ErrorWhenSyncing,
            Unkown
        }

        private SyncModes _syncMode = SyncModes.Unkown;
        public SyncModes SyncMode
        {
            get => _syncMode;
            set
            {
                if (_syncMode == value) return;
                _syncMode = value;
                SyncModeChanged?.Invoke(this, new EventArgs());
            }
        }

        private bool _isOnlineResolution = false;

        private MUNity.Observers.ResolutionObserver _resolutionObserver = null;

        private MUNityClient.Services.IResolutionService _resolutionService;

        public HubConnection HubConnection { get; set; }

        public List<string> IgnoreTransactions { get; set; }

        public event EventHandler<ResolutionChangedArgs> ResolutionChanged;

        public event EventHandler HeaderChangedFromExtern;

        public event EventHandler PreambleChangedFromExtern;

        public event EventHandler<PreambleParagraphChangedArgs> PreambleParagraphChanged;

        public event EventHandler<OperativeParagraphChangedEventArgs> OperativeParagraphChanged;

        public event EventHandler<AmendmentActivatedChangedEventArgs> AmendmentActivatedChanged;

        public event EventHandler<PreambleParagraphTextChangedEventArgs> PreambleParagraphTextChanged;

        public event EventHandler<OperativeParagraphTextChangedEventArgs> OperativeParagraphTextChanged;

        public event EventHandler<PreambleParagraphAddedEventArgs> PreambleParagraphAdded;

        public event EventHandler SyncModeChanged;

        public void InvokeHeaderChangedFromExtern(object sender, EventArgs args)
        {
            this.HeaderChangedFromExtern?.Invoke(sender, args);
        }

        public void InvokePreambleChangedFromExtern(object sender, EventArgs args)
        {
            this.PreambleChangedFromExtern?.Invoke(sender, args);
        }

        public Resolution Resolution { get; set; }

        private ResolutionViewModel(Resolution resolution, bool isOnline, MUNityClient.Services.IResolutionService resolutionService)
        {
            _isOnlineResolution = isOnline;
            this._resolutionService = resolutionService;
            this._resolutionObserver = new MUNity.Observers.ResolutionObserver(resolution);
            this._resolutionObserver.PreambleParagraphTextChanged += _resolutionObserver_PreambleParagraphTextChanged;
            Resolution = resolution;
            if (isOnline)
            {
                IgnoreTransactions = new List<string>();
                HubConnection = new HubConnectionBuilder().WithUrl($"{Program.API_URL}/resasocket").Build();

                var manipulator = new ViewModelLogic.ResolutionSocketViewModelManipulator(this);

                // Observer dazu bringen Updates an den Server zu senden
                var observerToServer = new ViewModelLogic.ResolutionObserverToServiceCalls(this._resolutionObserver, resolutionService);
            }
        }

        

        public bool VerifyTanAndRemoveItIfExisting(ResolutionEventArgs args)
        {
            if (this.IgnoreTransactions.Contains(args.Tan))
            {
                this.IgnoreTransactions.Remove(args.Tan);
                return true;
            }
            return false;
        }

        

        private void _resolutionObserver_PreambleParagraphTextChanged(object sender, PreambleParagraphTextChangedEventArgs args)
        {
            if (!_isOnlineResolution)
            {
                this._resolutionService.SaveOfflineResolution(this.Resolution);
            }
        }

        public OperativeParagraph AddOperativeParagraph()
        {
            throw new NotImplementedException();
            // Wenn Online
            //  Beim Server anfrangen
            // Wenn offline
            //  anlegen und lokal speichern
        }

        public void AddPreambleParagraph()
        {
            if (!_isOnlineResolution)
            {
                var paragraph = this.Resolution.CreatePreambleParagraph();
                this.PreambleParagraphAdded?.Invoke(this, new PreambleParagraphAddedEventArgs(this.Resolution.ResolutionId, paragraph));
                this._resolutionService.SaveOfflineResolution(this.Resolution);
            }
            else
            {
                // TODO: Request creating Preamble Paragraph at the server
                this.Resolution.CreatePreambleParagraph();
            }
            
        }

        /// <summary>
        /// Use the Subsribe method from the ResolutionService if you want to get updates from other
        /// clients.
        /// </summary>
        /// <param name="resolution"></param>
        /// <param name="resolutionService"></param>
        /// <returns></returns>
        public static async Task<ResolutionViewModel> CreateViewModelOnline(Resolution resolution, MUNityClient.Services.IResolutionService resolutionService)
        {
            var instance = new ResolutionViewModel(resolution, true, resolutionService);
            await instance.HubConnection.StartAsync();
            return instance;
        }

        public static ResolutionViewModel CreateViewModelOffline(Resolution resolution, MUNityClient.Services.IResolutionService resolutionService)
        {
            var instance = new ResolutionViewModel(resolution, false, resolutionService);
            //await instance.HubConnection.StartAsync();
            return instance;
        }
    }
}
