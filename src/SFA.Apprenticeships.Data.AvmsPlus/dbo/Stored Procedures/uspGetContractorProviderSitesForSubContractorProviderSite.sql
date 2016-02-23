
CREATE PROCEDURE [dbo].[uspGetContractorProviderSitesForSubContractorProviderSite]  
  @providerSiteId int  
AS
 BEGIN
 
	SET NOCOUNT ON
  
	SELECT PS.ProviderSiteID, PS.TradingName
	FROM vwSubContractors vsc
		INNER JOIN ProviderSiteRelationship psr ON psr.ProviderID = vsc.ProviderID AND psr.ProviderSiteRelationShipTypeID = 1
		INNER JOIN ProviderSite ps ON ps.ProviderSiteID = psr.ProviderSiteID
	WHERE vsc.SC_ProviderSiteID = @providerSiteId
	   
 END