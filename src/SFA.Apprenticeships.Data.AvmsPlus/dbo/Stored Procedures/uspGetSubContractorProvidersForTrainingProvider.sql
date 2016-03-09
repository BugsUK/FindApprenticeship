CREATE PROCEDURE [dbo].[uspGetSubContractorProvidersForTrainingProvider]
	@ProviderId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from              
	-- interfering with SELECT statements.              
	SET NOCOUNT ON;              
 
	
	SELECT
		vsc.SC_ProviderID
		,vsc.SC_UPIN
		, CASE  WHEN p.ProviderStatusTypeID = 3 THEN 1 ELSE 0 END AS IsSuspended
		,COALESCE(p.TradingName, MIN(ps.TradingName)) as TradingName
		,Count(ps.ProviderSiteID) as NumSites
	
	FROM
		vwSubContractors vsc
			INNER JOIN ProviderSite ps ON ps.ProviderSiteID = vsc.SC_ProviderSiteID	
			INNER JOIN ProviderSiteRelationship psr ON psr.ProviderSiteID = vsc.SC_ProviderSiteID AND psr.ProviderSiteRelationShipTypeID = 1
			INNER JOIN Provider p ON psr.ProviderID = p.ProviderID
	WHERE 
		vsc.ProviderID = @ProviderId 
	GROUP BY vsc.SC_ProviderID, vsc.SC_UPIN, p.TradingName, p.ProviderStatusTypeID	  
	
END