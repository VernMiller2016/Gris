using Gris.Application.Core;
using GRis.Validations;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace GRis.ViewModels.ServerAvailableHourModels
{
    public class ServerAvailableHourUploadViewModel
    {
        [AllowedFileExtension(".xlsx")]
        [Required(ErrorMessage = "Please select file to process")]
        [Display(Name = "Excel file")]
        [UIHint("FileUpload")]
        public HttpPostedFileBase ExcelFile { get; set; }

        [UIHint("YearMonthDatePicker")]
        [Display(Name = "Filter by Date")]
        [Required]
        public DateTime? Date { get; set; } = DateTime.Now;

        [Display(Name = "Default Available Hours")]
        [Required]
        public int DefaultAvailableHours { get; set; } = AppSettings.DefaultAvailableHours;

        public string DateAsMonthYear
        {
            get { return Date.HasValue ? Date.Value.ToString("MM/yyyy") : ""; }
        }
    }
}