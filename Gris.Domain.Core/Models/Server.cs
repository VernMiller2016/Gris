using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gris.Domain.Core.Models
{
    public class Server : SoftDeleteEntity
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public int VendorId { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        public int? CategoryId { get; set; }

        [StringLength(15)]
        public string GpEmpNumber { get; set; }

        [Required]
        public int Element { get; set; }

        public Category Category { get; set; }

        public string FullName => LastName + ", " + FirstName;

        public virtual ICollection<ServerTimeEntry> ServerTimeEntries { get; set; }
    }
}