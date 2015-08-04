@US276
Feature: Forgotten Password
	As a candidate who has forgotten my password
	I want to request to reset my password
	so that I can sign in to my account

Background: 
	Given I navigated to the ApprenticeshipSearchPage page
	And I am logged out
	And I navigated to the ApprenticeshipSearchPage page
	Then I am on the ApprenticeshipSearchPage page

Scenario: Reset password successful
	Given I have registered a new candidate
	When I navigate to the ForgottenCredentialsPage page
	Then I am on the ForgottenCredentialsPage page
	When I enter data
		| Field        | Value        |
		| EmailAddress | {EmailToken} |
	And I choose SendCodeButton
	Then I am on the ResetPasswordPage page
	When I get the token to reset the password
	And I navigate to the ForgottenCredentialsPage page
	When I am on the ForgottenCredentialsPage page
	And I enter data
		| Field        | Value        |
		| EmailAddress | {EmailToken} |
	And I choose SendCodeButton
	Then I am on the ResetPasswordPage page
	And I get the same token to reset the password
	When I enter data
		| Field             | Value                    |
		| PasswordResetCode | {PasswordResetCodeToken} |
		| Password          | {NewPasswordToken}       |
		| ConfirmPassword   | {NewPasswordToken}       |
	And I choose ResetPasswordButton
	Then I am on the ApprenticeshipSearchPage page
	And I see
		| Field              | Rule   | Value                                   |
		| SuccessMessageText | Equals | You've successfully reset your password |

Scenario: Reset password in an unactivated account
	Given I navigated to the RegisterCandidatePage page
	When I have created a new email address
	And I enter data
		| Field           | Value         |
		| Firstname       | FirstnameTest |
		| Lastname        | LastnameTest  |
		| Phonenumber     | 07469984649   |
		| EmailAddress    | {EmailToken}  |
		| Day             | 01            |
		| Month           | 01            |
		| Year            | 2000          |
		| Password        | ?Password01!  |
		| ConfirmPassword | ?Password01!  |
	And I choose HasAcceptedTermsAndConditions
	And I enter data
		| Field          | Value  |
		| PostcodeSearch | N7 8LS |
	And I wait 3 seconds
	And I am on AddressDropdown list item matching criteria
		| Field | Rule   | Value                           |
		| Text  | Equals | N7 8LS, 6, Furlong Road, London |
	And I choose WrappedElement
	And I am on the RegisterCandidatePage page
	And I choose CreateAccountButton
	Then I wait 120 second for the ActivationPage page
	When I navigate to the ForgottenCredentialsPage page
	And I am on the ForgottenCredentialsPage page
	And I enter data
		| Field        | Value        |
		| EmailAddress | {EmailToken} |
	And I choose SendCodeButton
	Then I am on the ResetPasswordPage page
	And I get the token to reset the password
	When I enter data
		| Field             | Value                    |
		| PasswordResetCode | {PasswordResetCodeToken} |
		| Password          | {NewPasswordToken}       |
		| ConfirmPassword   | {NewPasswordToken}       |
	And I choose ResetPasswordButton
	Then I am on the ApprenticeshipSearchPage page
	And I see
		| Field              | Rule   | Value                                   |
		| SuccessMessageText | Equals | You've successfully reset your password |