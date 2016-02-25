
/****** Object:  StoredProcedure [dbo].[uspGetContractOwnersByVacancyOwner]  
  Script Date: 01/17/2012 12:29:31
  Created:  By Sanjeev Kumar Kodavalla
  exec [dbo].[uspGetContractOwnersByVacancyOwner] 424
   ******/




CREATE PROCEDURE [dbo].[uspGetContractOwnersByVacancyOwner]    
 @providerSiteId int  
AS  
BEGIN  
  
 SET NOCOUNT ON  
   
   
 SELECT  P.ProviderID AS 'ProviderID' 
		,P.TradingName AS 'ProviderName' 
		,P.ProviderStatusTypeID AS 'ProviderStatusId'
		FROM dbo.ProviderSite PS -- ON PS.ProviderSiteID =  VO.ProviderSiteID  
   INNER JOIN dbo.ProviderSiteRelationship PSR ON PSR.ProviderSiteID = PS.ProviderSiteID  
   AND PSR.ProviderSiteRelationShipTypeID IN(1,2) -- subcontract and owner  
   INNER JOIN dbo.Provider  P ON P.ProviderID = PSR.ProviderID AND P.IsContracted = 1   
   AND P.ProviderStatusTypeID  != 2
   WHERE PS.ProviderSiteID = @providerSiteId   
     
   SET NOCOUNT OFF  
END