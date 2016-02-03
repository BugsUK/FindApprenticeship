CREATE PROCEDURE [dbo].[uspGetProviderById]
	@providerId int
AS
BEGIN 
	SET NOCOUNT ON          
	       
	SELECT       
	
 ProviderID As 'ProviderId',  
 UPIN As 'UPIN',  
 UKPRN As 'UKPRN',  
 FullName As 'FullName',  
 TradingName As 'TradingName',  
 IsContracted As 'IsContracted',  
 ContractedFrom As 'ContractedFrom',  
 ContractedTo As 'ContractedTo',  
 ProviderStatusTypeID AS 'ProviderStatusId',
 IsNASProvider as 'IsNASProvider'     


  FROM Provider
  WHERE Provider.ProviderID = @providerId   
END