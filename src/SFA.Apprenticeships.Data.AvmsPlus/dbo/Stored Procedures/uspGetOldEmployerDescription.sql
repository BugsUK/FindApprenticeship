CREATE PROCEDURE [dbo].[uspGetOldEmployerDescription]
	@employerId INT,
	@trainingProviderId INT
AS
BEGIN
	SELECT
		EmployerDescription
	FROM
		[VacancyOwnerRelationship]
	WHERE
		EmployerId = @employerId
	AND
		[ProviderSiteID] = @trainingProviderId
END