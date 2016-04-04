using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net.Mail;
using System.Web.UI.WebControls;
using LoggingAPI.Models;
using LoggingAPI.Models.Forms;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LoggingAPI.Gateways
{
    public class ExerciseLoggerGateway : IExerciseLoggerGateway
    {
        private readonly ExerciseDbContext _dbContext;
        private readonly JJUserManager _userManager;
        private readonly PermissionManager _permissionManager;

        public ExerciseLoggerGateway()
        {
            _dbContext = new ExerciseDbContext();
            _permissionManager = new PermissionManager(new RoleStore<Permission>(_dbContext));
            _userManager = new JJUserManager(new UserStore<User>(_dbContext));

            _userManager.UserValidator = new UserValidator<User>(_userManager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            _userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true
            };



        }
        public User UpdateUser(UserForm form)
        {
            var user = GetUserById(form.UserId);
            user.UpdateUser(form, _userManager, _permissionManager);
            return (user);
        }

        public IdentityResult AddUser(RegisterForm form)
        {
            var user = new User();
            user.UpdateUser(form, _userManager, _permissionManager);
            var result = _userManager.Create(user, form.Password);
            _dbContext.AuditLogs.AddRange(user.AuditLogsToAdd);
            _dbContext.SaveChanges();
            return result;
        }

        public List<User> GetUsers(string queryString)
        {
            throw new NotImplementedException();
        }

        public User GetUserById(string id)
        {
            return _userManager.FindById(id);
        }

        public User GetUserByUserName(string userName)
        {
            return _userManager.FindByUserName(userName);
        }

        public bool ChangePassword(ChangePasswordForm form)
        {
            throw new NotImplementedException();
        }

        public IdentityResult DeleteUser(User user)
        {
            // deleting audit logs.  
            _dbContext.AuditLogs.RemoveRange(user.EditedByAuditLogs);
            _dbContext.AuditLogs.RemoveRange(user.UpdatedAuditLogs);
            
            return (_userManager.Delete(user));
        }

        #region Audit Logs stuff

        /// <summary>
        /// TODO:  Need to figure out how to handle all types of audit logs for a user - so we can display everything a user has done
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<AuditLog> GetUserEditedByAuditLogs(string userId)
        {
            return _dbContext.AuditLogs.Where( ual => ual.EditedByUserId == userId).ToList();
        }

        public List<UserAuditLog> GetAuditLogsForUser(string userId)
        {
            return _dbContext.UserAuditLogs.Where(ual => ual.UserIdUpdated == userId).ToList();
        }

        #endregion
    }
}