using System;
using System.Linq;
using LoggingAPI.Gateways;
using LoggingAPI.Models;
using LoggingAPI.Models.Forms;
using Microsoft.AspNet.Identity;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using TechTalk.SpecFlow;

namespace ExerciseLogger.Tests
{
    [Binding]
    public class UserSteps
    {
        private const string EDITED_BY_USER_ID = "5cd6da42-a92a-4641-a427-a3e95fcb3683";

        //private string _userName;
        //private string _firstName;
        //private string _lastName;
//        private string _emailAddress;
        private string _password;
        private string _confirmedPassword;
        private IdentityResult _result;
        private ExerciseLoggerGateway _gateway;
//        private RegisterForm _registerForm;

        private User _user;
        private UserForm _userForm;


        private User deleteAndRegisterUser()
        {
            var user = _gateway.GetUserByUserName(_userForm.UserName);

            var userName = _user.UserName;
            var firstName = _user.FirstName;
            var lastName = _user.LastName;
            var email = _user.Email;



            if (user != null)
            {
                _gateway.DeleteUser(user);
            }

            var result = _gateway.AddUser(new RegisterForm
            {
                // jvandick user for testing purposes
                CurrentUser = _gateway.GetUserById(EDITED_BY_USER_ID),
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                Password = _password,
                ConfirmPassword = _password,
                UserName =  userName
            });
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First());
            }
            user = _gateway.GetUserByUserName(_user.UserName);
            return user;
        }



        [Given(@"I am using User Name = ""(.*)"", EmailAddress = ""(.*)"", First Name = ""(.*)"", Last Name=""(.*)"", Password = ""(.*)"", ConfirmedPassword = ""(.*)""")]
        public void GivenIAmUsingUserNameEmailAddressFirstNameLastNamePasswordConfirmedPassword(string userName, string emailAddress, string firstName, string lastName, string password, string confirmedPassword)
        {
            //_userName = userName;
            //_emailAddress = emailAddress;
            //_firstName = firstName;
            //_lastName = lastName;
            _password = password;
            _confirmedPassword = confirmedPassword;

            _userForm = new UserForm
            {
                Email = emailAddress,
                FirstName = firstName,
                LastName = lastName,
                UserName = userName
            };

            _gateway = new ExerciseLoggerGateway();
        }

        [Given(@"the user is enabled")]
        public void GivenTheUserIsEnabled()
        {
            // Get the user
            _user = _gateway.GetUserByUserName(_userForm.UserName);
            if (_user == null)
            {
                // Recreate it
                WhenIRequestTheSingleSignOnToRegister();
                _user = _gateway.GetUserByUserName(_userForm.UserName);
            }


            if (!_user.IsEnabled)
            {
                var userForm = _user.ToForm();
                userForm.CurrentUser = _gateway.GetUserById(EDITED_BY_USER_ID);
                userForm.IsEnabled = true;
                _user = _gateway.UpdateUser(userForm);
            }
        }

        [Given(@"the user is disabled")]
        public void GivenTheUserIsDisabled()
        {
            _user = _gateway.GetUserByUserName(_userForm.UserName);
            if (_user == null)
            {
                // Recreate it
                WhenIRequestTheSingleSignOnToRegister();
                _user = _gateway.GetUserByUserName(_userForm.UserName);
            }

            if (_user.IsEnabled)
            {
                var userForm = _user.ToForm();
                userForm.CurrentUser = _gateway.GetUserById(EDITED_BY_USER_ID);
                userForm.IsEnabled = false;
                _user = _gateway.UpdateUser(userForm);
            }
        }


        [When(@"I request the Single Sign On to register")]
        public void WhenIRequestTheSingleSignOnToRegister()
        {
            // See if the user exists
            _user = _gateway.GetUserByUserName(_userForm.UserName);
            if (_user != null)
            {
                // Delete it because we want to test to make sure we can register
                WhenIRequestToDeleteTheUserEntity();
            }

            var registerForm = new RegisterForm
            {
                // jvandick user for testing purposes
                CurrentUser = _gateway.GetUserById(EDITED_BY_USER_ID),
                Email = _userForm.Email,
                UserName = _userForm.UserName,
                FirstName = _userForm.FirstName,
                LastName = _userForm.LastName,
                Password = _password,
                ConfirmPassword = _confirmedPassword
            };

            _result = _gateway.AddUser(registerForm);
        }

        [When(@"I request to delete the User entity")]
        public void WhenIRequestToDeleteTheUserEntity()
        {
            _user = _gateway.GetUserByUserName(_userForm.UserName);
            if (_user == null)
                _user = deleteAndRegisterUser();

            _result = _gateway.DeleteUser(_user);
        }


        [When(@"I Request Audit Logs Associated To The User")]
        public void WhenIRequestAuditLogsAssociatedToTheUser()
        {
            _user = _gateway.GetUserByUserName(_userForm.UserName);
            if (_user == null)
                _user = deleteAndRegisterUser();

        }


        [When(@"I request to disable the User")]
        public void WhenIRequestToDisableTheUser()
        {
            var userForm = _user.ToForm();
            userForm.CurrentUser = _gateway.GetUserById(EDITED_BY_USER_ID);
            userForm.IsEnabled = false;
            _gateway.UpdateUser(userForm);
        }

        [When(@"I request to enable User")]
        public void WhenIRequestToEnableUser()
        {
            var userForm = _user.ToForm();
            userForm.CurrentUser = _gateway.GetUserById(EDITED_BY_USER_ID);
            userForm.IsEnabled = true;
            _gateway.UpdateUser(userForm);
        }


        [Then(@"the result should be a disabled User")]
        public void ThenTheResultShouldBeADisabledUser()
        {
            Assert.IsFalse(_user.IsEnabled);
        }


        [Then(@"the result should be an enabled User")]
        public void ThenTheResultShouldBeAnEnabledUser()
        {
            Assert.IsTrue(_user.IsEnabled);
        }


        [Then(@"the result should be a successful response")]
        public void ThenTheResultShouldBeASuccessfulResponse()
        {
            // Send out the errors if the result didnt succeed
            var errMsg = _result.Errors.Aggregate("", (current, error) => current + (error + ".  "));

            var user = _gateway.GetUserById(EDITED_BY_USER_ID);

            Assert.IsTrue(_result.Succeeded,
                "The IdentityResult should be Succeeded=true error( " + errMsg + " )");
        }

        [Then(@"the result should be a list of Audit Logs")]
        public void ThenTheResultShouldBeAListOfAuditLogs()
        {
            Assert.IsTrue(_user.UpdatedAuditLogs.TrueForAll(au => au.UpdatedUser.Id == _user.Id));
        }




    }
}
