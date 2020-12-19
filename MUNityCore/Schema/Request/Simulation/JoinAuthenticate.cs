using System.ComponentModel.DataAnnotations;

namespace MUNityCore.Schema.Request.Simulation
{
    public class JoinAuthenticate
    {
        [MaxLength(100)]
        public string Password { get; set; }

        [MaxLength(50)]
        [Required]
        public string DisplayName { get; set; }
    }
}