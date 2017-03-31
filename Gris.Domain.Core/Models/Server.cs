using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Gris.Domain.Core.Models
{
    public class Server
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

        [DefaultValue(true)]
        public bool Active { get; set; }

        public int? CategoryId { get; set; }

        public Category Category { get; set; }

        public string FullName => LastName + ", " + FirstName;

        public virtual ICollection<ServerTimeEntry> ServerTimeEntries { get; set; }
    }
}