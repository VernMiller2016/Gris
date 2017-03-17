using System;
using System.ComponentModel.DataAnnotations;

namespace GRis.Models
{
    public class ServiceDetail
    {
        [Required]
        public int ServiceDetailId { get; set; }

        public int SubUnitId { get; set; }

        public int ServerId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Begin Date")]
        public DateTime BeginDate { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:T}", ApplyFormatInEditMode = true)]
        [Display(Name = "Begin Time")]
        public DateTime BeginTime { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:T}", ApplyFormatInEditMode = true)]
        [Display(Name = "End Time")]
        public DateTime EndTime { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:T}", ApplyFormatInEditMode = true)]
        public DateTime Duration { get; set; }

        public int PlaceOfServiceId { get; set; }

        public int PaySourceId { get; set; }
    }
}