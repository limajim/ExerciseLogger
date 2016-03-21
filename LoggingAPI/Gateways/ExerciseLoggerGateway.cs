using System;
using System.Collections.Generic;
using System.Net.Mail;
using LoggingAPI.Models;
using LoggingAPI.Models.Forms;

namespace LoggingAPI.Gateways
{
    public class ExerciseLoggerGateway : IExerciseLoggerGateway
    {
        public User UpdateUser(UserForm form)
        {
            throw new NotImplementedException();
        }

        public User RegisterUser(RegisterForm form)
        {
            throw new NotImplementedException();
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