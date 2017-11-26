using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GRis.ViewModels.PaySource
{
    public class PaysourceFilterViewModel
    {
        [Display(Name = "Paysource Name")]
        public string PaysourceName { get; set; }
    }
}