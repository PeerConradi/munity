using System.ComponentModel.DataAnnotations;

namespace MUNity.Schema.Simulation
{
    public class CreateSimulationResolutionRequest : SimulationRequest
    {
        [Required]
        public string Titel { get; set; }
    }
}
