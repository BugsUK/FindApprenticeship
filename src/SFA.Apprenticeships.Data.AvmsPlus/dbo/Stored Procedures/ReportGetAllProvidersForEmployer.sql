CREATE PROCEDURE [dbo].[ReportGetAllProvidersForEmployer](@EmployerID int)
AS
select TP.ProviderSiteID, TP.TradingName
from dbo.ProviderSite TP
INNER JOIN dbo.VacancyOwnerRelationship VPR 
ON TP.ProviderSiteID = VPR.ProviderSiteID
AND VPR.EmployerId = @EmployerID
UNION SELECT -1, N'All'
;