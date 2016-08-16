CREATE PROCEDURE dbo.ReportGetEthnicOrigin
AS
SET NOCOUNT ON;
SELECT CandidateEthnicOriginID, CASE WHEN FullName = 'Please Select' Then ShortName Else FullName END AS FullName
FROM dbo.CandidateEthnicOrigin
Where CandidateEthnicOriginId != 0
UNION SELECT -1, 'All'