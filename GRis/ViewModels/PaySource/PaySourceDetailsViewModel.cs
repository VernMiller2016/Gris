using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace GRis.ViewModels.PaySource
{
    public class PaySourceDetailsViewModel
    {
        private string _description;
        public int Id { get; set; }

        [Required]
        [Display(Name = "PaySource Id")]
        public int VendorId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Description")]
        public string Description { get { return (new CultureInfo("en-US", false).TextInfo).ToTitleCase(_description.ToLower()); } set { _description = value; } }

        [Display(Name = "Is Active")]
        public bool Active { get; set; } = true;
    }
}