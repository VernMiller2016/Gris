using System.ComponentModel.DataAnnotations;

namespace GRis.ViewModels.Server
{
    public class ServerEditViewModel
    {
        public int Id { get; set; }

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

        [Display(Name = "Is Active")]
        public bool Active { get; set; }
    }
}