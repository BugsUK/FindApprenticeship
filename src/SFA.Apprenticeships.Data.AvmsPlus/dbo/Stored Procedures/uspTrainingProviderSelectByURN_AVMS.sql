CREATE PROCEDURE [dbo].[uspTrainingProviderSelectByURN_AVMS]                                    
(                             
@edsurn INT
)                              
AS                                
BEGIN                                
                            
SET NOCOUNT ON                                
          
SELECT                    
 isnull([ProviderSite].[FullName],'') AS 'FullName',                    
 [ProviderSite].[OutofDate] AS 'OutofDate',                    
 isnull([ProviderSite].[TradingName],'') AS 'TradingName',                    
 [ProviderSite].ProviderSIteID AS 'TrainingProviderId',                    
 [Provider].[UPIN] AS 'UPIN',                  
 isnull([AddressLine1],'') AS 'AddressLine1',                      
 isnull([AddressLine2],'') AS 'AddressLine2',                      
 isnull([AddressLine3],'') AS 'AddressLine3',                  
 isnull([AddressLine4],'') AS 'AddressLine4',                     
 [County].[FullName] AS 'County',                      
 isnull([Postcode],'') AS 'Postcode',                      
 isnull([Town],'') AS 'Town',          
 LAG.[FullName] AS 'Region',      
[ProviderSite].[CountyId],      
[ProviderSite].ManagingAreaID,    
[ProviderSite].[TrainingProviderStatusTypeId]    
                       
          
 FROM [dbo].[ProviderSite]  
 JOIN ProviderSiteRelationship
 ON ProviderSite.ProviderSiteID = ProviderSiteRelationship.ProviderSiteID
 JOIN ProviderSiteRelationshipType ON ProviderSiteRelationship.ProviderSiteRelationshipTYpeID
 = ProviderSiteRelationshipTYpe.ProviderSiteRelationshipTYpeID and ProviderSiteRelationshipTYpeName = N'Owner'
 JOIN Provider ON ProviderSiteRelationship.ProviderID = Provider.ProviderID                     
 LEFT JOIN County ON [County].[CountyId] = [ProviderSite].[CountyId]  
 INNER JOIN dbo.LocalAuthorityGroup LAG ON LAG.LocalAuthorityGroupID = [ProviderSite].ManagingAreaID 
 INNER JOIN dbo.LocalAuthorityGroupType LAGT ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID
 AND LocalAuthorityGroupTypeName = N'Managing Area'      
 --LEFT JOIN LSCRegion ON [LSCRegion].[LSCRegionId] = [TrainingProvider].ManagingAreaID        
 WHERE EDSURN = @EDSURN        
                 
 SET NOCOUNT OFF                    
END