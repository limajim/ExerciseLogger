using System.Collections.Generic;
using LoggingAPI.Models;
using LoggingAPI.Models.Forms;
using Microsoft.AspNet.Identity;

namespace LoggingAPI.Gateways
{
    public interface IExerciseLoggerGateway
    {
        User UpdateUser(UserForm form);
        IdentityResult AddUser(RegisterForm form);
        bool DisableUser(string userName);
        List<User> GetUsers(string queryString);
        User GetUserById(string id);
        User GetUserByUserName(string userName);
        bool ChangePassword(ChangePasswordForm form);

    }
}