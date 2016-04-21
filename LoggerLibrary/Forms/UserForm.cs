using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LoggerLibrary.Interfaces;

namespace LoggerLibrary.Forms
{
    public class UserForm : IForm, IValidatableObject
    {
        public UserForm()
        {
            PermissionIds = new List<string>();
        }


        public string UserId { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(20)]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public bool IsEnabled { get; set; }

        public UserForm CurrentUser { get; set; }

        public byte[] RowVersion { get; set; }


        public List<string> PermissionIds { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return null;
        }

    }
}