using System;
using System.ComponentModel.DataAnnotations;

namespace GRis.ViewModels.ServerTimeEntry
{
    public class ServerTimeEntryFilterViewModel
    {
        [UIHint("YearMonthDatePicker")]
        [Display(Name = "Filter by Date")]
        public DateTime? SelectedDate { get; set; } = null;
    }
}