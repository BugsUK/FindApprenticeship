CREATE PROCEDURE [dbo].[uspTrainingProviderFrameworkInsert]
	@ProviderSiteId INT
	,@ProviderSiteRelationshipTypeId INT
	,@ProviderId INT
	,@FrameworkId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @ProviderSiteRelationshipId int
	SELECT @ProviderSiteRelationshipId = ProviderSiteRelationshipID
	FROM dbo.ProviderSiteRelationship
	WHERE ProviderID = @ProviderId
		AND ProviderSiteRelationShipTypeID = @ProviderSiteRelationshipTypeId
		AND ProviderSiteID = @ProviderSiteId
	
    -- Insert statements for procedure here
	INSERT INTO dbo.[ProviderSiteFramework] (
		ProviderSiteRelationshipID,
		FrameworkId
		--TrainingProviderSectorId
	) VALUES ( 
		@ProviderSiteRelationshipId,
		@FrameworkId
		)
END