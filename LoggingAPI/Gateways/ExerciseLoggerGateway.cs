using System;
using System.Collections.Generic;
using System.Net.Mail;
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

            return _userManager.Create(user, form.Password);
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
            throw new NotImplementedException();
        }

        public User GetUserByUserName(string userName)
        {
            throw new NotImplementedException();
        }

        public bool ChangePassword(ChangePasswordForm form)
        {
            throw new NotImplementedException();
        }

    }
}