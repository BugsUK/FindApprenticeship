@USTBC
Feature: Feedback
	As the SFA 
	we want to offer the candidate the opportunity to provide feedback

Background: 
	Given I navigated to the ApprenticeshipSearchPage page
	And I am logged out
	And I navigated to the ApprenticeshipSearchPage page
	Then I am on the ApprenticeshipSearchPage page

Scenario: Feedback form
	Given I navigated to the FeedbackPage page
	When I am on the FeedbackPage page
	And I choose GiveFeedbackButton
	Then I see 
		| Field                     | Rule   | Value |
		| ValidationFieldErrorCount | Equals | 1     |
	When I am on the FeedbackPage page
	And I enter data
		| Field   | Value               |
		| Name    | Jane Doe            |
		| Email   | someone@example.com |
		| Details | Some feedback       |
	When I am on the FeedbackPage page
	Then I see 
		| Field                     | Rule   | Value |
		| ValidationFieldErrorCount | Equals | 0     |
