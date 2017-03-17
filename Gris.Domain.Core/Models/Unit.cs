using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GRis.Models
{
    public class Unit
    {
        [Required]
        public int UnitId { get; set; }

        [Display(Name = "Description")]
        [StringLength(50)]
        public string UnitDescription { get; set; }

        [DefaultValue(true)]
        public bool Active { get; set; }

        public ICollection<SubUnit> SubUnits { get; set; }
    }
}