CREATE VIEW [dbo].[vwRegions]
	AS 
	select 
	LAG.LocalAuthorityGroupID AS 'GeographicRegionID', 
	LAG.CodeName AS 'GeographicCodeName', 
	LAG.ShortName AS 'GeographicShortName', 
	LAG.FullName AS 'GeographicFullName',
	LAG.[LocalAuthorityGroupTypeID] AS 'GeographicTypeId',
    LAG.[LocalAuthorityGroupPurposeID] AS 'GeographicGroupPurposeId'
 from [LocalAuthorityGroup] LAG
JOIN LocalAuthorityGroupType  LAGT
ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID
WHERE  LAGT.LocalAuthorityGroupTypeName='Region'