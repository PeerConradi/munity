namespace MUNity.BlazorServer.BServices
{
    public class PresentationViewsService
    {
        private List<PresentationView> presentations = new List<PresentationView>();

        public PresentationView GetPresentation(string committeeId)
        {
            var presentation = presentations.FirstOrDefault(x => x.CommitteeId == committeeId);
            if (presentation == null)
            {
                presentation = new PresentationView()
                {
                    CommitteeId = committeeId,
                };
                presentations.Add(presentation);
            }
            return presentation;
        }
    }

    public class PresentationView
    {
        public event EventHandler<string> ResolutionIdChanged;

        public string CommitteeId { get; set; }

        private string resolutionId;
        public string ResolutionId
        {
            get => resolutionId;
            set
            {
                if (resolutionId != value)
                {
                    resolutionId = value;
                    ResolutionIdChanged?.Invoke(this, value);
                }
            }
        }
    }
}
