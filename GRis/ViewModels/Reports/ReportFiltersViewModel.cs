using System;
using System.ComponentModel.DataAnnotations;

namespace GRis.ViewModels.Reports
{
    public class ReportFiltersViewModel
    {
        [Required(ErrorMessage = "Date is required.")]
        [UIHint("YearMonthDatePicker")]
        [Display(Name = "Please select date")]
        public DateTime? SelectedDate { get; set; }
    }
}