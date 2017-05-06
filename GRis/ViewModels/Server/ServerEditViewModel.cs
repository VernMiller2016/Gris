﻿using Gris.Domain.Core.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GRis.ViewModels.Server
{
    public class ServerEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Server Id")]
        public int VendorId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50)]
        public string LastName { get; set; }

        public int? CategoryId { get; set; }

        [Display(Name = "Available Categories")]
        public List<SelectListItem> AvailableCategories { get; set; }

        [Display(Name = "Is Active")]
        public bool Active { get; set; }
    }
}