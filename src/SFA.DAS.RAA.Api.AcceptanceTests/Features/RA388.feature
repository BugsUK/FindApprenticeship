Feature: RA388
	In order to attract more candidates
	As a vacancy manager
	I want to be able to increase the wage of a vacancy

@RA388 @GetVacancy
Scenario: Get vacancy details with no identifier
	When I authorize my request with a Provider API key
	And I request the vacancy details for the vacancy with no identifier
	Then The response status is: NotFound
	And I do not see the vacancy details for the vacancy with no identifier

@RA388 @GetVacancyById
Scenario: Get vacancy details by id without authorization
	When I request the vacancy details for the vacancy with id: 1
	Then The response status is: Unauthorized

@RA388 @GetVacancyById
Scenario: Get vacancy details by id with an invalid api key
	When I authorize my request with an invalid API key
	And I request the vacancy details for the vacancy with id: 1
	Then The response status is: Unauthorized

@RA388 @GetVacancyById
Scenario: Get vacancy details by id with an unknown api key
	When I authorize my request with an unknown API key
	And I request the vacancy details for the vacancy with id: 1
	Then The response status is: Unauthorized

@RA388 @GetVacancyById
Scenario: Get vacancy details by id
	When I authorize my request with a Provider API key
	And I request the vacancy details for the vacancy with id: 1
	Then The response status is: OK
	And I see the vacancy details for the vacancy with id: 1

@RA388 @GetVacancyById
Scenario: Get vacancy details by id for different provider
	When I authorize my request with a Provider API key
	And I request the vacancy details for the vacancy with id: 2
	Then The response status is: Unauthorized with response message: You are not authorized to view or edit this vacancy.
	And I do not see the vacancy details for the vacancy with id: 2

@RA388 @GetVacancyById
Scenario: Get vacancy by id that doesn't exist
	When I authorize my request with a Provider API key
	And I request the vacancy details for the vacancy with id: 3
	Then The response status is: NotFound with response message: The requested vacancy has not been found.
	And I do not see the vacancy details for the vacancy with id: 3

@RA388 @GetVacancyByReferenceNumber
Scenario: Get vacancy details by reference number without authorization
	When I request the vacancy details for the vacancy with reference number: 1
	Then The response status is: Unauthorized

@RA388 @GetVacancyByReferenceNumber
Scenario: Get vacancy details by reference number with an invalid api key
	When I authorize my request with an invalid API key
	And I request the vacancy details for the vacancy with reference number: 1
	Then The response status is: Unauthorized

@RA388 @GetVacancyByReferenceNumber
Scenario: Get vacancy details by reference number with an unknown api key
	When I authorize my request with an unknown API key
	And I request the vacancy details for the vacancy with reference number: 1
	Then The response status is: Unauthorized

@RA388 @GetVacancyByReferenceNumber
Scenario: Get vacancy details by reference number
	When I authorize my request with a Provider API key
	And I request the vacancy details for the vacancy with reference number: 1
	Then The response status is: OK
	And I see the vacancy details for the vacancy with reference number: 1

@RA388 @GetVacancyByReferenceNumber
Scenario: Get vacancy details by reference number for different provider
	When I authorize my request with a Provider API key
	And I request the vacancy details for the vacancy with reference number: 2
	Then The response status is: Unauthorized
	And I do not see the vacancy details for the vacancy with reference number: 2

@RA388 @GetVacancyByReferenceNumber
Scenario: Get vacancy by reference number that doesn't exist
	When I authorize my request with a Provider API key
	And I request the vacancy details for the vacancy with reference number: 3
	Then The response status is: NotFound
	And I do not see the vacancy details for the vacancy with reference number: 3

@RA388 @GetVacancyByGuid
Scenario: Get vacancy details by guid without authorization
	When I request the vacancy details for the vacancy with guid: 1
	Then The response status is: Unauthorized

@RA388 @GetVacancyByGuid
Scenario: Get vacancy details by guid with an invalid api key
	When I authorize my request with an invalid API key
	And I request the vacancy details for the vacancy with guid: 1
	Then The response status is: Unauthorized

@RA388 @GetVacancyByGuid
Scenario: Get vacancy details by guid with an unknown api key
	When I authorize my request with an unknown API key
	And I request the vacancy details for the vacancy with guid: 1
	Then The response status is: Unauthorized

@RA388 @GetVacancyByGuid
Scenario: Get vacancy details by guid
	When I authorize my request with a Provider API key
	And I request the vacancy details for the vacancy with guid: 1
	Then The response status is: OK
	And I see the vacancy details for the vacancy with guid: 1

@RA388 @GetVacancyByGuid
Scenario: Get vacancy details by guid for different provider
	When I authorize my request with a Provider API key
	And I request the vacancy details for the vacancy with guid: 2
	Then The response status is: Unauthorized
	And I do not see the vacancy details for the vacancy with guid: 2

@RA388 @GetVacancyByGuid
Scenario: Get vacancy by guid that doesn't exist
	When I authorize my request with a Provider API key
	And I request the vacancy details for the vacancy with guid: 3
	Then The response status is: NotFound
	And I do not see the vacancy details for the vacancy with guid: 3

@RA388 @EditWage
Scenario: Increase fixed wage by £20 per week without authorization
	Given I have a Live Apprenticeship vacancy with id: 42, a fixed wage of £200 Weekly
	When I request to change the wage for the vacancy with id: 42 to Custom £220 Weekly
	Then The response status is: Unauthorized with response message: Authorization has been denied for this request.
	And I do not see the edited vacancy wage details for the vacancy with id: 42

@RA388 @EditWage
Scenario: Increase fixed wage by £20 per week on a traineeship vacancy
	Given I have a Live Traineeship vacancy with id: 42, a fixed wage of £200 Weekly
	When I authorize my request with a Provider API key
	And I request to change the wage for the vacancy with id: 42 to Custom £220 Weekly
	Then The response status is: BadRequest with response message: You can only edit the wage of an Apprenticeship vacancy.
	And I do not see the edited vacancy wage details for the vacancy with id: 42

@RA388 @EditWage
Scenario: Increase fixed wage by £20 per week on a submitted vacancy
	Given I have a Submitted Apprenticeship vacancy with id: 42, a fixed wage of £200 Weekly
	When I authorize my request with a Provider API key
	And I request to change the wage for the vacancy with id: 42 to Custom £220 Weekly
	Then The response status is: BadRequest
	And I do not see the edited vacancy wage details for the vacancy with id: 42

@RA388 @EditWage
Scenario: Increase fixed wage by £20 per week on a archived vacancy
	Given I have a Completed Apprenticeship vacancy with id: 42, a fixed wage of £200 Weekly
	When I authorize my request with a Provider API key
	And I request to change the wage for the vacancy with id: 42 to Custom £220 Weekly
	Then The response status is: BadRequest with response message: You can only edit the wage of a vacancy that is live or closed.
	And I do not see the edited vacancy wage details for the vacancy with id: 42

@RA388 @EditWage
Scenario: Increase fixed wage by £20 per week
	Given I have a Live Apprenticeship vacancy with id: 42, a fixed wage of £200 Weekly
	When I authorize my request with a Provider API key
	And I request to change the wage for the vacancy with id: 42 to Custom £220 Weekly
	Then The response status is: OK
	And I see that the wage details for the vacancy with id: 42 have been updated

@RA388 @EditWage
Scenario: Change fixed wage to wage range and increase by £20 per week
	Given I have a Live Apprenticeship vacancy with id: 42, a fixed wage of £200 Weekly
	When I authorize my request with a Provider API key
	And I request to change the wage for the vacancy with id: 42 to CustomRange £220 - £240 Weekly
	Then The response status is: OK
	And I see that the wage details for the vacancy with id: 42 have been updated

@RA388 @EditWage
Scenario: Decrease fixed wage by £20 per week
	Given I have a Live Apprenticeship vacancy with id: 42, a fixed wage of £200 Weekly
	When I authorize my request with a Provider API key
	And I request to change the wage for the vacancy with id: 42 to Custom £180 Weekly
	Then The response status is: BadRequest
	And The validation errors contain:
		| Property | Error                                            |
		| Amount   | The new fixed wage must be higher than the original figure. |
	And I do not see the edited vacancy wage details for the vacancy with id: 42

@RA388 @EditWage
Scenario: Change fixed wage to national minumum wage
	Given I have a Live Apprenticeship vacancy with id: 42, a fixed wage of £200 Weekly
	When I authorize my request with a Provider API key
	And I request to change the wage for the vacancy with id: 42 to NationalMinimum
	Then The response status is: BadRequest
	And The validation errors contain:
		| Property | Error                                                                              |
		| Type     | You can only change the type of a Custom (fixed) wage to CustomRange (wage range). |
	And I do not see the edited vacancy wage details for the vacancy with id: 42