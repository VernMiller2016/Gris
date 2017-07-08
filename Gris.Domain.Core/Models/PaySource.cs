using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        //[NotMapped]
        //public int? ProgramId { get; set; }

        //[NotMapped]
        //public Program Program { get; set; }

        public virtual ICollection<Program> Programs { get; set; }

        public virtual ICollection<ServerTimeEntry> ServerTimeEntries { get; set; }
    }
}