using System.Collections.Generic;
using LoggingAPI.Models;
using LoggingAPI.Models.Forms;

namespace LoggingAPI.Gateways
{
    public interface IExerciseLoggerGateway
    {
        User UpdateUser(UserForm form);
        User RegisterUser(RegisterForm form);
        bool DisableUser(string userName);
        List<User> GetUsers(string queryString);
        User GetUserById(string id);
        User GetUserByUserName(string userName);
        bool ChangePassword(ChangePasswordForm form);

    }
}