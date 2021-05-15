using Microsoft.AspNetCore.SignalR;
using MUNity.Models.Resolution;
using MUNity.Models.Resolution.EventArguments;
using MUNity.Schema.Resolution;
using MUNityCore.Extensions.CastExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Hubs
{

    /// <summary>
    /// The ResolutionHub handles every client request that comes via the socket: "resasocket"
    /// Note that for most functions of creating, editing or deleting a resolution the 
    /// ResolutionController is used, to make sure the user is authenticated to use this socket.
    /// </summary>
    public class ResolutionHub : Hub<MUNity.Hubs.ITypedResolutionHub>
    {

        private readonly Services.SqlResolutionService _resolutionService;

        public ResolutionHub(Services.SqlResolutionService resolutionService)
        {
            _resolutionService = resolutionService;
        }

        public async Task SetCommitteeName(HeaderStringPropChangedEventArgs body)
        {
            var success = await _resolutionService.SetCommitteeNameAsync(body.ResolutionId, body.Text);
            if (success)
                await Clients.Group(body.ResolutionId).HeaderCommitteeNameChanged(body);
        }

        public async Task SetSupporterNames(HeaderStringPropChangedEventArgs body)
        {
            var success = await _resolutionService.SetSupportersAsync(body.ResolutionId, body.Text);
            if (success)
                await Clients.Group(body.ResolutionId).HeaderSupportersChanged(body);
        }

        public async Task SetFullName(HeaderStringPropChangedEventArgs body)
        {
            var success = await _resolutionService.SetFullNameAsync(body.ResolutionId, body.Text);
            if (success)
                await Clients.Group(body.ResolutionId).HeaderFullNameChanged(body);
        }

        public async Task SetName(HeaderStringPropChangedEventArgs body)
        {
            var success = await _resolutionService.SetNameAsync(body.ResolutionId, body.Text);
            if (success)
                await Clients.Group(body.ResolutionId).HeaderNameChanged(body);
        }

        public async Task SetSubmitterName(HeaderStringPropChangedEventArgs body)
        {
            var success = await _resolutionService.SetSubmitterNameAsync(body.ResolutionId, body.Text);
            if (success)
                await Clients.Group(body.ResolutionId).HeaderSubmitterNameChanged(body);
        }

        public async Task SetTopic(HeaderStringPropChangedEventArgs body)
        {
            var success = await _resolutionService.SetTopicAsync(body.ResolutionId, body.Text);
            if (success)
                await Clients.Group(body.ResolutionId).HeaderTopicChanged(body);
        }

        public async Task SetSession(HeaderStringPropChangedEventArgs body)
        {
            var success = await _resolutionService.SetSessionAsync(body.ResolutionId, body.Text);
            if (success)
                await Clients.Group(body.ResolutionId).HeaderSessionChanged(body);
        }

        public async Task SetAgendaItem(HeaderStringPropChangedEventArgs body)
        {
            var success = await _resolutionService.SetAgendaItem(body.ResolutionId, body.Text);
            if (success)
                await Clients.Group(body.ResolutionId).HeaderAgendaItemChanged(body);
        }

        public async Task AddPreambleParagraph(AddPreambleParagraphRequest body)
        {
            var createdParagraph = _resolutionService.CreatePreambleParagraph(body.ResolutionId);
            if (createdParagraph != null)
            {
                var socketParam = new PreambleParagraphAddedEventArgs(body.ResolutionId, createdParagraph.ToModel());
                await Clients.Group(body.ResolutionId).PreambleParagraphAdded(socketParam);
            }
        }

        public async Task AddOperativeParagraph(AddOperativeParagraphRequest body)
        {
            var createdParagraph = _resolutionService.CreateOperativeParagraph(body.ResolutionId);
            if (createdParagraph != null)
            {
                var socketParam = new OperativeParagraphAddedEventArgs(body.ResolutionId, createdParagraph.ToModel());
                await Clients.Group(body.ResolutionId).OperativeParagraphAdded(socketParam);
            }
        }

        public async Task SetPreambleParagraphText(ChangePreambleParagraphTextRequest body)
        {
            var changed = _resolutionService.SetPreambleParagraphText(body.PreambleParagraphId, body.NewText);
            if (changed)
            {
                var socketParam = new PreambleParagraphTextChangedEventArgs(body.ResolutionId, body.PreambleParagraphId, body.NewText);
                await Clients.Group(body.ResolutionId).PreambleParagraphTextChanged(socketParam);
            }
        }

        public async Task SetOperativeParagraphText(ChangeOperativeParagraphTextRequest body)
        {
            var changed = _resolutionService.SetOperativeParagraphText(body.OperativeParagraphId, body.NewText);
            if (changed)
            {
                var socketParam = new OperativeParagraphTextChangedEventArgs(body.ResolutionId, body.OperativeParagraphId, body.NewText);
                await Clients.Group(body.ResolutionId).OperativeParagraphTextChanged(socketParam);
            }
        }

        public async Task SetPreambleParagraphComment(ChangePreambleParagraphTextRequest body)
        {
            var changed = _resolutionService.SetPreambleParagraphComment(body.PreambleParagraphId, body.NewText);
            if (changed)
            {
                var socketParam = new PreambleParagraphCommentTextChangedEventArgs(body.ResolutionId, body.PreambleParagraphId, "", body.NewText);
                await Clients.Group(body.ResolutionId).PreambleParagraphCommentTextChanged(socketParam);
            }
        }

        public async Task DeletePreambleParagraph(RemovePreambleParagraphRequest body)
        {
            var deleted = _resolutionService.RemovePreambleParagraph(body.PreambleParagraphId);
            if (deleted)
            {
                var socketParam = new PreambleParagraphRemovedEventArgs(body.ResolutionId, body.PreambleParagraphId);
                await Clients.Group(body.ResolutionId).PreambleParagraphRemoved(socketParam);
            }
        }

        public async Task DeleteOperativeParagraph(RemoveOperativeParagraphRequest body)
        {
            var deleted = _resolutionService.RemoveOperativeParagraph(body.OperativeParagraphId);
            if (deleted)
            {
                var socketParam = new OperativeParagraphRemovedEventArgs(body.ResolutionId, body.OperativeParagraphId);
                await Clients.Group(body.ResolutionId).OperativeParagraphRemoved(socketParam);
            }
            
        }

        public async Task ReorderPreambleParagraphs(ReorderPreambleRequest body)
        {
            var reordered = _resolutionService.ReorderPreamble(body.ResolutionId, body.NewOrder);
            if (reordered)
            {
                
                // TODO: Reoder into the socket
            }
        }

        public async Task CreateAddAmendment(CreateAddAmendmentRequest body)
        {
            var mdl = this._resolutionService.CreateAddAmendment(body.ResolutionId, body.Index, body.SubmitterName, body.Text);
            if (mdl == null)
                return;

            var args = mdl.ToSocketModel(body.ResolutionId);
            await Clients.Group(body.ResolutionId).AddAmendmentCreated(args);
        }

        public async Task CreateChangeAmendment(CreateChangeAmendmentRequest body)
        {
            var mdl = this._resolutionService.CreateChangeAmendment(body.ParagraphId, body.SubmitterName, body.NewText);
            if (mdl != null)
            {
                var args = mdl.ToModel();
                await Clients.Group(body.ResolutionId).ChangeAmendmentCreated(args);
            }
        }

        public async Task CreateDeleteAmendment(CreateDeleteAmendmentRequest body)
        {
            var mdl = this._resolutionService.CreateDeleteAmendment(body.ParagraphId, body.SubmitterName);
            if (mdl != null)
            {
                var args = mdl.ToModel();
                await Clients.Group(body.ResolutionId).DeleteAmendmentCreated(args);
            }
        }

        public async Task CreateMoveAmendment(CreateMoveAmendmentRequest body)
        {
            var mdl = this._resolutionService.CreateMoveAmendment(body.ParagraphId, body.SubmitterName, body.NewIndex);
            if (mdl != null)
            {
                var args = mdl.ToSocketModel(body.ResolutionId);
                await Clients.Group(body.ResolutionId).MoveAmendmentCreated(args);
            }
        }

        public async Task ActivateAmendment(ActivateAmendmentRequest body)
        {
            var success = this._resolutionService.ActivateAmendment(body.AmendmentId);
            if (success)
            {
                var args = new AmendmentActivatedChangedEventArgs()
                {
                    ResolutionId = body.ResolutionId,
                    Activated = true,
                    AmendmentId = body.AmendmentId
                };
                await this.Clients.Group(body.ResolutionId).AmendmentActivatedChanged(args);
            }
        }

        public async Task DeactivateAmendment(ActivateAmendmentRequest body)
        {
            var success = this._resolutionService.DeactivateAmendment(body.AmendmentId);
            if (success)
            {
                var args = new AmendmentActivatedChangedEventArgs()
                {
                    ResolutionId = body.ResolutionId,
                    Activated = false,
                    AmendmentId = body.AmendmentId
                };
                await this.Clients.Group(body.ResolutionId).AmendmentActivatedChanged(args);
            }
        }

        public async Task SubmitAddAmendment(AmendmentRequest body)
        {
            var success = this._resolutionService.SubmitAddAmendment(body.AmendmentId);
            if (success)
                await this.Clients.Group(body.ResolutionId).AmendmentSubmitted(body.AmendmentId);
            
        }

        public async Task SubmitChangeAmendment(AmendmentRequest body)
        {
            var success = this._resolutionService.SubmitChangeAmendment(body.AmendmentId);
            if (success)
                await this.Clients.Group(body.ResolutionId).AmendmentSubmitted(body.AmendmentId);
        }

        public async Task SubmitDeleteAmendment(AmendmentRequest body)
        {
            var success = this._resolutionService.SubmitDeleteAmendment(body.AmendmentId);
            if (success)
                await this.Clients.Group(body.ResolutionId).AmendmentSubmitted(body.AmendmentId);
        }

        public async Task SubmitMoveAmendment(AmendmentRequest body)
        {
            var success = this._resolutionService.SubmitMoveAmendment(body.AmendmentId);
            if (success)
                await this.Clients.Group(body.ResolutionId).AmendmentSubmitted(body.AmendmentId);
        }

        public async Task RemoveAddAmendment(AmendmentRequest body)
        {
            var success = this._resolutionService.RemoveAddAmendment(body.AmendmentId);
            if (success)
                await this.Clients.Group(body.ResolutionId).AmendmentRemoved(body.AmendmentId);
        }

        public async Task RemoveChangeAmendment(AmendmentRequest body)
        {
            var success = this._resolutionService.RemoveChangeAmendment(body.AmendmentId);
            if (success)
                await this.Clients.Group(body.ResolutionId).AmendmentRemoved(body.AmendmentId);
        }

        public async Task RemoveDeleteAmendment(AmendmentRequest body)
        {
            var success = this._resolutionService.RemoveDeleteAmendment(body.AmendmentId);
            if (success)
                await this.Clients.Group(body.ResolutionId).AmendmentRemoved(body.AmendmentId);
        }

        public async Task RemoveMoveAmendment(AmendmentRequest body)
        {
            var success = this._resolutionService.RemoveMoveAmendment(body.AmendmentId);
            if (success)
                await this.Clients.Group(body.ResolutionId).AmendmentRemoved(body.AmendmentId);
        }

        public async Task ChangePublicMode(string resolutionId, bool allowPublicEdit)
        {
            var args = new PublicModeChangedEventArgs()
            {
                AllowPublicEdit = allowPublicEdit,
                ResolutionId = resolutionId
            };

            var success = allowPublicEdit ? 
                this._resolutionService.EnablePublicEdit(resolutionId) : 
                this._resolutionService.DisablePublicEdit(resolutionId);

            if (success)
                await this.Clients.Group(resolutionId).PublicModeChanged(args);
            
        }
        
        public async Task Subscribe(string resolutionId)
        {
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, resolutionId);
        }

    }
}
