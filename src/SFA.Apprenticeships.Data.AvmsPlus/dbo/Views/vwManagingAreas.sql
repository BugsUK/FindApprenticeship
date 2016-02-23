CREATE VIEW [dbo].[vwManagingAreas]
	AS
	 SELECT 
	   LAG.[LocalAuthorityGroupID] AS 'ManagingAreaId'  
      ,LAG.[CodeName]  AS 'ManagingAreaCodeName'
      ,LAG.[ShortName] AS 'ManagingAreaShortName' 
      ,LAG.[FullName] AS 'ManagingAreaFullName'
      ,LAG.[LocalAuthorityGroupTypeID] AS 'ManagingAreaTypeId'
      ,LAG.[LocalAuthorityGroupPurposeID] AS 'ManagingAreaGroupPurposeId'
      ,Division.[LocalAuthorityGroupID] AS 'DivisionId'  
      ,Division.[FullName] AS 'DivisionName'  
  FROM [LocalAuthorityGroup] LAG  
  LEFT OUTER  JOIN [LocalAuthorityGroup] Division ON Division.[LocalAuthorityGroupID] = LAG.[ParentLocalAuthorityGroupID]  
  JOIN LocalAuthorityGroupType  LAGT
  ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID
  AND   LAGT.LocalAuthorityGroupTypeName='Managing Area'