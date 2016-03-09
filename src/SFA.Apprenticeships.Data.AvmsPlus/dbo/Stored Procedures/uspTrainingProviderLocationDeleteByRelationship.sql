CREATE PROCEDURE [dbo].[uspTrainingProviderLocationDeleteByRelationship]
	@ProviderSiteId INT
	,@ProviderSiteRelationshipTypeId INT
	,@ProviderId INT
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
		AND ProviderSiteId = @ProviderSiteId

    -- Insert statements for procedure here
	DELETE dbo.[ProviderSiteLocalAuthority]
		WHERE [ProviderSiteRelationshipID] = @ProviderSiteRelationshipId
END