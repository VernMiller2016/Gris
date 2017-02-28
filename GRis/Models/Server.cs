using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GRis.Models
{
    public class Server
    {
        [Required]
        public int ServerId { get; set; }

        [Required]
        [Display(Name = "Server Id")]
        [StringLength(10)]
        public string ServerNumber { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50)]
        public string LastName { get; set; }

        [DefaultValue(true)]
        public bool Active { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => LastName + ", " + FirstName;
    }
}