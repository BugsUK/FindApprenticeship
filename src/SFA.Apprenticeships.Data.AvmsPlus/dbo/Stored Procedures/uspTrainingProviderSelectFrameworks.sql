CREATE PROCEDURE [dbo].[uspTrainingProviderSelectFrameworks]
	@ProviderSiteId INT
	,@ProviderSiteRelationTypeId INT
	,@ProviderId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
		
		SELECT [ProviderSiteFramework].ProviderSiteFrameworkID,[ProviderSiteFramework].
			ProviderSiteRelationshipID,[ProviderSiteFramework].FrameworkId,
			--IsNull(ApprenticeshipFramework.SectorId,0) as TrainingProviderSectorId,
			IsNull(ApprenticeshipFramework.ApprenticeshipOccupationId,0) as ApprenticeshipOccupationId
			FROM dbo.[ProviderSiteFramework]
		INNER JOIN ApprenticeshipFramework ON
			ApprenticeshipFramework.ApprenticeshipFrameworkId = [ProviderSiteFramework].FrameworkId					
		INNER JOIN ProviderSiteRelationship ON
			ProviderSiteRelationship.ProviderSiteRelationshipID = [ProviderSiteFramework].ProviderSiteRelationshipID					
		WHERE ProviderSiteRelationship.ProviderSiteID = @ProviderSiteId
			AND ProviderSiteRelationship.ProviderSiteRelationShipTypeID = @ProviderSiteRelationTypeId
			AND ProviderSiteRelationship.ProviderID = @ProviderId
		ORDER BY ApprenticeshipFramework.FullName ASC
END