SET IDENTITY_INSERT ExternalService ON
GO

MERGE INTO dbo.ExternalService AS TARGET 
USING (VALUES
('1','Summary Vacancy Feed','VSI','Display of summary vacancy information',1,1,1),
('2','Full Vacancy Detail Feed','VDI','Display of detailed vacancy information',1,1,1),
('3','Reference Data File','RDS','Reference Data for web services',1,1,1),
('4','Bulk Vacancy Upload','BVU','Uploading of multiple vacancies',1,1,0),
('5','Application Tracking','ATS','Tracking of offline successful applications',1,1,0)
)
AS SOURCE (ID, ServiceName, ServiceShortName, ServiceDescription, IsEmployerAllowed, IsTrainingProviderAllowed, IsThirdPartyAllowed)
ON TARGET.ID = SOURCE.ID
WHEN MATCHED THEN
	UPDATE SET 
	TARGET.ServiceName = SOURCE.ServiceName, 
	TARGET.ServiceShortName = SOURCE.ServiceShortName, 
	TARGET.ServiceDescription = SOURCE.ServiceDescription, 
	TARGET.IsEmployerAllowed = SOURCE.IsEmployerAllowed, 
	TARGET.IsTrainingProviderAllowed = SOURCE.IsTrainingProviderAllowed, 
	TARGET.IsThirdPartyAllowed = SOURCE.IsThirdPartyAllowed
WHEN NOT MATCHED BY TARGET THEN
	INSERT (ID, ServiceName, ServiceShortName, ServiceDescription, IsEmployerAllowed, 
	IsTrainingProviderAllowed, IsThirdPartyAllowed)  
	Values (SOURCE.ID, SOURCE.ServiceName, SOURCE.ServiceShortName, SOURCE.ServiceDescription, SOURCE.IsEmployerAllowed, 
	SOURCE.IsTrainingProviderAllowed, SOURCE.IsThirdPartyAllowed)
WHEN NOT MATCHED BY SOURCE THEN
	DELETE;
	
SET IDENTITY_INSERT ExternalService OFF