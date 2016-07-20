CREATE PROCEDURE [dbo].[ReportGetAllEmployers]
AS
SET NOCOUNT ON;
SELECT EmployerID, TradingName
FROM dbo.Employer
UNION SELECT -1, 'All';