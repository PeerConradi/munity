using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using MUNityClient;
using MUNityClient.Shared;
using MUNity.Models.Resolution;
using MUNity.Extensions.ResolutionExtensions;
using MUNityClient.Extensions;
using MUNityClient.Shared.Resa;

namespace MUNityClient.Pages.Resa
{
    public partial class ResolutionEditor
    {
        [Parameter]
        public string Id
        {
            get;
            set;
        }

        private enum SyncModes
        {
            PingingServer,
            Offline,
            OnlineButNotSyncing,
            Syncing,
            Updated,
            ErrorWhenSyncing
        }

        private MUNityClient.Shared.Bootstrap.Modal AddAmendmentModal
        {
            get;
            set;
        }

        private MUNityClient.Shared.Bootstrap.Modal DeletePreambleParagraphModal
        {
            get;
            set;
        }

        private MUNityClient.Shared.Bootstrap.Modal DeleteOperativeParagraphModal
        {
            get;
            set;
        }

        private MUNityClient.Shared.Resa.NewAmendmentForm NewAmendmentForm
        {
            get;
            set;
        }

        public Resolution Resolution
        {
            get;
            set;
        }

        private PreambleParagraph PreambleMarkedForDeletion
        {
            get;
            set;
        }

        private OperativeParagraph OperativeMarkedForDeletion
        {
            get;
            set;
        }

        private Boolean fetchingResolutionErrored = false;
        private System.Timers.Timer _updateTimer = new System.Timers.Timer(3000);
        private List<PreambleParagraph> _preambleParagraphsToUpdate = new List<PreambleParagraph>();
        private List<OperativeParagraph> _operativeParagraphsToUpdate = new List<OperativeParagraph>();
        private bool fullUpdate = false;
        private Services.SocketHandlers.ResaSocketHandler _socketHandler;
        private SyncModes SyncMode = SyncModes.PingingServer;
        protected override async Task OnInitializedAsync()
        {
            // Get the resolution from any source (local or online)
            this.Resolution = await this.resolutionService.GetResolution(Id);
            _updateTimer.Elapsed += UpdateTimerElapsed;
            if (Resolution != null)
            {
                AppendTracker(this.Resolution);
                CheckResolutionOnline();
            }
        }

        private void CheckResolutionOnline()
        {
            // Das ganze kann im Hintergrund stattfinden und solange kann
            // Der Benutzer schon einmal arbeiten, denn die Resolution ist ja auf die eine
            // oder anderer Weise rein gekommen (offline oder online)
            _ = this.resolutionService.IsOnline().ContinueWith(async (result) =>
            {
                if (result.Result == true)
                {
                    var canEdit = await this.resolutionService.CanEditResolution(this.Resolution.ResolutionId);
                    if (!canEdit)
                    {
                        this.SyncMode = SyncModes.OnlineButNotSyncing;
                    }
                    else
                    {
                        // Reload Resolution because the one in the local storage may be outdated!
                        this.Resolution = await resolutionService.GetResolutionFromServer(Id);
                        this.SyncMode = SyncModes.Updated;
                        this._socketHandler = await this.resolutionService.Subscribe(this.Resolution);
                    }
                }
                else
                {
                    this.SyncMode = SyncModes.Offline;
                }

                this.StateHasChanged();
            });
        }

        private void AppendTracker(Resolution resolution)
        {
            var tracker = new MUNity.ServiceWorkers.ResolutionWorker(resolution);
            tracker.ResolutionChanged += ResolutionHasChanged;
        }

        private void ResolutionHasChanged(Resolution sender)
        {
            SaveResolution();
        }

        private async void UpdateTimerElapsed(object timer, System.Timers.ElapsedEventArgs args)
        {
            SyncMode = SyncModes.Syncing;
            this.StateHasChanged();
            bool somethingErrored = false;
            if (!fullUpdate)
            {
                // Update Preamble
                if (_preambleParagraphsToUpdate.Any())
                {
                    _preambleParagraphsToUpdate.ForEach(async n =>
                    {
                        var tan = this.resolutionService.GenerateTan();
                        _socketHandler.IgnoreTransactions.Add(tan);
                        var result = await this.resolutionService.UpdateResolutionPreambleParagraph(this.Resolution.ResolutionId, n, tan);
                        if (!result.IsSuccessStatusCode)
                            somethingErrored = true;
                    });
                }

                // Update Operative Section
                if (_operativeParagraphsToUpdate.Any())
                {
                    _operativeParagraphsToUpdate.ForEach(async n =>
                    {
                        var tan = this.resolutionService.GenerateTan();
                        _socketHandler.IgnoreTransactions.Add(tan);
                        var result = await this.resolutionService.UpdateResolutionOperativeParagraph(this.Resolution.ResolutionId, n, tan);
                        if (!result.IsSuccessStatusCode)
                            somethingErrored = true;
                    });
                }
            }
            else
            {
                var result = await this.resolutionService.SyncResolutionWithServer(this.Resolution);
                if (!result)
                    somethingErrored = true;
            }

            this._operativeParagraphsToUpdate.Clear();
            this._preambleParagraphsToUpdate.Clear();
            fullUpdate = false;
            if (somethingErrored)
                this.SyncMode = SyncModes.ErrorWhenSyncing;
            else
                this.SyncMode = SyncModes.Updated;
            this.StateHasChanged();
            this._updateTimer.Stop();
        }

        private void QueueSyncing(PreambleParagraph paragraph)
        {
            if (!this._preambleParagraphsToUpdate.Contains(paragraph))
                this._preambleParagraphsToUpdate.Add(paragraph);
            if (!this._updateTimer.Enabled)
                this._updateTimer.Start();
        }

        private void QueueSyncing(OperativeParagraph paragraph)
        {
            if (!this._operativeParagraphsToUpdate.Contains(paragraph))
                this._operativeParagraphsToUpdate.Add(paragraph);
            if (!this._updateTimer.Enabled)
                this._updateTimer.Start();
        }

        private void QueueSyncAll()
        {
            this.fullUpdate = true;
            if (!this._updateTimer.Enabled)
                this._updateTimer.Start();
        }

        private void PreambleTextChanged(PreambleParagraph sender, string oldText, string newText)
        {
            this.Resolution.Date = DateTime.Now;
            if (SyncMode == SyncModes.Syncing || SyncMode == SyncModes.Updated)
            {
                QueueSyncing(sender);
            }
            else if (SyncMode == SyncModes.Offline)
            {
                SaveResolution();
            }

            this.StateHasChanged();
        }

        private void OperativeTextChanged(OperativeParagraph sender, string oldText, string newText)
        {
            this.Resolution.Date = DateTime.Now;
            if (SyncMode == SyncModes.Syncing || SyncMode == SyncModes.Updated)
            {
                QueueSyncing(sender);
            }
            else if (SyncMode == SyncModes.Offline)
            {
                SaveResolution();
            }

            this.StateHasChanged();
        }

        private void OperativeNoticesChanged(OperativeParagraph sender)
        {
            this.Resolution.Date = DateTime.Now;
            if (SyncMode == SyncModes.Syncing || SyncMode == SyncModes.Updated)
            {
                QueueSyncing(sender);
            }
            else if (SyncMode == SyncModes.Offline)
            {
                SaveResolution();
            }

            this.StateHasChanged();
        }

        private void PreambleNoticesChanged(PreambleParagraph sender)
        {
            this.Resolution.Date = DateTime.Now;
            if (SyncMode == SyncModes.Syncing || SyncMode == SyncModes.Updated)
            {
                QueueSyncing(sender);
            }
            else if (SyncMode == SyncModes.Offline)
            {
                SaveResolution();
            }

            this.StateHasChanged();
        }

        private void SaveResolution()
        {
            if (SyncMode == SyncModes.Syncing || SyncMode == SyncModes.Updated)
            {
                QueueSyncAll();
            }
            else if (SyncMode == SyncModes.Offline)
            {
                this.resolutionService.SaveOfflineResolution(this.Resolution);
            }

            this.StateHasChanged();
        }

        public void removePreambleParagraph()
        {
            this.Resolution.Date = DateTime.Now;
            this.Resolution?.Preamble?.Paragraphs?.Remove(PreambleMarkedForDeletion);
            SaveResolution();
        }

        public void removeOperativeParagraph()
        {
            this.Resolution.Date = DateTime.Now;
            //OperativeMarkedForDeletion.NoticesChanged -= OperativeNoticesChanged;
            this.Resolution?.OperativeSection.RemoveOperativeParagraph(OperativeMarkedForDeletion);
            SaveResolution();
        }

        //Move a preamble section in the given direction. It's called by the move event from the preambleParagraph child
        //component
        public void MovePreambleParagraph(Boolean up, PreambleParagraph preambleParagraph)
        {
            this.Resolution.Date = DateTime.Now;
            int originalIndex = this.Resolution?.Preamble?.Paragraphs?.IndexOf(preambleParagraph) ?? -1;
            this.Resolution.Preamble.Paragraphs.Move(originalIndex, up ? MoveDirection.Up : MoveDirection.Down);
            SaveResolution();
        }

        //Move an operative section in the given direction. It's called by the move event from the operativeParagraph child
        //component
        public void MoveOperativeParagraph(Boolean up, OperativeParagraph preambleParagraph)
        {
            this.Resolution.Date = DateTime.Now;
            int originalIndex = this.Resolution?.OperativeSection?.Paragraphs?.IndexOf(preambleParagraph) ?? -1;
            this.Resolution.OperativeSection.Paragraphs.Move(originalIndex, up ? MoveDirection.Up : MoveDirection.Down);
            SaveResolution();
        }

        public void AmendmentInteracted()
        {
            this.StateHasChanged();
            SaveResolution();
        }

        private void AddPreambleParagraph()
        {
            this.Resolution.Date = DateTime.Now;
            var paragraph = this.Resolution.CreatePreambleParagraph();
            SaveResolution();
        }

        private void AddOperativeParagraph()
        {
            this.Resolution.Date = DateTime.Now;
            var paragraph = this.Resolution.OperativeSection.CreateOperativeParagraph();
            //paragraph.NoticesChanged += OperativeNoticesChanged;
            SaveResolution();
        }

        private void ShowNewAmendmentModal()
        {
            this.AddAmendmentModal.Open();
        }

        private void NewAmendment()
        {
            var amendment = this.NewAmendmentForm.GetAmendment();
            if (amendment == null)
            {
            // TODO: Meldung zeigen Resolution konnte nicht erstellt werden!
            }

            this.AddAmendmentModal.Close();
            SaveResolution();
        }

        /// <summary>
        /// List of amendments in visible order. This is the order of all the amendments
        /// first of by the paragraph they address from top to bottom.
        /// Then Amendments to delete are shown first, after that amendments to change the paragraph, after that
        /// amendments to move the paragraph.
        ///
        /// Amendments to add a new paragraph are shown last!
        /// </summary>
        private IEnumerable<AbstractAmendment> OrderedAmendments
        {
            get
            {
                var list = new List<AbstractAmendment>();
                foreach (var paragraph in this.Resolution.OperativeSection.Paragraphs)
                {
                    var deleteAmendments = this.Resolution.OperativeSection.DeleteAmendments.Where(n => n.TargetSectionId == paragraph.OperativeParagraphId);
                    if (deleteAmendments.Any())
                        list.AddRange(deleteAmendments);
                    var changeAmendments = this.Resolution.OperativeSection.ChangeAmendments.Where(n => n.TargetSectionId == paragraph.OperativeParagraphId);
                    if (changeAmendments.Any())
                        list.AddRange(changeAmendments);
                    var moveAmendments = this.Resolution.OperativeSection.MoveAmendments.Where(n => n.TargetSectionId == paragraph.OperativeParagraphId);
                    if (moveAmendments.Any())
                        list.AddRange(moveAmendments);
                }

                list.AddRange(this.Resolution.OperativeSection.AddAmendments);
                return list;
            }
        }

        private EventCallback RepairResolution()
        {
            MUNity.Troubleshooting.Resa.ResolutionTroubleshooting.FixResolution(this.Resolution);
            SaveResolution();
            return EventCallback.Empty;
        }
    }
}