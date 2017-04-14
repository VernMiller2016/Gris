using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gris.Domain.Core.Models
{
    public class ServerAvailableHour
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int ServerId { get; set; }

        [Required]
        [Index("DateRangeIndex", IsUnique = false)]
        public DateTime DateRange { get; set; }

        [Required]
        public float AvailableHours { get; set; }

        public Server Server { get; set; }
    }
}