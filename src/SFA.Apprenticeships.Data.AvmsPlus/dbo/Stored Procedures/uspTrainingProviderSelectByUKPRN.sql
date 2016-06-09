CREATE PROCEDURE [dbo].[uspTrainingProviderSelectByUKPRN]
@UKPRN VARCHAR (200)
AS
BEGIN                    
                    
 SET NOCOUNT ON                    
                     
 SELECT                    
-- isnull([trainingProvider].[EmployerStandard],'') AS 'EmployerStandard',                    
 isnull([ProviderSite].[FullName],'') AS 'FullName',                    
-- [trainingProvider].[IsContractHolder] AS 'IsContractHolder',                    
-- isnull([trainingProvider].[IsoStandard],'') AS 'IsoStandard',                    
-- [trainingProvider].[IsoYearAchieved] AS 'IsoYearAchieved',                    
-- isnull([trainingProvider].[LSCResponsibilityDetails],'') AS 'LSCResponsibilityDetails',    : No details provided in DataModel Sheet.        
-- [trainingProvider].[OFSTEDDate] AS 'OFSTEDDate',                    
-- isnull([trainingProvider].[OFSTEDGrading],'') AS 'OFSTEDGrading',                    
 [ProviderSite].[OutofDate] AS 'OutofDate',                    
-- isnull([trainingProvider].[PimsId],'') AS 'PimsId',                    
-- [trainingProvider].[PrimaryContact] AS 'PrimaryContact',                    
-- isnull([trainingProvider].[ProviderType],'') AS 'ProviderType',                    
-- isnull([trainingProvider].[Status],'') AS 'Status',                 
 --[trainingProvider].[SuccessRate] AS 'SuccessRate',      : No details provided in DataModel Sheet. Success rate will be calculated based on framework hence logically it should not be part of this SP.    
 isnull([ProviderSite].[TradingName],'') AS 'TradingName',                    
 [ProviderSite].ProviderSiteID AS 'TrainingProviderId',                    
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
 INNER JOIN dbo.LocalAuthorityGroup LAG ON  LAG.LocalAuthorityGroupID = [ProviderSite].ManagingAreaID 
 INNER JOIN dbo.LocalAuthorityGroupType LAGT ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID
 AND LocalAuthorityGroupTypeName = N'Managing Area'      
 --LEFT JOIN LSCRegion ON [LSCRegion].[LSCRegionId] = [TrainingProvider].ManagingAreaID        
 WHERE Provider.UKPRN = @UKPRN     
 AND Provider.ProviderStatusTypeID <> 2    
 AND  ([ProviderSite].TrainingProviderStatusTypeId = 1 OR [ProviderSite].TrainingProviderStatusTypeId = 3)
                    
 SET NOCOUNT OFF                    
END