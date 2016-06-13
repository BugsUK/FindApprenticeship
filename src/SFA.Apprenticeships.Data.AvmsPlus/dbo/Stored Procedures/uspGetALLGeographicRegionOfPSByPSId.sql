CREATE PROCEDURE [dbo].[uspGetALLGeographicRegionOfPSByPSId]		 
		@PSId INT
AS
BEGIN
	SET NOCOUNT ON;

SELECT DISTINCT
       TPL.[ProviderSiteLocalAuthorityID] AS 'ProviderLocationID'
      ,PS.ProviderSiteID AS 'ProviderSiteID'      
      ,TPL.[LocalAuthorityId] AS 'LocalAuthorityID'
      ,LAG.[LocalAuthorityGroupID] AS 'GeographicRegionID'
      ,LAG.[CodeName] AS 'GeographicCodeName'
      ,LAG.[ShortName] AS 'GeographicShortName'
      ,LAG.[FullName] AS 'GeographicFullName'      
  FROM [dbo].ProviderSiteLocalAuthority TPL
  JOIN dbo.ProviderSiteRelationship PSR ON TPL.ProviderSiteRelationshipID = PSR.ProviderSiteRelationshipID
  JOIN dbo.ProviderSite PS ON PSR.ProviderSiteID = PS.ProviderSiteID
  INNER JOIN dbo.LocalAuthority LA ON TPL.LocalAuthorityId = LA.LocalAuthorityId
  LEFT JOIN dbo.LocalAuthorityGroupMembership LAGM ON LA.LocalAuthorityId = LAGM.LocalAuthorityID
  LEFT JOIN dbo.LocalAuthorityGroup LAG ON LAGM.LocalAuthorityGroupID = LAG.LocalAuthorityGroupID
  LEFT JOIN dbo.LocalAuthorityGroupType LAGT ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID
  AND LAGT.LocalAuthorityGroupTypeName = N'Region'
  WHERE PS.ProviderSiteID =@PSId  

END