using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace GRis.ViewModels.Server
{
    public class ServerDetailsViewModel
    {
        private string _categoryName;
        private string _firstName;
        private string _lastName;

        public int Id { get; set; }

        [Required]
        [Display(Name = "Server Id")]
        public int VendorId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(50)]
        public string FirstName { get { return (new CultureInfo("en-US", false).TextInfo).ToTitleCase(_firstName.ToLower()); } set { _firstName = value; } }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50)]
        public string LastName { get { return (new CultureInfo("en-US", false).TextInfo).ToTitleCase(_lastName.ToLower()); } set { _lastName = value; } }

        [Display(Name = "Full Name")]
        public string FullName => (new CultureInfo("en-US", false).TextInfo).ToTitleCase(LastName.ToLower() + ", " + FirstName.ToLower());

        [Display(Name = "Is Active")]
        public bool Active { get; set; } = true;

        [Display(Name = "Gp Employee Number")]
        public string GpEmpNumber { get; set; }

        [Display(Name = "Element")]
        public string ElementDisplayName { get; set; }

        [Display(Name = "Server Category")]
        public string CategoryName
        {
            get
            {
                if (CategoryId == null)
                {
                    _categoryName = "Not Found";
                }
                else
                {
                    if (CategoryId == 1)
                    {
                        _categoryName = "Combined - admin";
                    }
                    else if (CategoryId == 2)
                    {
                        _categoryName = "MH-admin";
                    }
                    else if (CategoryId == 3)
                    {
                        _categoryName = "MED - admin";
                    }
                    else if (CategoryId == 4)
                    {
                        _categoryName = "CD-Admin";
                    }
                    else
                    {
                        _categoryName = "Not Found";
                    }
                }
                return _categoryName;
            }
        }

        public int? CategoryId { get; set; }
    }
}