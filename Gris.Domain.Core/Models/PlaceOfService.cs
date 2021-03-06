﻿using System.ComponentModel.DataAnnotations;

namespace Gris.Domain.Core.Models
{
    public class PlaceOfService : SoftDeleteEntity
    {
        [Required]
        [Key]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Place of Service")]
        public int VendorId { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }
    }
}