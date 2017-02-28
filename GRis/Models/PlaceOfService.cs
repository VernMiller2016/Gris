using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GRis.Models
{
    public class PlaceOfService
    {
        [Required]
        public int PlaceOfServiceId { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }

        [DefaultValue(true)]
        public bool Active { get; set; }
    }
}