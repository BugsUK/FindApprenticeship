CREATE PROCEDURE [dbo].[uspTrainingProviderSelectSectorsByProviderId]
@ProviderId INT
AS
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
     SELECT 
		@ProviderId as 'ProviderId',
		ApprenticeshipOccupation.ApprenticeshipOccupationId as SectorId,
		ISNULL(PassRate, 0) as 'PassRate',
		ISNULL([New], 1) as 'New',
		ApprenticeshipOccupation.Codename,
		ApprenticeshipOccupation.ShortName,
		ApprenticeshipOccupation.FullName
	FROM ApprenticeshipOccupation 
		LEFT JOIN dbo.SectorSuccessRates ON ApprenticeshipOccupation.ApprenticeshipOccupationId = SectorSuccessRates.SectorId  AND SectorSuccessRates.ProviderID = @ProviderId
	ORDER BY ApprenticeshipOccupation.FullName ASC
END