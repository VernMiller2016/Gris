using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GRis.ViewModels.Program
{
    public class ProgramEditViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "GP Project")]
        [StringLength(9)]
        public string GpProject { get; set; }

        [Display(Name = "Is Active")]
        public bool Active { get; set; }

        [Display(Name = "Related PaySources")]
        [Required(ErrorMessage = "Please select paysource")]
        public int[] SelectedPaySources { get; set; }

        public IEnumerable<SelectListItem> PaySources { get; set; }
    }
}