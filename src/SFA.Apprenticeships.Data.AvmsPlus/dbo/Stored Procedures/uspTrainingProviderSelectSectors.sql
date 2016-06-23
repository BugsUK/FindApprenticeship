CREATE PROCEDURE [dbo].[uspTrainingProviderSelectSectors]
@ProviderSiteId INT
AS
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
	DECLARE @ProviderId int
	SELECT @ProviderId = ProviderID
	FROM ProviderSiteRelationship psr
	WHERE ProviderSiteID = @ProviderSiteId AND ProviderSiteRelationShipTypeID = 1

  SELECT 
		@ProviderId as 'ProviderId',
		ApprenticeshipOccupation.ApprenticeshipOccupationId as SectorId,
		ISNULL(PassRate, 0) as 'PassRate',
		ISNULL([New], 1) as 'New',
		ApprenticeshipOccupation.Codename,
		ApprenticeshipOccupation.ShortName,
		ApprenticeshipOccupation.FullName
	FROM ApprenticeshipOccupation 
		LEFT JOIN dbo.SectorSuccessRates ON ApprenticeshipOccupation.ApprenticeshipOccupationId = SectorSuccessRates.SectorID  AND SectorSuccessRates.ProviderID = @ProviderId
	ORDER BY ApprenticeshipOccupation.FullName ASC

 
END