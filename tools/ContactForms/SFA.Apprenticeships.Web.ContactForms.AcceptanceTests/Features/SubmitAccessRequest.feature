Feature: SubmitAccessRequest
	In order to get access to system
	As an applicant
	I want to able to send web access request

@SmokeTests
Scenario: As a applicant I am on the access request page
	and all required fields are present and all validators show 
	and on save page navigates to ThanYou page with success message
	Given I navigated to the AccessRequestPage page	
	When I am on the AccessRequestPage page
	And I wait to see Firstname
	And I wait to see Lastname	
	And I wait to see EmailAddress
	And I wait to see WorkPhoneNumber
	And I wait to see Companyname
	And I wait to see Position	
	And I wait to see AddressLine1
	And I wait to see City
	And I wait to see Postcode
	And I choose SendAccessRequestButton
	And I wait to see ValidationSummary
	Then I see
        | Field                  | Rule   | Value |
        | ValidationSummaryCount | Equals | 11    |	
	And I am on the AccessRequestPage page
	When I am on UserTypeDropdown list item matching criteria
		| Field | Rule   | Value    |
		| Text  | Equals | Employer |
	And I choose WrappedElement
	Then I see 
		| Field                     | Rule   | Value |
		| ValidationFieldErrorCount | Equals | 10    |	
	And I am on the AccessRequestPage page
	And I enter data
		| Field               | Value                  |
		| Firstname           | FirstnameTest          |
		| Lastname            | LastnameTest           |
		| Position            | PositionTest           |
		| EmailAddress        | helpdesk.sfa@gmail.com |
		| ConfirmEmailAddress | helpdesk.sfa@gmail.com |
		| WorkPhoneNumber     | 07999999999            |	
	Then I see 
		| Field                     | Rule   | Value |
		| ValidationFieldErrorCount | Equals | 4     |
	And I am on the AccessRequestPage page
	And I enter data
		| Field        | Value            |
		| Companyname  | CompanynameTest  |
		| AddressLine1 | 1, Oxford street |
		| City         | London           |
		| Postcode     | tw5 0by          |		
	Then I see 
		| Field                     | Rule   | Value |
		| ValidationFieldErrorCount | Equals | 0     |
	And I am on the AccessRequestPage page
	When I choose SendAccessRequestButton	
	And I am on the ThankYouPageAccessRequest page
	And I wait to see ThankYouLabel
	And I wait to see SuccessMessageLabel
	Then I see
        | Field               | Rule   | Value                                                                                                                                                                                                     |
        | ThankYouLabel       | Equals | Thank You                                                                                                                                                                                                 |
        | SuccessMessageLabel | Equals | Thank you, your request has been successfully sent. A member of our support team will contact you in the next 2 working days. <br/><br/> Alternatively you could join us on Facebook, Twitter or Linkedin |
	


@SmokeTest
Scenario: As a applicant I am on the access request page and all required fields are present and all validators show correct validation messages	
	Given I navigated to the AccessRequestPage page	
	When I am on the AccessRequestPage page	
	And I choose SendAccessRequestButton
	And I wait to see ValidationSummary
	Then I see
        | Field                  | Rule   | Value |
        | ValidationSummaryCount | Equals | 14    |
	And I am on ValidationSummaryItems list item matching criteria
		| Field | Rule   | Value                   |
		| Text  | Equals | Please enter first name |
		| Href  | Equals | #firstname              |
	And I am on the AccessRequestPage page
	And I am on ValidationSummaryItems list item matching criteria
		| Field | Rule   | Value                  |
		| Text  | Equals | Please enter last name |
		| Href  | Equals | #lastname              |	
		And I am on the AccessRequestPage page
	And I am on ValidationSummaryItems list item matching criteria
		| Field | Rule   | Value                      |
		| Text  | Equals | Please enter email address |
		| Href  | Equals | #email                     |
	And I am on the AccessRequestPage page
	And I am on ValidationSummaryItems list item matching criteria
		| Field | Rule   | Value                          |
		| Text  | Equals | Please enter work phone number |
		| Href  | Equals | #workPhoneNumber               |	
	And I am on the AccessRequestPage page
	And I am on ValidationSummaryItems list item matching criteria
		| Field | Rule   | Value                          |
		| Text  | Equals | Please enter your company name |
		| Href  | Equals | #companyname                   |	
	And I am on the AccessRequestPage page
	And I am on ValidationSummaryItems list item matching criteria
		| Field | Rule   | Value                            |
		| Text  | Equals | Please enter position at company |
		| Href  | Equals | #Position                        |		
	And I am on the AccessRequestPage page
	And I am on ValidationSummaryItems list item matching criteria
		| Field | Rule   | Value                                   |
		| Text  | Equals | Please enter your first line of address |
		| Href  | Equals | #Address_AddressLine1                   |
	And I am on the AccessRequestPage page
	And I am on ValidationSummaryItems list item matching criteria
		| Field | Rule   | Value                  |
		| Text  | Equals | Please enter your city |
		| Href  | Equals | #Address_City          |
	And I am on the AccessRequestPage page
	And I am on ValidationSummaryItems list item matching criteria
		| Field | Rule   | Value                      |
		| Text  | Equals | Please enter your postcode |
		| Href  | Equals | #Address_Postcode          |
	
	