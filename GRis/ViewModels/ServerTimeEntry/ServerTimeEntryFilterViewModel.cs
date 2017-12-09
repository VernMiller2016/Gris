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

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name ="Last Name")]
        public string SecondName { get; set; }

        [Display(Name = "Paysource Name")]
        public string PaysourceName { get; set; }

        public bool IsEmpty
        {
            get { return (!Date.HasValue && (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(SecondName)) && string.IsNullOrWhiteSpace(PaysourceName)); }
        }
    }
}