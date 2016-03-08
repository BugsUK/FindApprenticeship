CREATE PROCEDURE [dbo].[uspGetExternalServicesBySystemId]
	@SystemId int
AS
 

IF EXISTS (SELECT *  
 FROM dbo.ExternalServiceSystemRelationship  
 WHERE ExternalSystemId = @SystemId)
	
SELECT ES.ID As 'ServiceId',  
	  ES.ServiceName,  
	  IsNasDisabled,  
	  IsUserEnabled,  
	  ES.IsEmployerAllowed,  
	  ES.IsTrainingProviderAllowed,  
	  ES.IsThirdPartyAllowed,  
	  ES.ServiceShortName,  
	  ES.ServiceDescription  
 FROM dbo.ExternalServiceSystemRelationship ESR  
 RIGHT OUTER JOIN dbo.ExternalService ES  
 ON ESR.ExternalServiceId = ES.ID  
 WHERE ISNULL(ExternalSystemId,@SystemId) = @SystemId
 
 ELSE
 BEGIN
 
  Declare @bit bit
   SET @bit = 0 
   
 SELECT ES.ID As 'ServiceId',  
		ES.ServiceName,  
	  @bit 'IsNasDisabled',  
	  @bit 'IsUserEnabled',  
	  ES.IsEmployerAllowed,  
	  ES.IsTrainingProviderAllowed,  
	  ES.IsThirdPartyAllowed,  
	  ES.ServiceShortName,  
	  ES.ServiceDescription  
 FROM ExternalService ES  
 
 END