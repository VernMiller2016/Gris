using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Gris.Domain.Core.Models
{
    public class Server
    {
        [Required]
        [Key]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Server Id")]
        public int ServerId { get; set; }

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

        public int? CategoryId { get; set; }

        public Category Category { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => LastName + ", " + FirstName;
    }
}