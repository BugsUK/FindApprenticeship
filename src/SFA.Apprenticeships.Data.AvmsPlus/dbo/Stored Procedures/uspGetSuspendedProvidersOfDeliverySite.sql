CREATE PROCEDURE [dbo].[uspGetSuspendedProvidersOfDeliverySite]
	@providerSiteId int
AS
BEGIN
SET NOCOUNT ON

SELECT          
	P.ProviderID
	,P.UPIN
	,P.UKPRN
	,P.FullName
	,P.TradingName
	,P.IsContracted

  FROM Provider P
INNER JOIN ProviderSiteRelationship R ON R.ProviderId = P.ProviderId AND R.ProviderSiteRelationShipTypeID = 2
AND R.ProviderSiteID = @providerSiteId
WHERE P.ProviderStatusTypeId = 3

  SET NOCOUNT OFF
END