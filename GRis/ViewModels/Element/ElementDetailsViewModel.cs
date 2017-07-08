using System.ComponentModel.DataAnnotations;

namespace GRis.ViewModels.Element
{
    public class ElementDetailsViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }

        [Display(Name = "Element ID")]
        public int VendorId { get; set; }
    }
}