CREATE PROCEDURE [dbo].[uspTrainingProviderSelectAllByEdsId]  
 @EdsId int  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
	SELECT  
	 
	 [ProviderSite].[EdsUrn]  AS 'EdsUrn',  
	 [Provider].[UKPRN]  AS 'UKPRN',  
	 [Provider].[UPin]  AS 'UPin',  
	 [ProviderSite].[ProviderSiteID] AS 'TrainingProviderId',  
	 isnull([ProviderSite].[FullName],'') AS 'FullName',  
	 isnull([ProviderSite].[TradingName],'') AS 'TradingName',  
	 isnull([ProviderSite].[OwnerOrganisation],'') as 'OwnerOrgnistaion',  
	 isnull([ProviderSite].[AddressLine1],'') as 'AddressLine1',  
	 isnull([ProviderSite].[AddressLine2],'') as 'AddressLine2',  
	 isnull([ProviderSite].[AddressLine3],'') as 'AddressLine3',  
	 isnull([ProviderSite].[AddressLine4],'') as 'AddressLine4',  
	 isnull([ProviderSite].[Town],'') as 'Town',  
	 [County].[FullName] as 'County',  
	 [ProviderSite].CountyId, 
	 LAG.[FullName] as 'Region',  
	 [ProviderSite].ManagingAreaID as 'RegionId', 
	 [ProviderSite].LocalAuthorityId as 'LocalAuthorityId', 
	 isnull([ProviderSite].[PostCode],'') as 'PostCode',  
	 [ProviderSite].[Longitude] as 'Longitude',  
	 [ProviderSite].[Latitude] as 'Latitude',  
	 [ProviderSite].[GeocodeEasting] as 'GeocodeEasting',  
	 [ProviderSite].[GeocodeNorthing] as 'GeocodeNorthing',
	 [ProviderSite].[EmployerDescription] as 'EmployerDescription',
	 [ProviderSite].[CandidateDescription] as 'CandidateDescription',
	 [ProviderSite].[WebPage] as 'WebPage',
	 [ProviderSite].[OutofDate] as 'OutofDate',
	 [ProviderSite].[ContactDetailsForEmployer] as 'ContactDetailsForEmployer',
	 [ProviderSite].[ContactDetailsForCandidate] as 'ContactDetailsForCandidate',
	 [ProviderSite].[HideFromSearch] as 'HideFromSearch'
	 
	FROM [dbo].[ProviderSite]   
	LEFT JOIN [dbo].[County] [County] ON [County].[CountyId] = [ProviderSite].[CountyId]
	INNER JOIN dbo.LocalAuthorityGroup LAG ON  LAG.LocalAuthorityGroupID = [ProviderSite].ManagingAreaID 
	INNER JOIN dbo.LocalAuthorityGroupType LAGT ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID
	AND LocalAuthorityGroupTypeName = N'Managing Area' 
	 JOIN ProviderSiteRelationship
	 ON ProviderSite.ProviderSiteID = ProviderSiteRelationship.ProviderSiteID
	 JOIN ProviderSiteRelationshipType ON ProviderSiteRelationship.ProviderSiteRelationshipTYpeID
	 = ProviderSiteRelationshipTYpe.ProviderSiteRelationshipTYpeID
	 JOIN Provider ON ProviderSiteRelationship.ProviderID = Provider.ProviderID   
	WHERE [ProviderSite].[EdsUrn]=@EdsId  

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[uspTrainingProviderSelectAllByEdsId] TO [db_executor]
    AS [dbo];

