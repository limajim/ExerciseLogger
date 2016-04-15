using System.Collections.Generic;
using System.Web.UI.WebControls;
using LoggingAPI.Models;
using LoggingAPI.Models.Forms;
using Microsoft.AspNet.Identity;
using System;
using LoggingAPI.Models.ViewModels;

namespace LoggingAPI.Gateways
{
    public interface IExerciseLoggerGateway : IDisposable
    {
        User UpdateUser(UserForm form);
        IdentityResult AddUser(RegisterForm form);
        List<UserViewModel> GetUsers();
        User GetUserById(string id);
        User GetUserByUserName(string userName);
        bool ChangePassword(ChangePasswordForm form);

        List<AuditLog> GetUserEditedByAuditLogs(string userId);
        List<UserAuditLog> GetAuditLogsForUser(string userId);
    }
}