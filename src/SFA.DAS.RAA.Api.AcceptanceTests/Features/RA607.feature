Feature: RA607
	In order to begin the process of creating a vacancy via the API
	As a provider or emloyer
	I want to be able to link an employer to a provider site and specify the vacancies location

@RA607
Scenario: Link an employer to a provider site without authorization
	When I request to link employer identified with EDSURN: 123456789 to provider site identified with EDSURN: 987654321
	Then The response status is: Unauthorized
