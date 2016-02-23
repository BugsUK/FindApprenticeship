CREATE PROCEDURE [dbo].[uspTrainingProviderSelectFrameworksById]
	@ProviderSiteId INT
	,@ProviderSiteRelationTypeId INT
	,@ProviderId INT
	,@OccupationId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [ProviderSiteFramework].ProviderSiteFrameworkID,[ProviderSiteFramework].
		ProviderSiteRelationshipID,[ProviderSiteFramework].FrameworkId,
		--ISNULL([ProviderSiteFramework].TrainingProviderSectorId,0) as TrainingProviderSectorId ,
		ApprenticeshipFramework.ApprenticeshipOccupationId,
		ApprenticeshipFramework.FullName
	FROM dbo.[ProviderSiteFramework]
		INNER JOIN ApprenticeshipFramework ON
			ApprenticeshipFramework.ApprenticeshipFrameworkId = [ProviderSiteFramework].FrameworkId					
		INNER JOIN ProviderSiteRelationship ON
			ProviderSiteRelationship.ProviderSiteRelationshipID = [ProviderSiteFramework].ProviderSiteRelationshipID	
	WHERE ProviderSiteRelationship.ProviderSiteID = @ProviderSiteId
		AND ProviderSiteRelationship.ProviderSiteRelationShipTypeID = @ProviderSiteRelationTypeId
		AND ProviderSiteRelationship.ProviderID = @ProviderId
		AND ApprenticeshipFramework.ApprenticeshipOccupationId = @OccupationId
	ORDER BY ApprenticeshipFramework.FullName ASC

END