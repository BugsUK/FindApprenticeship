CREATE VIEW [dbo].[vwAliases]
AS 
select LAG.LocalAuthorityGroupID AS 'AliasID',
	   LAG.CodeName AS 'AliasCodeName', 
	   LAG.ShortName AS 'AliasShortName', 
	   LAG.FullName AS 'AliasFullName',
	   LAG.[LocalAuthorityGroupTypeID] AS 'AliasTypeId',
       LAG.[LocalAuthorityGroupPurposeID] AS 'AliasGroupPurposeId'
	   from [LocalAuthorityGroup] LAG
JOIN LocalAuthorityGroupType  LAGT
ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID
WHERE  LAGT.LocalAuthorityGroupTypeName='Alias'