CREATE PROCEDURE [dbo].[uspGeographicRegionSelectByLocalAuthorityCode]  
 @LocalAuthCode CHAR(4)
AS  
  
BEGIN  
  
 SET NOCOUNT ON;  
  
SELECT LAG.[LocalAuthorityGroupID]
	FROM [dbo].[LocalAuthorityGroup] LAG	
	INNER JOIN dbo.LocalAuthorityGroupType LAGT ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID AND LocalAuthorityGroupTypeName = N'Region'
	INNER JOIN dbo.LocalAuthorityGroupMembership LAGM ON LAGM.LocalAuthorityGroupID = LAG.LocalAuthorityGroupID
	INNER JOIN dbo.LocalAuthority LA ON LA.LocalAuthorityId = LAGM.LocalAuthorityID	
 WHERE LA.[CodeName] = @LocalAuthCode  
  
END