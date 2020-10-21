namespace MUNityCore.Models.Resolution.V2
{
    public interface IMoveAmendment : IAmendment
    {
        public string NewTargetSectionId { get; set; }

        public int Position { get; set; }
    }
}