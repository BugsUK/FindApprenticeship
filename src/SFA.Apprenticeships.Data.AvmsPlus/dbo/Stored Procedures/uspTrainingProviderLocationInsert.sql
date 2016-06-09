CREATE PROCEDURE [dbo].[uspTrainingProviderLocationInsert]
	@ProviderSiteId INT
	,@ProviderSiteRelationshipTypeId INT
	,@ProviderId INT
	,@LocalAuthorityId INT
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

	-- handle national locations
	IF @LocalAuthorityId IS NULL
	BEGIN
		DELETE FROM ProviderSiteLocalAuthority
		WHERE ProviderSiteRelationshipID = @ProviderSiteRelationshipId

		INSERT INTO ProviderSiteLocalAuthority
		( ProviderSiteRelationshipID, LocalAuthorityId)
		VALUES 
		(@ProviderSiteRelationshipId, NULL)
	END
	ELSE
	BEGIN
		IF NOT EXISTS (SELECT 1 FROM ProviderSiteLocalAuthority WHERE ProviderSiteRelationshipID=@ProviderSiteRelationshipID AND LocalAuthorityId=@LocalAuthorityId)
		BEGIN
			INSERT INTO ProviderSiteLocalAuthority
			( ProviderSiteRelationshipID, LocalAuthorityId)
			VALUES 
			(@ProviderSiteRelationshipId, @LocalAuthorityId)
		END
	END

END