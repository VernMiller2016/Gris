using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GRis.ViewModels.Server
{
    public class ServerAddViewModel
    {
        [Required]
        [Display(Name = "Server Id")]
        public int VendorId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(15)]
        public string GpEmpNumber { get; set; }

        [Required]
        public int Element { get; set; }

        [Display(Name = "Is Active")]
        public bool Active { get; set; } = true;

        public int? CategoryId { get; set; }

        [Display(Name = "Available Categories")]
        public List<SelectListItem> AvailableCategories { get; set; }
    }
}