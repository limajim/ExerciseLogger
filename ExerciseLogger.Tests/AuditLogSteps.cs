using LoggingAPI.Gateways;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using LoggingAPI.Models;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace ExerciseLogger.Tests
{
    [Binding]
    public class AuditLogSteps
    {
        private string _userId;
        private ExerciseLoggerGateway _gateway;
        private List<UserAuditLog> _userAuditLogs;

        [Given(@"I have a User Id = ""(.*)""")]
        public void GivenIHaveAUserId(string userId)
        {
            _userId = userId;
            _gateway = new ExerciseLoggerGateway();

        }
        
        [When(@"I request Audit Logs edited by him/her")]
        public void WhenIRequestAuditLogsEditedByHimHer()
        {
            _userAuditLogs = _gateway.GetUserAuditLogs(_userId);
        }
        
        [Then(@"the result should return the User Audit Logs that were created for that user")]
        public void ThenTheResultShouldReturnTheUserAuditLogsThatWereCreatedForThatUser()
        {
            Assert.IsTrue(_userAuditLogs.TrueForAll(au => au.EditedByUserId == _userId));
        }

    }
}
