using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using LoggerLibrary.Forms;
using LoggerLibrary.Interfaces;

namespace LoggingAPI.Models
{
    public class User : IdentityUser, IModel<UserForm>
    {
        public User()
        {
            IsEnabled = true;
            AuditLogsToAdd = new List<UserAuditLog>();
        }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        // DefaultValue does not do anything - it needs to be set in the constructor or you need to override the SQL generator for the specific generator you are using.
        [Required]
        [DefaultValue(true)]
        public bool IsEnabled { get; set; }

        public string Test { get; set; }

        public List<UserAuditLog> AuditLogsToAdd { get; set; }

        public virtual List<UserAuditLog> UpdatedAuditLogs { get; set; }

        public virtual List<UserAuditLog> EditedByAuditLogs { get; set; }


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
                    UpdatedUser = this
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
                            "Updated First Name from:" + FirstName + " to " + form.FirstName + " for User: " + UserName,
                        UpdatedUser = this
                    });
                if (!LastName.Equals(form.LastName, StringComparison.InvariantCultureIgnoreCase))
                    AuditLogsToAdd.Add(new UserAuditLog
                    {
                        DateEntered = DateTime.Now,
                        EditedByUser = form.CurrentUser,
                        EventLogInformation =
                            "Updated Last Name from:" + LastName + " to " + form.LastName + " for User: " + UserName,
                        UpdatedUser = this
                    });

                if (!UserName.Equals(form.UserName, StringComparison.InvariantCultureIgnoreCase))
                    AuditLogsToAdd.Add(new UserAuditLog
                    {
                        DateEntered = DateTime.Now,
                        EditedByUser = form.CurrentUser,
                        EventLogInformation =
                            "Updated User Name from:" + UserName + " to " + form.UserName + " for User: " +
                            form.UserName,
                        UpdatedUser = this
                    });
                if (!Email.Equals(form.Email, StringComparison.InvariantCultureIgnoreCase))
                    AuditLogsToAdd.Add(new UserAuditLog
                    {
                        DateEntered = DateTime.Now,
                        EditedByUser = form.CurrentUser,
                        EventLogInformation =
                            "Updated Email from:" + Email + " to " + form.Email + " for User: " + UserName,
                        UpdatedUser = this
                    });
                if (!IsEnabled.Equals(form.IsEnabled))
                    AuditLogsToAdd.Add(new UserAuditLog
                    {
                        DateEntered = DateTime.Now,
                        EditedByUser = form.CurrentUser,
                        EventLogInformation = (form.IsEnabled?"Enabled ":"Disabled ") + UserName,
                        UpdatedUser = this
                    });

            }

            IsEnabled = form.IsEnabled;
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
                            EventLogInformation = "Deleted Permission:" + permission.Name + " for User: " + UserName //,
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
                        EventLogInformation = "Added Permission:" + permission.Name + " for User: " + UserName //,
                        //UpdatedUserId = form.UserId
                    });
                }
                else
                {
                    throw new ApplicationException("Failed to add user to role");
                }
            }
        }

        /// <summary>
        /// TODO:  May need to add permissions here. 
        /// </summary>
        /// <returns></returns>
        public UserForm ToForm()
        {
            return
                (new UserForm
                {
                    Email = Email,
                    FirstName = FirstName,
                    LastName = LastName,
                    UserId = Id,
                    UserName = UserName,
                    IsEnabled = IsEnabled
                });

        }
    }
}