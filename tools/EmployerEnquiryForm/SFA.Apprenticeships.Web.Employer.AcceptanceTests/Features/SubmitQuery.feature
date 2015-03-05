Feature: SubmitQuery
	In order to finding out more about employing apprentices 
	As an employer 
	I want to able to send employer query form

@SmokeTests
Scenario: As a employer I am on the employer contact page 
	and all required fields are present and all validators show 
	and on save page navigates to ThanYou page with success message
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
	When I am on EmployeeCountDropdown list item matching criteria
		| Field | Rule   | Value |
		| Text  | Equals | < 9   |
	And I choose WrappedElement
	And I am on the EmployerEnquiryPage page
	When I am on WorkSectorDropdown list item matching criteria
		| Field | Rule   | Value        |
		| Text  | Equals | Construction |
	And I choose WrappedElement
	And I am on the EmployerEnquiryPage page
	When I am on PrevExperienceDropdown list item matching criteria
		| Field | Rule   | Value |
		| Text  | Equals | Yes   |
	And I choose WrappedElement
	And I am on the EmployerEnquiryPage page
	When I am on EnquirySourceDropdown list item matching criteria
		| Field | Rule   | Value     |
		| Text  | Equals | Newspaper |
	And I choose WrappedElement
	And I am on the EmployerEnquiryPage page
	And I enter data
		| Field              | Value                                                                    |
		| EnquiryDescription | This is a UI test to verify validation & submittion is working correctly |	
	Then I see 
		| Field                     | Rule   | Value |
		| ValidationFieldErrorCount | Equals | 0     |
	And I am on the EmployerEnquiryPage page
	When I choose SendEmployerEnquiryButton	
	And I am on the ThankYouPage page
	And I wait to see ThankYouLabel
	And I wait to see SuccessMessageLabel
	Then I see
        | Field               | Rule   | Value                                                                                                                                                                                           |
        | ThankYouLabel       | Equals | Thank You                                                                                                                                                                                       |
        | SuccessMessageLabel | Equals | Thank you, your enquiry has been successfully sent. A member of our support team will contact you in the next 2 working days.  Alternatively you could join us on Facebook, Twitter or Linkedin |
	


	@SmokeTest
Scenario: As a employer I am on the employer contact page and all required fields are present and all validators show correct validation messages	
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
	And I am on ValidationSummaryItems list item matching criteria
		| Field | Rule   | Value                      |
		| Text  | Equals | Please enter email address |
		| Href  | Equals | #EmailAddress              |
		And I am on the EmployerEnquiryPage page
	And I am on ValidationSummaryItems list item matching criteria
		| Field | Rule   | Value                          |
		| Text  | Equals | Please enter work phone number |
		| Href  | Equals | #WorkPhoneNumber               |	
		And I am on the EmployerEnquiryPage page
	And I am on ValidationSummaryItems list item matching criteria
		| Field | Rule   | Value                          |
		| Text  | Equals | Please enter your company name |
		| Href  | Equals | #Companyname                   |	
		And I am on the EmployerEnquiryPage page
	And I am on ValidationSummaryItems list item matching criteria
		| Field | Rule   | Value                            |
		| Text  | Equals | Please enter position at company |
		| Href  | Equals | #Position                        |	
		And I am on the EmployerEnquiryPage page
	And I am on ValidationSummaryItems list item matching criteria
		| Field | Rule   | Value                                                                                      |
		| Text  | Equals | Please select total number of employees or if you don't know then please select don't know |
		| Href  | Equals | #EmployeesCount                                                                            |	
		And I am on the EmployerEnquiryPage page
	And I am on ValidationSummaryItems list item matching criteria
		| Field | Rule   | Value                              |
		| Text  | Equals | Please select your industry sector |
		| Href  | Equals | #WorkSector                        |	
		And I am on the EmployerEnquiryPage page
	And I am on ValidationSummaryItems list item matching criteria
		| Field | Rule   | Value                                   |
		| Text  | Equals | Please tell us the nature of your query |
		| Href  | Equals | #EnquiryDescription                     |	
		And I am on the EmployerEnquiryPage page
	And I am on ValidationSummaryItems list item matching criteria
		| Field | Rule   | Value                                                 |
		| Text  | Equals | Please select previous experience (Yes/No/Don't Know) |
		| Href  | Equals | #PreviousExperienceType                               |	
		And I am on the EmployerEnquiryPage page
	And I am on ValidationSummaryItems list item matching criteria
		| Field | Rule   | Value                                                   |
		| Text  | Equals | Please select what has prompted you to make an enquiry? |
		| Href  | Equals | #EnquirySource                                          |	
		And I am on the EmployerEnquiryPage page
	And I am on ValidationSummaryItems list item matching criteria
		| Field | Rule   | Value                                   |
		| Text  | Equals | Please enter your first line of address |
		| Href  | Equals | #AddressLine1                           |
		And I am on the EmployerEnquiryPage page
	And I am on ValidationSummaryItems list item matching criteria
		| Field | Rule   | Value                  |
		| Text  | Equals | Please enter your city |
		| Href  | Equals | #City                  |
		And I am on the EmployerEnquiryPage page
	And I am on ValidationSummaryItems list item matching criteria
		| Field | Rule   | Value                      |
		| Text  | Equals | Please enter your postcode |
		| Href  | Equals | #Postcode                  |	
	
	