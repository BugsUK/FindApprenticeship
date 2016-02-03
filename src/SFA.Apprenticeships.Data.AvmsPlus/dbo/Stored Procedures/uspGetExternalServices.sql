CREATE PROCEDURE [dbo].[uspGetExternalServices]
	
AS
	SELECT 	[ID] 'ServiceId'
		  ,[ServiceName]
		  ,[ServiceShortName]
		  ,[ServiceDescription]
		  ,[IsEmployerAllowed]
		  ,[IsTrainingProviderAllowed]
		  ,[IsThirdPartyAllowed]
	FROM  [ExternalService]