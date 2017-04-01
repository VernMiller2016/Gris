using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gris.Domain.Core.Models
{
    public class Unit : SoftDeleteEntity
    {
        [Required]
        public int UnitId { get; set; }

        [Display(Name = "Description")]
        [StringLength(50)]
        public string UnitDescription { get; set; }

        public ICollection<SubUnit> SubUnits { get; set; }
    }
}