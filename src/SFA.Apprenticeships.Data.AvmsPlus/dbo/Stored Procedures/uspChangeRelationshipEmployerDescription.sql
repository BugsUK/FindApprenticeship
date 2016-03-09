CREATE PROCEDURE [dbo].[uspChangeRelationshipEmployerDescription] 
	@oldTrainingProviderId INT,
	@oldEmployerId INT,
	@relationshipId INT
AS
BEGIN
	DECLARE @message nvarchar(MAX)
	
	--Get old employer description
	SELECT
		@message = EmployerDescription
	FROM
		[VacancyOwnerRelationship]
	WHERE
		EmployerId = @oldEmployerId
	AND
		[ProviderSiteID] = @oldTrainingProviderId
		
	--Now update with new description	
	UPDATE 
		[VacancyOwnerRelationship]
	SET
		EmployerDescription = @message
	WHERE
		[VacancyOwnerRelationshipId] = @relationshipId
END