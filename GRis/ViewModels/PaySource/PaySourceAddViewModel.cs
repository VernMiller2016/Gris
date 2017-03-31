using System.ComponentModel.DataAnnotations;

namespace GRis.ViewModels.PaySource
{
    public class PaySourceAddViewModel
    {
        [Required]
        [Display(Name = "PaySource Id")]
        public int VendorId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Is Active")]
        public bool Active { get; set; } = true;
    }
}