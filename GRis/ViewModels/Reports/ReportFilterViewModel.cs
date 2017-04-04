using System;
using System.ComponentModel.DataAnnotations;

namespace GRis.ViewModels.Reports
{
    public class ReportFilterViewModel
    {
        [Required(ErrorMessage = "Date is required.")]
        [UIHint("YearMonthDatePicker")]
        [Display(Name = "Please select date")]
        public DateTime? Date { get; set; }

        public string DateAsMonthYear
        {
            get { return Date.HasValue ? Date.Value.ToString("MM/yyyy") : ""; }
        }
    }
}