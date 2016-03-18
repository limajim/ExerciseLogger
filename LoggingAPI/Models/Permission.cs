using Microsoft.AspNet.Identity.EntityFramework;

namespace LoggingAPI.Models
{
    public class Permission : IdentityRole
    {
        public Permission()
        {
        }

        public Permission(string name)
        {
            Name = name;
        }

        public string Description { get; set; }
        public string PermissionLevel { get; set; }
    }
}