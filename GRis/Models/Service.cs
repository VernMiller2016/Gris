using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GRis.Models
{
    public class Service
    {
        [Required]
        public int ServiceId { get; set; }

        [Required]
        [Display(Name = "Service Code")]
        [StringLength(15)]
        public string ServiceCode { get; set; }

        [Required]
        [Display(Name = "Service Description")]
        [StringLength(50)]
        public string ServiceDescription { get; set; }

        [DefaultValue(true)]
        public bool Active { get; set; }

    }
}