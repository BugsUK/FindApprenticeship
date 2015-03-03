Feature: SubmitQuery
	In order to finding out more about employing apprentices 
	As an employer 
	I want to able to send employer query form

@SmokeTests
Scenario: As a employer I am on the employer contact page and all required fields are present and all validators show
	Given I navigated to the EmployerEnquiryPage page	
	When I am on the EmployerEnquiryPage page
	And I wait to see Firstname
	And I wait to see Lastname	
	And I wait to see EmailAddress
	And I wait to see WorkPhoneNumber
	And I wait to see Companyname
	And I wait to see Position
	And I wait to see EmployeesCount
	And I wait to see WorkSector
	And I wait to see EnquiryDescription
	And I wait to see PreviousExperienceType
	And I wait to see EnquirySource
	And I wait to see AddressLine1
	And I wait to see City
	And I wait to see Postcode
	And I choose SendEmployerEnquiryButton
	And I wait to see ValidationSummary
	Then I see
        | Field                  | Rule   | Value |
        | ValidationSummaryCount | Equals | 14    |
	And I am on ValidationSummaryItems list item matching criteria
		| Field | Rule   | Value                   |
		| Text  | Equals | Please enter first name |
		| Href  | Equals | #firstname              |
	And I am on the EmployerEnquiryPage page
	And I am on ValidationSummaryItems list item matching criteria
		| Field | Rule   | Value                  |
		| Text  | Equals | Please enter last name |
		| Href  | Equals | #lastname              |	
	And I am on the EmployerEnquiryPage page
	And I enter data
		| Field           | Value                  |
		| Firstname       | FirstnameTest          |
		| Lastname        | LastnameTest           |
		| Position        | PositionTest           |		
		| EmailAddress    | helpdesk.sfa@gmail.com |
		| WorkPhoneNumber | 07999999999            |	
	Then I see 
		| Field                     | Rule   | Value |
		| ValidationFieldErrorCount | Equals | 9     |
	And I am on the EmployerEnquiryPage page
	And I enter data
		| Field        | Value            |
		| Companyname  | CompanynameTest  |
		| AddressLine1 | 1, Oxford street |
		| City         | London           |
		| Postcode     | tw5 0by          |	
	Then I see 
		| Field                     | Rule   | Value |
		| ValidationFieldErrorCount | Equals | 5     |
	And I am on the EmployerEnquiryPage page
	And I choose EmployeeCountDropdown 		
	Then I see 
		| Field                     | Rule   | Value |
		| ValidationFieldErrorCount | Equals | 4     |