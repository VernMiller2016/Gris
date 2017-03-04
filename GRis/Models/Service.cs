using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GRis.Models
{
    public class Service
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
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