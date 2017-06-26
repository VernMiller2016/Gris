using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gris.Domain.Core.Models
{
    public class Element
    {
        public int Id { get; set; }

        [Required]
        public string DisplayName { get; set; }

        [Required]
        public int VendorId { get; set; }

        public virtual ICollection<Server> Servers { get; set; }
    }
}