using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GRis.ViewModels.ServerTimeEntry
{
    public class ServerTimeEntryEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Server")]
        public int ServerVendorId { get; set; }

        [Required]
        [Display(Name = "PaySource")]
        public int PaySourceVendorId { get; set; }

        [Required]
        [Display(Name = "Duration")]
        [UIHint("TimePicker")]
        public TimeSpan? Duration { get; set; }

        [Required]
        [Display(Name = "Begin Date")]
        [UIHint("DatePicker")]
        public DateTime? BeginDate { get; set; }

        public IEnumerable<SelectListItem> SelectedServers { get; set; }

        public IEnumerable<SelectListItem> SelectedPaySources { get; set; }
    }
}