using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GRis.Models
{
    /// <summary>
    /// States, Medicaids, all other-contracts, ...
    /// </summary>
    public class Program
    {
        [Required]
        public int ProgramId { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }

        [StringLength(9)]
        public string GpProject { get; set; }

        public List<PaySource> PaySources { get; set; }

        [DefaultValue(true)]
        public bool Active { get; set; }
    }
}