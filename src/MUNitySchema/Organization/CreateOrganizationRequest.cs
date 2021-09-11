using System.ComponentModel.DataAnnotations;

namespace MUNity.Schema.Organization
{
    public class CreateOrganizationRequest
    {
        [Required]
        [MinLength(2)]
        public string ShortName { get; set; }

        [Required]
        [MinLength(3)]
        public string Name { get; set; }
    }
}
