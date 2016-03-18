using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using LoggingAPI.Models.Forms;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LoggingAPI.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual List<AuditLog> AuditLogs { get; set; }

        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }

        public int UpdateUser(UserForm form, JJUserManager userManager, PermissionManager permissionManager)
        {
            FirstName = form.FirstName;
            LastName = form.LastName;
            UserName = form.UserName;

            var auditLog = new AuditLog
            {
                DateEntered = DateTime.Now,
                UserId = form.CurrentUserId,
                EventLogInformation = "Registered user:" + form.UserName
            };

            return (0);
        }

        public IdentityResult RegisterUser(RegisterForm form, JJUserManager userManager)
        {
            FirstName = form.FirstName;
            LastName = form.LastName;
            UserName = form.UserName;

            var result = userManager.Create(this, form.Password);

            var auditLog = new AuditLog{DateEntered = DateTime.Now,UserId=form.CurrentUserId,
                EventLogInformation="Registered user:"+form.UserName};

            return (result);
        }
    }
}