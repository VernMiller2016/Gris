using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Gris.Domain.Core.Models
{
    public class Service
    {
        [Required]
        [Key]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Service Id")]
        public int ServiceId { get; set; }

        [Required]
        [Display(Name = "Service Description")]
        [StringLength(50)]
        public string ServiceDescription { get; set; }

        [DefaultValue(true)]
        public bool Active { get; set; }
    }
}