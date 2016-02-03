CREATE PROCEDURE [dbo].[ReportGetAllProvidersForEmployer](@EmployerID int)
AS
select TP.ProviderSIteID, TP.TradingName
from dbo.ProviderSIte TP
INNER JOIN dbo.VacancyOwnerRelationship VPR 
ON TP.PRoviderSIteID = VPR.ProviderSIteID
AND VPR.EmployerId = @EmployerID
UNION SELECT -1, N'All'
;