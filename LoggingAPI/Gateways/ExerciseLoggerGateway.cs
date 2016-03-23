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
            throw new NotImplementedException();
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

        public bool DisableUser(string userName)
        {
            throw new NotImplementedException();
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
            return (_userManager.Delete(user));
        }

    }
}