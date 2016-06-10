CREATE PROCEDURE [dbo].[uspTEMPGetTrainingProviderPRN]    
 @TrainingProviderId int    
AS  
BEGIN  
 SET NOCOUNT ON  
  
 SELECT   
 [Provider].[UPIN] AS 'UKPRN'  
 FROM [dbo].[ProviderSite] 
 JOIN ProviderSiteRelationship
 ON ProviderSite.ProviderSiteID = ProviderSiteRelationship.ProviderSiteID
 JOIN ProviderSiteRelationshipType ON ProviderSiteRelationship.ProviderSiteRelationshipTYpeID
 = ProviderSiteRelationshipTYpe.ProviderSiteRelationshipTYpeID and ProviderSiteRelationshipTYpeName = N'Owner'
 JOIN Provider ON ProviderSiteRelationship.ProviderID = Provider.ProviderID  
 WHERE   
 [ProviderSite].ProviderSiteID = @TrainingProviderId  
  
 SET NOCOUNT OFF   
END