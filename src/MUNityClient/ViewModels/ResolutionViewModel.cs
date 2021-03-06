using Microsoft.AspNetCore.SignalR.Client;
using MUNity.Hubs;
using MUNity.Models.Resolution;
using MUNity.Models.Resolution.EventArguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNity.Extensions.ResolutionExtensions;

namespace MUNityClient.ViewModels
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

        public ViewModelLogic.IResolutionHandler Handler { get; set; }

        private bool _isOnlineResolution = false;

        private MUNityClient.Services.IResolutionService _resolutionService;

        [Obsolete("Remove this HubConnection its inside the OnlineHandler!")]
        public HubConnection HubConnection { get; set; }

        public List<string> IgnoreTransactions { get; set; }

        public event EventHandler<ResolutionChangedArgs> ResolutionChanged;

        public event EventHandler HeaderChangedFromExtern;

        public event EventHandler PreambleChangedFromExtern;

        public event EventHandler OperativeSeciontChangedFromExtern;

        public event EventHandler<OperativeParagraphChangedEventArgs> OperativeParagraphChangedFromExtern;


        public event EventHandler SyncModeChanged;

        public void InvokeHeaderChangedFromExtern(object sender, EventArgs args)
        {
            this.HeaderChangedFromExtern?.Invoke(sender, args);
        }

        public void InvokePreambleChangedFromExtern(object sender, EventArgs args)
        {
            this.PreambleChangedFromExtern?.Invoke(sender, args);
        }

        public void InvokeOperativeSectionChangedFromExtern(object sender, EventArgs args)
        {
            this.OperativeSeciontChangedFromExtern?.Invoke(sender, args);
        }

        public void InvokeOperativeParagraphChangedFromExtern(object sender, OperativeParagraphChangedEventArgs args)
        {
            this.OperativeParagraphChangedFromExtern?.Invoke(sender, args);
        }

        public Resolution Resolution { get; set; }

        private ResolutionViewModel(Resolution resolution, bool isOnline, MUNityClient.Services.IResolutionService resolutionService)
        {
            _isOnlineResolution = isOnline;
            this._resolutionService = resolutionService;
            Resolution = resolution;
        }

        /// <summary>
        /// List of amendments in visible order. This is the order of all the amendments
        /// first of by the paragraph they address from top to bottom.
        /// Then Amendments to delete are shown first, after that amendments to change the paragraph, after that
        /// amendments to move the paragraph.
        ///
        /// Amendments to add a new paragraph are shown last!
        /// </summary>
        public IEnumerable<AbstractAmendment> OrderedAmendments
        {
            get
            {
                var list = new List<AbstractAmendment>();
                foreach (var paragraph in this.Resolution.OperativeSection.Paragraphs)
                {
                    var deleteAmendments = this.Resolution.OperativeSection.DeleteAmendments.Where(n => n.TargetSectionId ==
                    paragraph.OperativeParagraphId);
                    if (deleteAmendments.Any())
                        list.AddRange(deleteAmendments);

                    var changeAmendments = this.Resolution.OperativeSection.ChangeAmendments.Where(n => n.TargetSectionId ==
                    paragraph.OperativeParagraphId);
                    if (changeAmendments.Any())
                        list.AddRange(changeAmendments);

                    var moveAmendments = this.Resolution.OperativeSection.MoveAmendments.Where(n => n.TargetSectionId ==
                    paragraph.OperativeParagraphId);
                    if (moveAmendments.Any())
                        list.AddRange(moveAmendments);
                }

                list.AddRange(this.Resolution.OperativeSection.AddAmendments);
                return list;
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
            var handler = new ViewModelLogic.ResolutionOnlineHandler(resolution);
            await handler.HubConnection.StartAsync();
            instance.Handler = handler;
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
