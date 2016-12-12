Feature: RA388
	In order to attract more candidates
	As a vacancy manager
	I want to be able to increase the wage of a vacancy

@RA388
Scenario: Get vacancy details without authorization
	When I request the vacancy details for the vacancy with id: 1
	Then The response status is: Unauthorized

@RA388
Scenario: Get vacancy details with an invalid api key
	When I authorize my request with an invalid API key
	And I request the vacancy details for the vacancy with id: 1
	Then The response status is: Unauthorized

@RA388
Scenario: Get vacancy details with an unknown api key
	When I authorize my request with an unknown API key
	And I request the vacancy details for the vacancy with id: 1
	Then The response status is: Unauthorized

@RA388
Scenario: Get vacancy details
	When I authorize my request with a Provider API key
	And I request the vacancy details for the vacancy with id: 1
	Then The response status is: OK
	And I see the vacancy details for the vacancy with id: 1

@RA388
Scenario: Get vacancy details for different provider
	When I authorize my request with a Provider API key
	And I request the vacancy details for the vacancy with id: 2
	Then The response status is: Unauthorized
	And I do not see the vacancy details for the vacancy with id: 2

@RA388
Scenario: Get vacancy that doesn't exist
	When I authorize my request with a Provider API key
	And I request the vacancy details for the vacancy with id: 3
	Then The response status is: NotFound
	And I do not see the vacancy details for the vacancy with id: 3
