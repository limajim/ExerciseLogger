using System.Collections.Generic;

namespace LoggerLibrary.ViewModels
{
    public class UserViewModel
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsEnabled { get; set; }

        public List<PermissionViewModel> Permissions { get; set; }
    }
}