using System;
using LoggingAPI.Gateways;
using LoggingAPI.Models.Forms;
using Microsoft.AspNet.Identity;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace ExerciseLogger.Tests
{
    [Binding]
    public class UserSteps
    {

        private string _userName;
        private string _firstName;
        private string _password;
        private string _emailAddress;
        private IdentityResult _result;
        private ExerciseLoggerGateway _gateway;
        private RegisterForm _registerForm;



        [Given(@"I am using User Name = ""(.*)"", EmailAddress = ""(.*)"", First Name = ""(.*)"", Last Name=""(.*)"", Password = ""(.*)"", ConfirmedPassword = ""(.*)""")]
        public void GivenIAmUsingUserNameEmailAddressFirstNameLastNamePasswordConfirmedPassword(string userName, string emailAddress, string firstName, string lastName, string password, string confirmedPassword)
        {
            _gateway = new ExerciseLoggerGateway();
            _registerForm = new RegisterForm
            {
                ConfirmPassword = confirmedPassword,
                CurrentUserId = "jisdsd jksdfj",
                Email = emailAddress,
                UserName = userName,
                FirstName = firstName,
                LastName = lastName,
                Password = password
            };
        }
        
        [When(@"I request the Single Sign On to register")]
        public void WhenIRequestTheSingleSignOnToRegister()
        {
            _result = _gateway.AddUser(_registerForm);
        }
        
        [Then(@"the result should be a successful response")]
        public void ThenTheResultShouldBeASuccessfulResponse()
        {
            Assert.IsTrue(_result.Succeeded);
        }
    }
}
