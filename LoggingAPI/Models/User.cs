using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

        public void UpdateUser(RegisterForm form, JJUserManager userManager, PermissionManager permissionManager)
        {
            UpdateUser(
                new UserForm
                {
                    CurrentUserId = form.CurrentUserId,
                    Email = form.Email,
                    FirstName = form.FirstName,
                    LastName = form.LastName,
                    UserName = form.UserName
                }, userManager, permissionManager);


            var auditLog = new AuditLog
            {
                DateEntered = DateTime.Now,
                UserId = form.CurrentUserId,
                EventLogInformation = "Registered user:" + form.UserName
            };

        }

        public void UpdateUser(UserForm form, JJUserManager userManager, PermissionManager permissionManager)
        {
            FirstName = form.FirstName;
            LastName = form.LastName;
            UserName = form.UserName;
            Email = form.Email;

            var auditLog = new AuditLog
            {
                DateEntered = DateTime.Now,
                UserId = form.CurrentUserId,
                EventLogInformation = "Registered user:" + form.UserName
            };


            //Delete permissions
            var userPermissionsToDelete =
                Roles.Where(userRole => form.PermissionIds.All(ur => ur != userRole.RoleId)).ToList();
            if (userPermissionsToDelete.Any())
            {
                foreach (
                    var permission in
                        userPermissionsToDelete.Select(
                            userPermission => permissionManager.FindById(userPermission.RoleId)))
                {
                    var result = userManager.RemoveFromRole(Id, permission.Name);
                    if (result.Succeeded)
                    {
                        // Add log here.
                    }
                    else
                    {
                        throw new ApplicationException(result.Errors.First());
                    }
                }
            }

            // Add permissions
            var userPermissionsToAdd = form.PermissionIds.Where(
                p => !Roles.Any(r => r.RoleId.Equals(p, StringComparison.InvariantCultureIgnoreCase))).ToList();

            foreach (
                var permission in userPermissionsToAdd.Select(userPermId => permissionManager.FindById(userPermId)))
            {
                var result = userManager.AddToRole(Id, permission.Name);
                if (result.Succeeded)
                {
                    // Add log here.
                }
                else
                {
                    throw new ApplicationException("Failed to add user to role");
                }
            }


        }

    }
}