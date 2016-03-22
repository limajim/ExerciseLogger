using System;
using System.Linq;
using LoggingAPI.Gateways;
using LoggingAPI.Models;
using LoggingAPI.Models.Forms;
using Microsoft.AspNet.Identity;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace ExerciseLogger.Tests
{
    [Binding]
    public class UserSteps
    {

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


        private User deleteAndRegisterUser()
        {
            var user = _gateway.GetUserByUserName(_user.UserName);

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
                CurrentUserId = "5cd6da42-a92a-4641-a427-a3e95fcb3683",
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

            _user = new User { Email = emailAddress, FirstName = firstName, LastName = lastName, UserName = userName };

            _gateway = new ExerciseLoggerGateway();
        }
        
        [When(@"I request the Single Sign On to register")]
        public void WhenIRequestTheSingleSignOnToRegister()
        {
            // See if the user exists
            var user = _gateway.GetUserByUserName(_user.UserName);
            if (user != null)
            {
                // Delete it because we want to test to make sure we can register
                WhenIRequestToDeleteTheUserEntity();
            }

            var registerForm = new RegisterForm
            {
                // jvandick user for testing purposes
                CurrentUserId = "5cd6da42-a92a-4641-a427-a3e95fcb3683",
                Email = _user.Email,
                UserName = _user.UserName,
                FirstName = _user.FirstName,
                LastName = _user.LastName,
                Password = _password,
                ConfirmPassword = _confirmedPassword
            };

            _result = _gateway.AddUser(registerForm);
        }

        [When(@"I request to delete the User entity")]
        public void WhenIRequestToDeleteTheUserEntity()
        {
            var user = _gateway.GetUserByUserName(_user.UserName);
            if (user == null)
                user = deleteAndRegisterUser();

            _result = _gateway.DeleteUser(user);
        }


        [Then(@"the result should be a successful response")]
        public void ThenTheResultShouldBeASuccessfulResponse()
        {
            // Send out the errors if the result didnt succeed
            var errMsg = _result.Errors.Aggregate("", (current, error) => current + (error + ".  "));

            Assert.IsTrue(_result.Succeeded,
                "The IdentityResult should be Succeeded=true error( " + errMsg + " )");
        }
    }
}
