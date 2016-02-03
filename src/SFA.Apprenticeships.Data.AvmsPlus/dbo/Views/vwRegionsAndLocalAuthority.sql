CREATE VIEW [dbo].[vwRegionsAndLocalAuthority]
	AS 
	SELECT 
	LAG.LocalAuthorityGroupID AS 'GeographicRegionID', 	
	LAG.CodeName AS 'GeographicCodeName', 
	LAG.ShortName AS 'GeographicShortName', 
	LAG.FullName AS 'GeographicFullName',
	LA.LocalAuthorityId AS 'LocalAuthorityId',
    LA.[CodeName] AS 'LocalAuthorityCodeName',
    LA.[ShortName] AS 'LocalAuthorityShortName',
    LA.[FullName] AS 'LocalAuthorityFullName',   
    LA.[CountyId] AS 'LocalAuthorityCountyId'  
 FROM [LocalAuthorityGroup] LAG
 INNER JOIN dbo.LocalAuthorityGroupType LAGT ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID AND LocalAuthorityGroupTypeName = N'Region'
 INNER JOIN dbo.LocalAuthorityGroupMembership LAGM ON LAGM.LocalAuthorityGroupID = LAG.LocalAuthorityGroupID
 INNER JOIN dbo.LocalAuthority LA ON LA.LocalAuthorityId = LAGM.LocalAuthorityID