Feature: RA388
	In order to attract more candidates
	As a vacancy manager
	I want to be able to increase the wage of a vacancy

@RA388
Scenario: Get vacancy details
	When I request the vacancy details for the vacancy with id: 1
	Then The response status is: OK
	And I see the vacancy details for the vacancy with id: 1
