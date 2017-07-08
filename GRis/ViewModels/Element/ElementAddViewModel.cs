using System.ComponentModel.DataAnnotations;

namespace GRis.ViewModels.Element
{
    public class ElementAddViewModel
    {
        [Required]
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }

        [Required]
        [Display(Name = "Element ID")]
        public int VendorId { get; set; }
    }
}