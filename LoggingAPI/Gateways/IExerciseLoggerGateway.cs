using System.Collections.Generic;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using System;
using LoggerLibrary.ViewModels;
using LoggingAPI.Models;
using LoggerLibrary.Forms;

namespace LoggingAPI.Gateways
{
    public interface IExerciseLoggerGateway : IDisposable
    {
        User UpdateUser(UserForm form);
        IdentityResult AddUser(RegisterForm form);
        List<UserViewModel> GetUsers();
        UserViewModel GetUserById(string id);
        UserViewModel GetUserByUserName(string userName);
        bool ChangePassword(ChangePasswordForm form);

        List<AuditLogViewModel> GetUserEditedByAuditLogs(string userId);
        List<AuditLogViewModel> GetAuditLogsForUser(string userId);
    }
}