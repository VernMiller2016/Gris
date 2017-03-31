using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Gris.Domain.Core.Models
{
    /// <summary>
    /// States, Medicaids, all other-contracts, ...
    /// </summary>
    public class Program
    {
        [Required]
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }

        [StringLength(9)]
        public string GpProject { get; set; }

        [DefaultValue(true)]
        public bool Active { get; set; }

        public virtual ICollection<PaySource> PaySources { get; set; }
    }
}