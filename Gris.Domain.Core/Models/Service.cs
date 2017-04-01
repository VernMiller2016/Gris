using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Gris.Domain.Core.Models
{
    public class Service:SoftDeleteEntity
    {
        [Required]
        [Key]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Service Id")]
        public int VendorId { get; set; }

        [Required]
        [Display(Name = "Service Description")]
        [StringLength(50)]
        public string ServiceDescription { get; set; }
    }
}