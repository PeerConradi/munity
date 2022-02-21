using MUNity.Database.Context;
using MUNity.Database.Models.Resolution;
using MUNity.Services;

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


        public ResolutionExchange(IServiceScopeFactory scopeFactory)
        {
            serviceScopeFactory = scopeFactory;
        }
    }
}
