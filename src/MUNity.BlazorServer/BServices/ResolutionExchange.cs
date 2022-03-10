using MUNity.Database.Context;
using MUNity.Database.Models.Resolution;
using MUNity.Services;
using MUNity.Base;

namespace MUNity.BlazorServer.BServices
{
    public class ResolutionExchange
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        public event EventHandler<ResaElement> ResolutionChanged;

        public event EventHandler<ResaOperativeParagraph> OperativeParagraphChanged;

        public event EventHandler<ResaPreambleParagraph> PreambleParagraphChanged;

        public event EventHandler<ResaChangeAmendment> ChangeAmendmentChanged;

        public event EventHandler<ResaDeleteAmendment> DeleteAmendmentChanged;

        public event EventHandler<ResaMoveAmendment> MoveAmendmentChanged;

        public event EventHandler<ResaAddAmendment> AddAmendmentChanged;

        public ResaElement Resolution { get; set; }

        public void AddPreambleParagraph()
        {
            using var scope = serviceScopeFactory.CreateScope();
            using var resolutionService = scope.ServiceProvider.GetRequiredService<ResolutionService>();
            var paragraph = resolutionService.CreatePreambleParagraph(Resolution);
            this.ResolutionChanged?.Invoke(this, Resolution);
        }

        public void AddOperativeParagraph()
        {
            using var scope = serviceScopeFactory.CreateScope();
            using var resolutionService = scope.ServiceProvider.GetRequiredService<ResolutionService>();
            var paragraph = resolutionService.CreateOperativeParagraph(Resolution);
            this.ResolutionChanged?.Invoke(this, Resolution);
        }

        public void UpdatePreambleParagraph(ResaPreambleParagraph paragraph)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var resolutionService = scope.ServiceProvider.GetRequiredService<ResolutionService>();
            resolutionService.UpdatePreambleParagraph(paragraph);
            PreambleParagraphChanged?.Invoke(this, paragraph);
        }

        public void UpdateResaElement()
        {
            using var scope = serviceScopeFactory.CreateScope();
            var resolutionService = scope.ServiceProvider.GetRequiredService<ResolutionService>();
            resolutionService.UpdateResaElement(this.Resolution);
            ResolutionChanged?.Invoke(this, Resolution);
        }

        public void UpdateOperativeParagraph(ResaOperativeParagraph paragraph)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var resolutionService = scope.ServiceProvider.GetRequiredService<ResolutionService>();
            resolutionService.UpdateOperativeParagraph(paragraph);
            OperativeParagraphChanged?.Invoke(this, paragraph);
        }

        public void MovePreambleParagraphUp(ResaPreambleParagraph paragraph)
        {
            // Make update in database
            using var scope = serviceScopeFactory.CreateScope();
            var resolutionService = scope.ServiceProvider.GetRequiredService<ResolutionService>();
            resolutionService.MovePreambleParagraphUp(paragraph);

            // Make update in exchange
            var oldIndex = this.Resolution.PreambleParagraphs.IndexOf(paragraph);
            this.Resolution.PreambleParagraphs.Swap(oldIndex, oldIndex - 1);
            PreambleParagraphChanged?.Invoke(this, paragraph);
            ResolutionChanged?.Invoke(this, Resolution);
        }

        public void MoveOperativeParagraphUp(ResaOperativeParagraph paragraph)
        {
            // Make update in database
            using var scope = serviceScopeFactory.CreateScope();
            var resolutionService = scope.ServiceProvider.GetRequiredService<ResolutionService>();
            resolutionService.MoveOperativeParagraphUp(paragraph);

            // Make update in exchange
            if (paragraph.Parent == null)
            {
                var oldIndex = this.Resolution.OperativeParagraphs.IndexOf(paragraph);
                this.Resolution.OperativeParagraphs.Swap(oldIndex, oldIndex - 1);
                OperativeParagraphChanged?.Invoke(this, paragraph);
                ResolutionChanged?.Invoke(this, Resolution);
            }
            else
            {
                var oldIndex = paragraph.Parent.Children.IndexOf(paragraph);
                paragraph.Parent.Children.Swap(oldIndex, oldIndex - 1);
                OperativeParagraphChanged?.Invoke(this, paragraph);
                ResolutionChanged?.Invoke(this, Resolution);
            }
        }

        public void MovePreambleParagraphDown(ResaPreambleParagraph paragraph)
        {
            // Make update in database
            using var scope = serviceScopeFactory.CreateScope();
            var resolutionService = scope.ServiceProvider.GetRequiredService<ResolutionService>();
            resolutionService.MovePreambleParagraphDown(paragraph);   

            // make update in exchange
            var oldIndex = this.Resolution.PreambleParagraphs.IndexOf(paragraph);
            this.Resolution.PreambleParagraphs.Swap(oldIndex, oldIndex + 1);
            PreambleParagraphChanged?.Invoke(this, paragraph);
            ResolutionChanged?.Invoke(this, Resolution);
        }

        public void MoveOperativeParagraphDown(ResaOperativeParagraph paragraph)
        {
            // Make update in database
            using var scope = serviceScopeFactory.CreateScope();
            var resolutionService = scope.ServiceProvider.GetRequiredService<ResolutionService>();
            resolutionService.MoveOperativeParagraphDown(paragraph);

            // Make update in exchange
            if (paragraph.Parent == null)
            {
                var oldIndex = this.Resolution.OperativeParagraphs.IndexOf(paragraph);
                this.Resolution.OperativeParagraphs.Swap(oldIndex, oldIndex + 1);
                OperativeParagraphChanged?.Invoke(this, paragraph);
                ResolutionChanged?.Invoke(this, Resolution);
            }
            else
            {
                var oldIndex = paragraph.Parent.Children.IndexOf(paragraph);
                paragraph.Parent.Children.Swap(oldIndex, oldIndex + 1);
                OperativeParagraphChanged?.Invoke(this, paragraph);
                ResolutionChanged?.Invoke(this, Resolution);
            }
        }

        public void RemovePreambleParagraph(ResaPreambleParagraph paragraph)
        {
            // Make update in database
            using var scope = serviceScopeFactory.CreateScope();
            var resolutionService = scope.ServiceProvider.GetRequiredService<ResolutionService>();
            resolutionService.RemovePreambleParagraph(paragraph);

            this.Resolution.PreambleParagraphs.Remove(paragraph);
            PreambleParagraphChanged?.Invoke(this, paragraph);
            ResolutionChanged?.Invoke(this, Resolution);
        }

        public void RemoveOperativeParagraph(ResaOperativeParagraph paragraph)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var resolutionService = scope.ServiceProvider.GetRequiredService<ResolutionService>();
            resolutionService.RemoveOperativeParagraph(paragraph);

            ResolutionChanged?.Invoke(this, Resolution);
        }
        
        public void AddRemoveAmendment(ResaOperativeParagraph paragraph, int roleId)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var resolutionService = scope.ServiceProvider.GetRequiredService<ResolutionService>();
            resolutionService.AddRemoveAmendment(paragraph, roleId);

            OperativeParagraphChanged?.Invoke(this, paragraph);
        }

        public void AddChangeAmendment(ResaOperativeParagraph paragraph, int roleId, string newText)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var resolutionService = scope.ServiceProvider.GetRequiredService<ResolutionService>();
            resolutionService.AddChangeAmendment(paragraph, roleId, newText);

            OperativeParagraphChanged?.Invoke(this, paragraph);
        }

        public void AddMoveAmendment(ResaOperativeParagraph paragraph, int roleId, int newIndex)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var resolutionService = scope.ServiceProvider.GetRequiredService<ResolutionService>();
            resolutionService.AddMoveAmendment(paragraph, roleId, newIndex);

            OperativeParagraphChanged?.Invoke(this, paragraph);
            ResolutionChanged?.Invoke(this, Resolution);
        }

        public void AddAddAmendment(int roleId, string newText)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var resolutionService = scope.ServiceProvider.GetRequiredService<ResolutionService>();
            resolutionService.AddAddAmendment(this.Resolution, roleId, newText);

            ResolutionChanged?.Invoke(this, this.Resolution);
        }

        public void RevokeDeleteAmendment(ResaDeleteAmendment amendment)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var resolutionService = scope.ServiceProvider.GetRequiredService<ResolutionService>();
            resolutionService.RevokeDeleteAmendment(amendment);

            OperativeParagraphChanged?.Invoke(this, amendment.TargetParagraph);
        }

        public void RevokeChangeAmendment(ResaChangeAmendment amendment)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var resolutionService = scope.ServiceProvider.GetRequiredService<ResolutionService>();
            resolutionService.RevokeChangeAmendment(amendment);

            OperativeParagraphChanged?.Invoke(this, amendment.TargetParagraph);
        }

        public void SupportAmendment(ResaAmendment amendment, int roleId)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var resolutionService = scope.ServiceProvider.GetRequiredService<ResolutionService>();
            resolutionService.SupportAmendment(amendment, roleId);

            NotifyOnAmendentChange(amendment);
        }

        public void RevokeSupport(ResaAmendment amendment, int roleId)
        {
            var scope = serviceScopeFactory.CreateScope();
            var resolutionService = scope.ServiceProvider.GetRequiredService<ResolutionService>();
            resolutionService.RevokeSupport(amendment, roleId);

            NotifyOnAmendentChange(amendment);
        }

        public void SupportResolution(ResaElement resolution, int roleId)
        {
            var scope = serviceScopeFactory.CreateScope();
            var resolutionService = scope.ServiceProvider.GetRequiredService<ResolutionService>();
            resolutionService.SupportResolution(resolution, roleId);

            ResolutionChanged?.Invoke(this, resolution);
        }

        public void RevokeSupport(ResaElement resolution, int roleId)
        {
            var scope = serviceScopeFactory.CreateScope();
            var resolutionService = scope.ServiceProvider.GetRequiredService<ResolutionService>();
            resolutionService.RevokeSupport(resolution, roleId);

            ResolutionChanged?.Invoke(this, resolution);
        }

        public void SubmitAmendment(ResaAmendment amendment)
        {
            if (amendment is ResaDeleteAmendment deleteAmendment)
            {
                var resolution = amendment.Resolution;

                var scope = serviceScopeFactory.CreateScope();
                var resolutionService = scope.ServiceProvider.GetRequiredService<ResolutionService>();
                resolutionService.SubmitDeleteAmendment(deleteAmendment);

                ResolutionChanged?.Invoke(this, resolution);
            }
            else if (amendment is ResaChangeAmendment changeAmendment)
            {
                var scope = serviceScopeFactory.CreateScope();
                var resolutionService = scope.ServiceProvider.GetRequiredService<ResolutionService>();
                resolutionService.SubmitChangeAmendment(changeAmendment);
                OperativeParagraphChanged?.Invoke(this, changeAmendment.TargetParagraph);
            }
            else if (amendment is ResaMoveAmendment moveAmendment)
            {
                var scope = serviceScopeFactory.CreateScope();
                var resolutionService = scope.ServiceProvider.GetRequiredService<ResolutionService>();
                resolutionService.SubmitMoveAmendment(moveAmendment);
                ResolutionChanged?.Invoke(this, Resolution);
            }
            else if (amendment is ResaAddAmendment addAmendment)
            {
                var scope = serviceScopeFactory.CreateScope();
                var resolutionService = scope.ServiceProvider.GetRequiredService<ResolutionService>();
                resolutionService.SubmitAddAmendment(addAmendment);
                ResolutionChanged?.Invoke(this, Resolution);
            }
        }

        public void ActivateAmendment(ResaAmendment amendment, bool active = true)
        {
            var scope = serviceScopeFactory.CreateScope();
            var resolutionService = scope.ServiceProvider.GetRequiredService<ResolutionService>();
            resolutionService.ActivateAmendment(amendment, active);
            if (amendment is ResaDeleteAmendment delAmendment)
            {
                OperativeParagraphChanged?.Invoke(this, delAmendment.TargetParagraph);
            }
            else if (amendment is ResaChangeAmendment changeAmendment)
            {
                OperativeParagraphChanged?.Invoke(this, changeAmendment.TargetParagraph);
            }
            else if (amendment is ResaMoveAmendment moveAmendment)
            {
                OperativeParagraphChanged?.Invoke(this, moveAmendment.SourceParagraph);
                OperativeParagraphChanged?.Invoke(this, moveAmendment.VirtualParagraph);
                ResolutionChanged?.Invoke(this, Resolution);
            }
            else if (amendment is ResaAddAmendment addAmendment)
            {
                ResolutionChanged?.Invoke(this, Resolution);
                OperativeParagraphChanged?.Invoke(this, addAmendment.VirtualParagraph);
            }
        }

        private void NotifyOnAmendentChange(ResaAmendment amendment)
        {
            if (amendment is ResaDeleteAmendment delete)
            {
                OperativeParagraphChanged?.Invoke(this, delete.TargetParagraph);
            }
            else if (amendment is ResaChangeAmendment change)
            {
                OperativeParagraphChanged?.Invoke(this, change.TargetParagraph);
            }
            else if (amendment is ResaMoveAmendment move)
            {
                OperativeParagraphChanged?.Invoke(this, move.SourceParagraph);
            }
        }

        public ResolutionExchange(IServiceScopeFactory scopeFactory)
        {
            serviceScopeFactory = scopeFactory;
        }
    }
}
