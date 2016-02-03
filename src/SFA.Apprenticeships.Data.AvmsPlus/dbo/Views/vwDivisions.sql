CREATE VIEW [dbo].[vwDivisions]
	AS 
	select 
	LAG.LocalAuthorityGroupID AS 'DivisionID', 
	LAG.CodeName AS 'DivisionCode', 
	LAG.ShortName AS 'DivisionShortName', 
	LAG.FullName AS 'DivisionFullName',
	LAG.[LocalAuthorityGroupTypeID] AS 'DivisionTypeId',
    LAG.[LocalAuthorityGroupPurposeID] AS 'DivisionGroupPurposeId'
 from [LocalAuthorityGroup] LAG
JOIN LocalAuthorityGroupType  LAGT
ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID
WHERE  LAGT.LocalAuthorityGroupTypeName='Division'