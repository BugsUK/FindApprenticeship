CREATE PROCEDURE [dbo].[uspTrainingProviderOfferDeleteByTrainingProviderIdAndGeographicRegionId]
	 @ProviderSiteId INT
	 ,@ProviderSiteRelationshipTypeId INT
	 ,@ProviderId INT
	 ,@GeographicRegionId INT
AS
BEGIN
	SET NOCOUNT ON
	
	DECLARE @ProviderSiteRelationshipId int
	SELECT @ProviderSiteRelationshipId = ProviderSiteRelationshipID
	FROM dbo.ProviderSiteRelationship
	WHERE ProviderID = @ProviderId
		AND ProviderSiteRelationShipTypeID = @ProviderSiteRelationshipTypeId
		AND ProviderSiteID = @ProviderSiteId


    DELETE FROM [dbo].[ProviderSiteOffer]
		WHERE ProviderSiteOfferID IN 
			(
				SELECT dbo.[ProviderSiteOffer].ProviderSiteOfferID
					FROM dbo.[ProviderSiteOffer] 
						INNER JOIN dbo.[ProviderSiteLocalAuthority] ON dbo.[ProviderSiteOffer].ProviderSiteLocalAuthorityID = dbo.[ProviderSiteLocalAuthority].ProviderSiteLocalAuthorityID
						INNER JOIN vwRegionsAndLocalAuthority vr ON [ProviderSiteLocalAuthority].LocalAuthorityId = vr.LocalAuthorityId
					WHERE (dbo.[ProviderSiteLocalAuthority].[ProviderSiteRelationshipID] = @ProviderSiteRelationshipId) 
						AND (vr.GeographicRegionID = @GeographicRegionId)
			)
    
    SET NOCOUNT OFF
END