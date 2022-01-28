using System.ComponentModel.DataAnnotations;

namespace MUNity.VirtualCommittees.Dtos
{
    public class CreateSimulationResolutionRequest : SimulationRequest
    {
        [Required]
        public string Titel { get; set; }
    }
}
