using LoggingAPI.Gateways;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using LoggerLibrary.Forms;
using LoggerLibrary.ViewModels;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace ExerciseLogger.Tests
{
    [Binding]
    public class AuditLogSteps
    {
        private string _userId;
        private ExerciseLoggerGateway _gateway;
        private List<AuditLogViewModel> _editedBAuditLogs;
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
            _editedBAuditLogs = _gateway.GetUserEditedByAuditLogs(_userId);
        }


        [Then(@"the result should return the Audit Logs that were created for that user")]
        public void ThenTheResultShouldReturnTheAuditLogsThatWereCreatedForThatUser()
        {
            Assert.IsTrue(_editedBAuditLogs.TrueForAll(au => au.EditedByUserId == _userId));
        }



    }
}
