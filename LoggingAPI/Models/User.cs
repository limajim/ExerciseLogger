using System;
using System.Collections.Generic;
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
        public User()
        {
            AuditLogsToAdd = new List<UserAuditLog>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<UserAuditLog> AuditLogsToAdd { get; set; }

        public virtual IList<UserAuditLog> UpdatedAuditLogs { get; set; }
        public virtual IList<UserAuditLog> EditedByAuditLogs { get; set; }

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
                    CurrentUser = form.CurrentUser,
                    Email = form.Email,
                    FirstName = form.FirstName,
                    LastName = form.LastName,
                    UserName = form.UserName
                }, userManager, permissionManager);
        }

        public void UpdateUser(UserForm form, JJUserManager userManager, PermissionManager permissionManager)
        {
            var newUser = string.IsNullOrEmpty(FirstName);
            if (newUser)
            {
                
                AuditLogsToAdd.Add(new UserAuditLog
                {
                    DateEntered = DateTime.Now,
                    EditedByUser = form.CurrentUser,
                    EventLogInformation = "Registered user: " + form.UserName,
                    UpdatedUserId = form.UserId
                });
            }
            else
            {
                if (!FirstName.Equals(form.FirstName, StringComparison.InvariantCultureIgnoreCase))
                    AuditLogsToAdd.Add(new UserAuditLog
                    {
                        DateEntered = DateTime.Now,
                        EditedByUser = form.CurrentUser,
                        EventLogInformation =
                            "Updated First Name from:" + FirstName + " to " + form.FirstName + " for User: " + UserName//,
//                        UpdatedUserId = form.UserId
                    });
                if (!LastName.Equals(form.LastName, StringComparison.InvariantCultureIgnoreCase))
                    AuditLogsToAdd.Add(new UserAuditLog
                    {
                        DateEntered = DateTime.Now,
                        EditedByUser = form.CurrentUser,
                        EventLogInformation =
                            "Updated Last Name from:" + LastName + " to " + form.LastName + " for User: " + UserName//,
  //                      UpdatedUserId = form.UserId
                    });

                if (!UserName.Equals(form.UserName, StringComparison.InvariantCultureIgnoreCase))
                    AuditLogsToAdd.Add(new UserAuditLog
                    {
                        DateEntered = DateTime.Now,
                        EditedByUser = form.CurrentUser,
                        EventLogInformation =
                            "Updated User Name from:" + UserName + " to " + form.UserName + " for User: " +
                            form.UserName//,
//                        UpdatedUserId = form.UserId
                    });
                if (!Email.Equals(form.Email, StringComparison.InvariantCultureIgnoreCase))
                    AuditLogsToAdd.Add(new UserAuditLog
                    {
                        DateEntered = DateTime.Now,
                        EditedByUser = form.CurrentUser,
                        EventLogInformation =
                            "Updated Email from:" + Email + " to " + form.Email + " for User: " + UserName//,
//                        UpdatedUserId = form.UserId
                    });
            }

            FirstName = form.FirstName;
            LastName = form.LastName;
            UserName = form.UserName;
            Email = form.Email;

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
                        AuditLogsToAdd.Add(new UserAuditLog
                        {
                            DateEntered = DateTime.Now,
                            EditedByUser = form.CurrentUser,
                            EventLogInformation = "Deleted Permission:" + permission.Name + " for User: " + UserName//,
                            //UpdatedUserId = form.UserId
                        });
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
                    AuditLogsToAdd.Add(new UserAuditLog
                    {
                        DateEntered = DateTime.Now,
                        EditedByUser = form.CurrentUser,
                        EventLogInformation = "Added Permission:" + permission.Name + " for User: " + UserName//,
                        //UpdatedUserId = form.UserId
                    });
                }
                else
                {
                    throw new ApplicationException("Failed to add user to role");
                }
            }
        }
    }
}