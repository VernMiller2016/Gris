using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GRis.Models
{
    public class PaySource
    {
        [Required]
        public int PaySourceId { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }

        [DefaultValue(true)]
        public bool Active { get; set; }

        public int? ProgramId { get; set; }

        public Program Program { get; set; }
    }
}