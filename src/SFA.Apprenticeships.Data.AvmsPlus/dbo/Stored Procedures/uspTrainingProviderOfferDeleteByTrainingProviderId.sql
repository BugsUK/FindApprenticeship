CREATE PROCEDURE [dbo].[uspTrainingProviderOfferDeleteByTrainingProviderId]
	 @ProviderSiteId INT
	,@ProviderSiteRelationshipTypeId INT
	,@ProviderId INT
AS
BEGIN
	SET NOCOUNT ON
	
	DECLARE @ProviderSiteRelationshipId int
	SELECT @ProviderSiteRelationshipId = ProviderSiteRelationshipID
	FROM dbo.ProviderSiteRelationship
	WHERE ProviderID = @ProviderId
		AND ProviderSiteRelationShipTypeID = @ProviderSiteRelationshipTypeId
		AND ProviderSiteId = @ProviderSiteId

    DELETE FROM [dbo].[ProviderSiteOffer]
    WHERE ProviderSiteOfferID IN (
		SELECT tpo.ProviderSiteOfferID
		FROM [dbo].[ProviderSiteOffer] tpo
			INNER JOIN [dbo].[ProviderSiteLocalAuthority] tpl ON tpo.ProviderSiteLocalAuthorityID = tpl.ProviderSiteLocalAuthorityID
			INNER JOIN [dbo].[ProviderSiteFramework] tpf ON tpo.[ProviderSiteFrameworkID] = tpf.ProviderSiteFrameworkID
		WHERE tpl.[ProviderSiteRelationshipID] = @ProviderSiteRelationshipId
			AND   tpf.ProviderSiteRelationshipID = @ProviderSiteRelationshipId
    )
    
    SET NOCOUNT OFF
END