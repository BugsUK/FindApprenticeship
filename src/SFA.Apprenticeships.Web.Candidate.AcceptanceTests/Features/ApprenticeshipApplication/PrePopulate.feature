﻿Feature: Apprenticeship Application Pre Populate
	In order to speed up the application process
	As a candidate
	I want valid data I have previously entered to pre populate the application form

Background: 
	Given I navigated to the ApprenticeshipSearchPage page
	And I am logged out
	And I navigated to the ApprenticeshipSearchPage page
	Then I am on the ApprenticeshipSearchPage page

@US461 @PrimaryTransaction
Scenario: Pre-populate my Education Qualifications Work Experience About You details
	Given I have registered a new candidate
	When I select the "first" apprenticeship vacancy in location "N7 8LS" that can apply by this website
	Then I am on the ApprenticeshipDetailsPage page
	When I choose ApplyButton
	Then I am on the ApprenticeshipApplicationPage page
	And I see
		| Field                   | Rule   | Value              |
		#Should have been filled in automatically from personal details
		| FullnameReadOnly        | Equals | Firstname Lastname |
		| EmailReadOnly           | Equals | {EmailToken}       |
		| DobReadOnly             | Equals | 01/01/2000         |
		| PhoneReadOnly           | Equals | 07469984649        |
		| AddressLine1ReadOnly    | Equals | 6 Furlong Road     |
		| AddressLine2ReadOnly    | Equals |                    |
		| AddressLine3ReadOnly    | Equals | London             |
		| AddressLine4ReadOnly    | Equals | London             |
		| AddressPostcodeReadOnly | Equals | N7 8LS             |
		#Should not be filled in as no previous application has been submitted
		| EducationNameOfSchool   | Equals |                    |
		| EducationFromYear       | Equals |                    |
		| EducationToYear         | Equals |                    |
		| WhatAreYourStrengths    | Equals |                    |
		| WhatCanYouImprove       | Equals |                    |
	When I enter data
		| Field                   | Value                         |
		| EducationNameOfSchool   | SchoolName                    |
		| EducationFromYear       | 2010                          |
		| EducationToYear         | 2012                          |
		| WhatAreYourStrengths    | My strengths                  |
		| WhatCanYouImprove       | What can I improve            |
		| HobbiesAndInterests     | Hobbies and interests         |
	And I choose SaveButton
	Then I wait to see ApplicationSavedMessage
	And I see
		| Field                   | Rule      | Value           |
		| ApplicationSavedMessage | Ends With | my applications |
	When I select the "second" apprenticeship vacancy in location "N7 8LS" that can apply by this website
	Then I am on the ApprenticeshipDetailsPage page
	When I choose ApplyButton
	Then I am on the ApprenticeshipApplicationPage page
	And I see
		| Field                 | Rule   | Value                 |
		| EducationNameOfSchool | Equals | SchoolName            |
		| EducationFromYear     | Equals | 2010                  |
		| EducationToYear       | Equals | 2012                  |
		| WhatAreYourStrengths  | Equals | My strengths          |
		| WhatCanYouImprove     | Equals | What can I improve    |
		| HobbiesAndInterests   | Equals | Hobbies and interests |