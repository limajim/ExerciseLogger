using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LoggingAPI.Models.Interfaces;

namespace LoggingAPI.Models.Forms
{
    public class UserForm : IForm, IValidatableObject
    {
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

        public string CurrentUserId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return null;
        }

    }
}