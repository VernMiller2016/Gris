using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gris.Domain.Core.Models
{
    public class PaySource : SoftDeleteEntity
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public int VendorId { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }

        public int? ProgramId { get; set; }

        public Program Program { get; set; }

        public virtual ICollection<ServerTimeEntry> ServerTimeEntries { get; set; }
    }
}