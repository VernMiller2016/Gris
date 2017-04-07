using System;
using System.ComponentModel.DataAnnotations;

namespace GRis.ViewModels.ServerTimeEntry
{
    public class ServerTimeEntryDetailsViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Server Id")]
        public int ServerVendorId { get; set; }

        [Display(Name = "PaySource Id")]
        public int PaySourceVendorId { get; set; }

        [Display(Name = "PaySource")]
        public string PaySourceDescription { get; set; }

        [Display(Name = "Server Name")]
        public string ServerName { get; set; }

        [Display(Name = "Duration")]
        public TimeSpan Duration { get; set; }

        [Display(Name = "Begin Date")]
        [DataType(DataType.Date)]
        public DateTime BeginDate { get; set; }
    }
}