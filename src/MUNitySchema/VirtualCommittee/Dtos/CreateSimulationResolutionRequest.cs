using System.ComponentModel.DataAnnotations;

namespace MUNity.VirtualCommittee.Dtos
{
    public class CreateSimulationResolutionRequest : SimulationRequest
    {
        [Required]
        public string Titel { get; set; }
    }
}
