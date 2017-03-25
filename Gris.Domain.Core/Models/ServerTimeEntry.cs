using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gris.Domain.Core.Models
{
    public class ServerTimeEntry
    {
        [Key]
        public int Id { get; set; }

        public int ServerId { get; set; }

        public int PaySourceId { get; set; }

        public TimeSpan Duration { get; set; }

        [Display(Name = "Begin Date")]
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime BeginDate { get; set; }

        public Server Server { get; set; }

        public PaySource PaySource { get; set; }
    }
}