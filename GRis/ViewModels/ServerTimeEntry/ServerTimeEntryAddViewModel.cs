using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GRis.ViewModels.ServerTimeEntry
{
    public class ServerTimeEntryAddViewModel
    {
        public int ServerVendorId { get; set; }

        public int PaySourceVendorId { get; set; }

        public string ServerName { get; set; }

        public TimeSpan Duration { get; set; }

        [Display(Name = "Begin Date")]        
        public DateTime BeginDate { get; set; }
    }
}