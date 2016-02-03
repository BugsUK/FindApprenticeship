CREATE VIEW [dbo].[vwManagingAreaAndLocalAuthority]
	AS 
	SELECT 
	LAG.LocalAuthorityGroupID AS 'ManagingAreaID', 	
	LAG.CodeName AS 'ManagingAreaCodeName', 
	LAG.ShortName AS 'ManagingAreaShortName', 
	LAG.FullName AS 'ManagingAreaFullName',
	LA.LocalAuthorityId AS 'LocalAuthorityId',
    LA.[CodeName] AS 'LocalAuthorityCodeName',
    LA.[ShortName] AS 'LocalAuthorityShortName',
    LA.[FullName] AS 'LocalAuthorityFullName',   
    LA.[CountyId] AS 'LocalAuthorityCountyId'  
 FROM [LocalAuthorityGroup] LAG
 INNER JOIN dbo.LocalAuthorityGroupType LAGT ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID AND LocalAuthorityGroupTypeName = N'Managing Area'
 INNER JOIN dbo.LocalAuthorityGroupMembership LAGM ON LAGM.LocalAuthorityGroupID = LAG.LocalAuthorityGroupID
 INNER JOIN dbo.LocalAuthority LA ON LA.LocalAuthorityId = LAGM.LocalAuthorityID