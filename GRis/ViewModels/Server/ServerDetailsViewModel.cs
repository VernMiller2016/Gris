using System.ComponentModel.DataAnnotations;

namespace GRis.ViewModels.Server
{
    public class ServerDetailsViewModel
    {
        private string _categoryName;

        public int Id { get; set; }

        [Required]
        [Display(Name = "Server Id")]
        public int VendorId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => LastName + ", " + FirstName;

        [Display(Name = "Is Active")]
        public bool Active { get; set; } = true;

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