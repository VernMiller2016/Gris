using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gris.Domain.Core.Models
{
    public class PaySource
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
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