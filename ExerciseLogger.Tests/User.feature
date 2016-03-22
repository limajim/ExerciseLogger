Feature: User
	In order to Register and Log into the application
	As a person
	I want to have a User entity

@mytag
Scenario: Register
	Given I am using User Name = "testregister", EmailAddress = "test@cscos.com", First Name = "Register", Last Name="User", Password = "Test1!234", ConfirmedPassword = "Test1!234"
	When I request the Single Sign On to register
	Then the result should be a successful response
