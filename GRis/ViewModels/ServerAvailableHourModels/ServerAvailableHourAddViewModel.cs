using System;
using System.ComponentModel.DataAnnotations;

namespace GRis.ViewModels.ServerAvailableHourModels
{
    public class ServerAvailableHourAddViewModel
    {
        [Display(Name = "Server Id")]
        [Required]
        public int ServerVendorId { get; set; }

        [Display(Name = "Date")]
        [Required]
        public DateTime DateRange { get; set; }

        [Display(Name = "Available Hourse")]
        [Required]
        public float AvailableHours { get; set; }
    }
}