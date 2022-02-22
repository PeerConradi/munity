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

        public void UpdatePreambleParagraph(ResaPreambleParagraph paragraph)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var resolutionService = scope.ServiceProvider.GetRequiredService<ResolutionService>();
            resolutionService.UpdatePreambleParagraph(paragraph);
            PreambleParagraphChanged?.Invoke(this, paragraph);
        }

        public void MovePreambleParagraphUp(ResaPreambleParagraph paragraph)
        {
            // Make update in database
            using var scope = serviceScopeFactory.CreateScope();
            var resolutionService = scope.ServiceProvider.GetRequiredService<ResolutionService>();
            resolutionService.MovePreambleParagraphUp(paragraph);

            // Make update inexchange
            var oldIndex = this.Resolution.PreambleParagraphs.IndexOf(paragraph);
            this.Resolution.PreambleParagraphs.Swap(oldIndex, oldIndex - 1);
            PreambleParagraphChanged?.Invoke(this, paragraph);
            ResolutionChanged?.Invoke(this, Resolution);
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

        public ResolutionExchange(IServiceScopeFactory scopeFactory)
        {
            serviceScopeFactory = scopeFactory;
        }
    }
}
