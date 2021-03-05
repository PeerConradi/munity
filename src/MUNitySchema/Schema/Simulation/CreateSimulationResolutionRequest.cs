using System.ComponentModel.DataAnnotations;

namespace MUNity.Schema.Simulation
{
    public class CreateSimulationResolutionRequest : SimulationRequest
    {
        [Required]
        public string Titel { get; set; }
    }

    public class SimulationResolutionRequest : SimulationRequest
    {
        public string ResolutionId { get; set; }
    }
}
