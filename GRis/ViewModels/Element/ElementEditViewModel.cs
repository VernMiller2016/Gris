using System.ComponentModel.DataAnnotations;

namespace GRis.ViewModels.Element
{
    public class ElementEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }

        [Required]
        [Display(Name = "Element ID")]
        public int VendorId { get; set; }
    }
}