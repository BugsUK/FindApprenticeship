CREATE PROCEDURE [dbo].[uspTrainingProviderSelectLocations]
	@ProviderSiteId INT
	,@ProviderSiteRelationTypeId INT
	,@ProviderId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		la.ProviderSiteLocalAuthorityID
		,la.ProviderSiteRelationshipID
		,la.LocalAuthorityId
		,vr.GeographicRegionID
		,vr.GeographicCodeName
		,vr.GeographicShortName
		,vr.GeographicFullName
	FROM dbo.[ProviderSiteLocalAuthority] la
		INNER JOIN ProviderSiteRelationship psr ON
			psr.ProviderSiteRelationshipID = la.ProviderSiteRelationshipID					
		LEFT JOIN vwRegionsAndLocalAuthority vr ON 
			la.LocalAuthorityId = vr.LocalAuthorityId
		WHERE psr.ProviderSiteID = @ProviderSiteId
			AND psr.ProviderSiteRelationShipTypeID = @ProviderSiteRelationTypeId
			AND psr.ProviderID = @ProviderId
END