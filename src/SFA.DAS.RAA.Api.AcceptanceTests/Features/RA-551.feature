Feature: RA-551
	In order to begin work on the RAA API
	As an API user
	I want to confirm the API responds to basic requests

@RA-551
Scenario: View swagger homepage
	When I request the swagger homepage
	Then I should see the swagger homepage
