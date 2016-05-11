CREATE PROCEDURE dbo.ReportGetDisplayNames
AS
SET NOCOUNT ON;
SELECT DISTINCT DisplayName, RdlName FROM 

(SELECT DisplayName, HTMLVersion, CSVVersion, SummaryVersion FROM reportdefinitions ) a
UNPIVOT
(RdlName FOR Type IN ([HTMLVersion],[CSVVersion],[SummaryVersion]))
as b;
