using System;
using System.Collections.Generic;
using System.Linq;
using LoggingAPI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using LoggerLibrary.ViewModels;
using LoggerLibrary.Forms;

namespace LoggingAPI.Gateways
{
    public class ExerciseLoggerGateway : IExerciseLoggerGateway
    {
        private ExerciseDbContext _dbContext;
        private JJUserManager _userManager;
        private PermissionManager _permissionManager;

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
        public UserViewModel UpdateUser(UserForm form)
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

        public List<UserViewModel> GetUsers()
        {
            var query = (from usr in _dbContext.Users
                         select new UserViewModel {Email=usr.Email,FirstName=usr.FirstName,LastName=usr.LastName,UserName=usr.UserName, IsEnabled = usr.IsEnabled});
            return (query.ToList());
        }

        public UserViewModel GetUserById(string id)
        {
            return _userManager.FindById(id);
        }

        public UserViewModel GetUserByUserName(string userName)
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
        public List<AuditLogViewModel> GetUserEditedByAuditLogs(string userId)
        {
            return _dbContext.AuditLogs.Where( ual => ual.EditedByUserId == userId).ToList();
        }

        public List<AuditLogViewModel> GetAuditLogsForUser(string userId)
        {
            return _dbContext.UserAuditLogs.Where(ual => ual.UserIdUpdated == userId).ToList();
        }

        #endregion

        public void Dispose()
        {
            if(_userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            if(_dbContext != null )
            {
                _dbContext.Dispose();
                _dbContext = null;
            }
        }
    }
}