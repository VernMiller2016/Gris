using GRis.ViewModels.PaySource;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GRis.ViewModels.Program
{
    public class ProgramDetailsViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "GP Project")]
        [StringLength(9)]
        public string GpProject { get; set; }

        [Display(Name = "Is Active")]
        public bool Active { get; set; }

        [Display(Name = "Related PaySources")]
        public List<PaySourceDetailsViewModel> PaySources { get; set; }
    }
}