CREATE PROCEDURE [dbo].[uspTrainingProviderSelectOffers]
	@ProviderSiteId INT
	,@ProviderSiteRelationTypeId INT
	,@ProviderId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT  [ProviderSiteOffer].ProviderSiteOfferID,
			[ProviderSiteOffer].ProviderSiteLocalAuthorityID,
			[ProviderSiteOffer].[ProviderSiteFrameworkID],
			[ProviderSiteOffer].Apprenticeship,
			[ProviderSiteOffer].AdvancedApprenticeship,
			[ProviderSiteOffer].HigherApprenticeship
		 FROM [ProviderSiteOffer]
			INNER JOIN [ProviderSiteLocalAuthority] 
				ON [ProviderSiteLocalAuthority].ProviderSiteLocalAuthorityID = [ProviderSiteOffer].ProviderSiteLocalAuthorityID
			INNER JOIN ProviderSiteRelationship ON
				[ProviderSiteLocalAuthority].ProviderSiteRelationshipID = ProviderSiteRelationship.ProviderSiteRelationshipID	
		WHERE ProviderSiteRelationship.ProviderSiteID = @ProviderSiteId
			AND ProviderSiteRelationship.ProviderSiteRelationShipTypeID = @ProviderSiteRelationTypeId
			AND ProviderSiteRelationship.ProviderID = @ProviderId

	UNION

	SELECT  [ProviderSiteOffer].ProviderSiteOfferID,
			[ProviderSiteOffer].ProviderSiteLocalAuthorityID,
			[ProviderSiteOffer].[ProviderSiteFrameworkID],
			[ProviderSiteOffer].Apprenticeship,
			[ProviderSiteOffer].AdvancedApprenticeship,
			[ProviderSiteOffer].HigherApprenticeship
		 FROM [ProviderSiteOffer]
			INNER JOIN [ProviderSiteFramework]
				ON [ProviderSiteFramework].ProviderSiteFrameworkID = [ProviderSiteOffer].[ProviderSiteFrameworkID]
			INNER JOIN ProviderSiteRelationship ON
				[ProviderSiteFramework].ProviderSiteRelationshipID = ProviderSiteRelationship.ProviderSiteRelationshipID	
		WHERE ProviderSiteRelationship.ProviderSiteID = @ProviderSiteId
			AND ProviderSiteRelationship.ProviderSiteRelationShipTypeID = @ProviderSiteRelationTypeId
			AND ProviderSiteRelationship.ProviderID = @ProviderId
END