CREATE FUNCTION dbo.ReportGetChildManagingAreas (@localAuthorityGroupCodeName nvarchar(3))
RETURNS TABLE AS RETURN
WITH r AS 
(SELECT LocalAuthorityGroupID
FROM dbo.LocalAuthorityGroup
WHERE CodeName = @localAuthorityGroupCodeName
UNION ALL
SELECT t.LocalAuthorityGroupID
FROM dbo.LocalAuthorityGroup t JOIN r 
	ON t.ParentLocalAuthorityGroupID = r.LocalAuthorityGroupID)
	
SELECT r.LocalAuthorityGroupID FROM r
JOIN dbo.LocalAuthorityGroup ON r.LocalAuthorityGroupID = dbo.LocalAuthorityGroup.LocalAuthorityGroupID
JOIN dbo.LocalAuthorityGroupType ON dbo.LocalAuthorityGroup.LocalAuthorityGroupTypeID = dbo.LocalAuthorityGroupType.LocalAuthorityGroupTypeID
WHERE LocalAuthorityGroupTypeName = N'Managing Area'