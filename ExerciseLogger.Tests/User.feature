Feature: User
	In order to Register and Log into the application
	As a person
	I want to have a User entity

@mytag
Scenario: Register
	Given I am using User Name = "testregister", EmailAddress = "test@cscos.com", First Name = "Register", Last Name="User", Password = "Test1!234", ConfirmedPassword = "Test1!234"
	When I request the Single Sign On to register
	Then the result should be a successful response

Scenario: Delete
	Given I am using User Name = "testregister", EmailAddress = "test@cscos.com", First Name = "Register", Last Name="User", Password = "Test1!234", ConfirmedPassword = "Test1!234"
	When I request to delete the User entity
	Then the result should be a successful response

Scenario: Disable User
	Given I am using User Name = "testregister", EmailAddress = "test@cscos.com", First Name = "Register", Last Name="User", Password = "Test1!234", ConfirmedPassword = "Test1!234"
	And the user is enabled
	When I request to disable the User
	Then the result should be a disabled User

Scenario: Enable User
	Given I am using User Name = "testregister", EmailAddress = "test@cscos.com", First Name = "Register", Last Name="User", Password = "Test1!234", ConfirmedPassword = "Test1!234"
	And the user is disabled
	When I request to enable User
	Then the result should be an enabled User


Scenario: Get User Audit Logs
	Given I am using User Name = "testregister", EmailAddress = "test@cscos.com", First Name = "Register", Last Name="User", Password = "Test1!234", ConfirmedPassword = "Test1!234"
	When I Request Audit Logs Associated To The User
	Then the result should be a list of Audit Logs