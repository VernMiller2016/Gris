using System;
using System.ComponentModel.DataAnnotations;

namespace GRis.ViewModels.ServerTimeEntry
{
    public class ServerTimeEntryFilterViewModel
    {
        [UIHint("YearMonthDatePicker")]
        [Display(Name = "Filter by Date")]
        public DateTime? Date { get; set; } = null;

        public string DateAsMonthYear
        {
            get { return Date.HasValue ? Date.Value.ToString("MM/yyyy") : ""; }
        }
    }
}