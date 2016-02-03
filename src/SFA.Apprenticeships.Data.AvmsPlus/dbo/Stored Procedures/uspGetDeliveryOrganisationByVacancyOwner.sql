  
 /****** Object:  StoredProcedure [dbo].[uspGetDeliveryOrganisationByVacancyOwner]  
  Script Date: 01/17/2012 12:29:31
  Created:  By Sanjeev Kumar Kodavalla
  exec [dbo].[uspGetDeliveryOrganisationByVacancyOwner] 24,424
   ******/ 
CREATE PROCEDURE [dbo].[uspGetDeliveryOrganisationByVacancyOwner]     
  @providerID INT,  --- Selected From DropDown        
 @providerSiteId INT -- Logged in providerSite    
AS    
BEGIN    
    
 SET NOCOUNT ON    
     
	SELECT 
		ProviderSiteID,
		ProviderSite,
		ProviderSiteTypeId,
		PostCode
	FROM (
		SELECT 
			PS.ProviderSiteID AS 'ProviderSiteID' 
			,PS.TradingName AS 'ProviderSite' 
			,PS.TrainingProviderStatusTypeId AS 'ProviderSiteTypeId'
			,ISNULL(PS.PostCode, '') AS 'PostCode'
			,CASE PS.ProviderSiteID  
				WHEN @providerSiteId THEN 1
				ELSE 0 
			  END as IsVO
		FROM ProviderSiteRelationShip PSR     
		  INNER JOIN dbo.ProviderSite PS ON PSR.ProviderSiteID = PS.ProviderSiteID
		  AND PS.TrainingProviderStatusTypeID != 2    
		WHERE  PSR.Providerid = @providerID    
		  AND PSR.ProviderSiteRelationShipTypeid in (1,2)
		) Sites
    ORDER BY IsVO DESC, ProviderSite ASC
       
   SET NOCOUNT OFF    
END