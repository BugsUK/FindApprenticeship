CREATE PROCEDURE [dbo].[uspGetTrainingProviderFrameworkSectors]    
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
    -- Insert statements for procedure here  
 Select ProviderID, SectorID 
	FROM SectorSuccessRates
Where NOT EXISTS 
 (Select * From [ProviderSiteFramework] PSF JOIN ApprenticeShipFramework AF
 ON PSF.FrameworkID = AF.ApprenticeshipFrameworkID AND AF.ApprenticeshipOccupationID = sectorsuccessrates.SectorID)

END