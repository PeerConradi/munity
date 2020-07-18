namespace MUNityAngular.Models.Resolution.V2
{
    public interface IChangeAmendment : IAmendment
    {
        public string NewText { get; set; }
    }
}