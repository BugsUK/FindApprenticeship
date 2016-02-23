CREATE PROCEDURE [dbo].[uspTrainingProviderSelectByUPin]
@upin INT
AS
BEGIN  
  
 SET NOCOUNT ON  
   
 SELECT  
  Upin,  
 [ProviderSite].ProviderSiteID,  
  ProviderSite.TradingName,
  AddressLine1,
  Town
 FROM [dbo].[ProviderSite]
 JOIN ProviderSiteRelationship
 ON ProviderSite.ProviderSiteID = ProviderSiteRelationship.ProviderSiteID
 JOIN ProviderSiteRelationshipType ON ProviderSiteRelationship.ProviderSiteRelationshipTYpeID
 = ProviderSiteRelationshipTYpe.ProviderSiteRelationshipTYpeID and ProviderSiteRelationshipTYpeName = N'Owner'
 JOIN Provider ON ProviderSiteRelationship.ProviderID = Provider.ProviderID         
 WHERE [Upin]=@upin  
 AND [TrainingProviderStatusTypeId] = 1
  
 SET NOCOUNT OFF  
END