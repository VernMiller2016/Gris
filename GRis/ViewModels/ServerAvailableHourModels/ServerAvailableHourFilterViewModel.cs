using System;
using System.ComponentModel.DataAnnotations;

namespace GRis.ViewModels.ServerAvailableHourModels
{
    public class ServerAvailableHourFilterViewModel
    {
        [UIHint("YearMonthDatePicker")]
        [Display(Name = "Filter by Date")]
        [Required]
        public DateTime? Date { get; set; } = DateTime.Now;
        
        public string DateAsMonthYear
        {
            get { return Date.HasValue ? Date.Value.ToString("MM/yyyy") : ""; }
        }
    }
}