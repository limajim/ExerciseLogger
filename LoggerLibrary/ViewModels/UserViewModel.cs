using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LoggerLibrary.ViewModels
{
    public class UserViewModel
    {
        public string UserId { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public bool IsEnabled { get; set; }

        public List<PermissionViewModel> Permissions { get; set; }
    }
}