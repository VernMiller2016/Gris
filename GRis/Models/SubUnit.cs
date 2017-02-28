using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GRis.Models
{
    public class SubUnit
    {
        [Required]
        public int SubUnitId { get; set; }

        [Required]
        [Display(Name = "SubUnit Code")]
        [StringLength(10)]
        public string SubUnitCode { get; set; }

        [Display(Name = "Description")]
        [StringLength(50)]
        public string SubUnitDescription { get; set; }

        [DefaultValue(true)]
        public bool Active { get; set; }

        public Unit Unit { get; set; }
    }
}