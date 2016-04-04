Feature: AuditLog
	In order to assure that Auditing is being done
	As a person
	I want to be able to view audit log information

@mytag
Scenario: GetAuditLogsCreatedByUser
	Given I have a User Id = "5cd6da42-a92a-4641-a427-a3e95fcb3683"
	When I request Audit Logs edited by him/her
	Then the result should return the Audit Logs that were created for that user
