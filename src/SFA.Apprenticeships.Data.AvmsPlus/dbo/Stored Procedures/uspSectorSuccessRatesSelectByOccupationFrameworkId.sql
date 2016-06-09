CREATE PROCEDURE [dbo].[uspSectorSuccessRatesSelectByOccupationFrameworkId]
	@frameworkId int
AS  
BEGIN  
	-- SET NOCOUNT ON added to prevent extra result sets from  
	-- interfering with SELECT statements.  
	SET NOCOUNT ON;  
  
	Select	
			ssr.ProviderID,
			ssr.SectorID,
			ssr.PassRate
	From	SectorSuccessRates ssr, ApprenticeshipFramework f
	Where	ssr.SectorId = f.ApprenticeshipOccupationId
	And		f.ApprenticeshipFrameworkId = @frameworkId

END