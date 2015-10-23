@USCookies
Feature: Cookies
	In order to avoid breaching any data protection or GDS cookie policies
	As a user
	I want my cookies to managed correctly as I log in and out of the site

Scenario: As a registered user when I logout I want my cookies to be cleared
	Given I registered an account and activated it
	And I navigated to the LoginPage page
	When I am on the LoginPage page
	And I enter data
		| Field        | Value           |
		| EmailAddress | {EmailToken}    |
		| Password     | {PasswordToken} |
	And I choose SignInButton
	Then I am on the MyApplicationsPage page
	When I select the "first" apprenticeship vacancy in location "N7 8LS" that can apply by this website
	Then I am on the ApprenticeshipDetailsPage page
	When I choose ApplyButton
	Then I am on the ApprenticeshipApplicationPage page
	And I have the cookie 'User.Context' with a populated value 'User.UserName'
	And I have the cookie 'User.Context' with a populated value 'User.FullName'
	And I have the cookie 'User.Context' with a populated value 'User.TermsConditionsVersion'
	And I have the cookie 'User.Data' with a populated value 'Data.SessionId'
	And I have the cookie 'User.Data' with a populated value 'Data.ResultsPerPage'
	And I have the cookie 'User.Data' with a populated value 'Data.ApprenticeshipLevel'
	And I have the cookie 'User.Data' with a populated value 'UserJourney'
	And I have the cookie 'User.Data' with a populated value 'Data.VacancyDistance'
	And I have the cookie 'User.Data' with a populated value 'Data.LastViewedVacancy'
	And I have the cookie 'User.Data' with a populated value 'Data.LastSearchedLocation'
	And I have the cookie 'User.Data' with a populated value 'SearchReturnUrl'
	When I Logout
	Then I am on the LoginPage page
	And I do not have the cookie 'User.Context'
	And I have the cookie 'User.Data' without a value 'Data.ApprenticeshipLevel'
	And I have the cookie 'User.Data' without a value 'Data.VacancyDistance'
	And I have the cookie 'User.Data' without a value 'Data.LastViewedVacancy'
	And I have the cookie 'User.Data' without a value 'SearchReturnUrl'
	And I have the cookie 'User.Data' with a populated value 'Data.LastSearchedLocation'
	And I have the cookie 'User.Data' with a populated value 'UserJourney'
	And I have the cookie 'User.Data' with a populated value 'Data.SessionId'
	And I have the cookie 'User.Data' with a populated value 'Data.ResultsPerPage'
	


