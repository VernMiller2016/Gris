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

        [Display(Name = "Server Name")]
        public string ServerName { get; set; }

        [Display(Name = "Paysource Name")]
        public string PaysourceName { get; set; }

        public bool IsEmpty
        {
            get { return (!Date.HasValue && string.IsNullOrWhiteSpace(ServerName) && string.IsNullOrWhiteSpace(PaysourceName)); }
        }
    }
}