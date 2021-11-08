using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MUNity.Schema.Project
{
    public class CreateProjectModel
    {
        [Required]
        public string OrganizationId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Short { get; set; }
    }
}
