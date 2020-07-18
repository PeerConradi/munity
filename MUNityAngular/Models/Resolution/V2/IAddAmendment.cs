namespace MUNityAngular.Models.Resolution.V2
{
    public interface IAddAmendment : IAmendment
    {
        public string ParentSectionId { get; set; }

        public int Position { get; set; }

        public string Text { get; set; }
    }
}