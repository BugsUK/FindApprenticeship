@US711
Feature: Update Username
	As a candidate 
	I want to be able to make amendments to my username
	so that I can change email account used for communications

@US711
Scenario: As a candidate I can change my username
	Given I have registered a new candidate
	When I navigate to the SettingsPage page
	And I am on the SettingsPage page
	And I choose ChangeUsernameLink
	Then I am on the UpdateEmailAddressPage page
	When I create a new email address to update my username
	And I enter data
		| Field       | Value                  |
		| NewUsername | {NewEmailAddressToken} |
	And I choose ChangeUsernameButton
	Then I am on the VerifyEmailAddressPage page
	When I recieve the update email address verification code
	And I enter data
		| Field               | Value                             |
		| PendingUsernameCode | {NewEmailAddressVerificationCode} |
		| Password            | {PasswordToken}                   |
	And I choose VerifyEmailButton
	Then I am on the LoginPage page
	When I enter data
		| Field        | Value                  |
		| EmailAddress | {NewEmailAddressToken} |
		| Password     | {PasswordToken}        |
	And I choose SignInButton
	Then I am on the MyApplicationsPage page

Scenario: As a candidate I am prompted to verify my username change
	Given I have registered a new candidate
	When I navigate to the SettingsPage page
	And I am on the SettingsPage page
	And I choose ChangeUsernameLink
	Then I am on the UpdateEmailAddressPage page
	When I create a new email address to update my username
	And I enter data
		| Field       | Value                  |
		| NewUsername | {NewEmailAddressToken} |
	And I choose ChangeUsernameButton
	Then I am on the VerifyEmailAddressPage page
	When I recieve the update email address verification code
	And I choose SignoutLink
	Then I am on the LoginPage page
	When I enter data
		| Field        | Value           |
		| EmailAddress | {EmailToken}    |
		| Password     | {PasswordToken} |
	And I choose SignInButton
	Then I am on the MyApplicationsPage page
	And I see
		| Field           | Rule     | Value                     |
		| InfoMessageText | Contains | verify your email address |
