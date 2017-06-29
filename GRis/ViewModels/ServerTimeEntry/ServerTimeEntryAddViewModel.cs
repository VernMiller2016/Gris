using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GRis.ViewModels.ServerTimeEntry
{
    public class ServerTimeEntryAddViewModel
    {
        [Required]
        [Display(Name = "Server")]
        public int ServerId { get; set; }

        [Required]
        [Display(Name = "PaySource")]
        public int PaySourceId { get; set; }

        [Display(Name = "Program")]
        public int? ProgramId { get; set; }

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

        public IEnumerable<SelectListItem> SelectedPrograms { get; set; }
    }
}