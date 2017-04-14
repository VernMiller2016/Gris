using System;
using System.ComponentModel.DataAnnotations;

namespace GRis.ViewModels.ServerAvailableHourModels
{
    public class ServerAvailableHourDetailsViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Server Name")]
        public string ServerName { get; set; }

        [Display(Name = "Server Id")]
        public int ServerVendorId { get; set; }

        [Display(Name = "Date")]
        public DateTime DateRange { get; set; }

        [Display(Name = "Available Hours")]
        public float AvailableHours { get; set; }
    }
}