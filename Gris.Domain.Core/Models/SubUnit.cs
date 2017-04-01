using System.ComponentModel.DataAnnotations;

namespace Gris.Domain.Core.Models
{
    public class SubUnit : SoftDeleteEntity
    {
        [Required]
        public int SubUnitId { get; set; }

        [Display(Name = "Description")]
        [StringLength(50)]
        public string SubUnitDescription { get; set; }

        public Unit Unit { get; set; }
    }
}