@US454
Feature: Register Candidate and capture monitoring information
	In order to report more accurately on candidate demographics
	As an data analyst
	I want to be able report on the different genders, disabilities and ethnicities

Background: 
	Given I am logged out
	And I navigated to the ApprenticeshipSearchPage page
	Then I am on the ApprenticeshipSearchPage page

Scenario: As a new candidate my additional monitoring information is saved
	Given I navigated to the RegisterCandidatePage page
	And I have created a new email address
	When I enter data
		| Field           | Value         |
		| Firstname       | FirstnameTest |
		| Lastname        | LastnameTest  |
		| Day             | 01            |
		| Month           | 01            |
		| Year            | 1999          |
		| EmailAddress    | {EmailToken}  |
		| Phonenumber     | 07999999999   |
		| Password        | ?Password01!  |
		| ConfirmPassword | ?Password01!  |
		| PostcodeSearch  | N7 8LS        |
	And I choose FindAddresses
	And I wait 3 seconds
	And I am on AddressDropdown list item matching criteria
		| Field        | Rule   | Value                  |
		| Text         | Equals | Flat A, 6 Furlong Road |
		| AddressLine1 | Equals | Flat A                 |
		| AddressLine2 | Equals | 6 Furlong Road         |
		| AddressLine3 | Equals | London                 |
		| AddressLine4 | Equals | Islington              |
		| Postcode     | Equals | N7 8LS                 |
		| Uprn         | Equals | 5300034721             |
		| Latitude     | Equals | 51.54751633697479      |
		| Longitude    | Equals | -0.10660693737952387   |
	And I choose WrappedElement
	And I am on the RegisterCandidatePage page
	And I choose HasAcceptedTermsAndConditions
	And I choose CreateAccountButton
	Then I wait 500 second for the ActivationPage page
	When I get the token for my newly created account
	And I enter data
		| Field          | Value                 |
		| ActivationCode | {ActivationCodeToken} |
	And I choose ActivateButton
	Then I wait 120 second for the MonitoringInformationPage page
	When I choose GenderOther
	And I choose DisabilityPreferNotToSay
	And I choose WantSupportInInterview
	And I enter data
		| Field          | Value                |
		| SupportDetails | Support details text |
	And I choose SaveAndContinueButton
	Then I wait 120 second for the ApprenticeshipSearchPage page
	When I navigate to the SettingsPage page
	Then I see
		| Field          | Rule   | Value                |
		| GenderOther    | Equals | 3                    |
		| SupportDetails | Equals | Support details text |
