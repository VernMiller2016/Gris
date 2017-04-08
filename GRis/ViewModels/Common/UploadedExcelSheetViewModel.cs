using GRis.Validations;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace GRis.ViewModels.General
{
    public class UploadedExcelSheetViewModel
    {
        [AllowedFileExtension(".xlsx")]
        [Required(ErrorMessage = "Please select file to process")]
        [Display(Name = "Excel file")]
        [UIHint("FileUpload")]
        public HttpPostedFileBase ExcelFile { get; set; }
    }
}