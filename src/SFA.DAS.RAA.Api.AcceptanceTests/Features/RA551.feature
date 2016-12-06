Feature: RA-551
	In order to begin work on the RAA API
	As an API user
	I want to confirm the API responds to basic requests

@RA551
Scenario: View swagger homepage
	When I request the swagger homepage
	Then I should see the swagger homepage

@RA551
Scenario: View swagger docs
	When I request the swagger docs
	Then I should see the swagger docs
