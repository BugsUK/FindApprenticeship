CREATE PROCEDURE dbo.ReportGetManagedBy
AS
BEGIN
SET NOCOUNT ON;

SELECT CodeName, FullName
FROM dbo.LocalAuthorityGroup
JOIN dbo.LocalAuthorityGroupPurpose
ON dbo.LocalAuthorityGroup.LocalAuthorityGroupPurposeID = dbo.LocalAuthorityGroupPurpose.LocalAuthorityGroupPurposeID
AND LocalAuthorityGroupPurposeName = 'Management'
UNION SELECT '-1', 'All'
;
END