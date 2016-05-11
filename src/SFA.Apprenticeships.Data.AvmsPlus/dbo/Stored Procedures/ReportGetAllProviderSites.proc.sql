CREATE PROCEDURE [dbo].[ReportGetAllProviderSites]
AS
SET NOCOUNT ON;
SELECT ProviderSiteID, TradingName
FROM dbo.ProviderSite
UNION SELECT -1, 'All'