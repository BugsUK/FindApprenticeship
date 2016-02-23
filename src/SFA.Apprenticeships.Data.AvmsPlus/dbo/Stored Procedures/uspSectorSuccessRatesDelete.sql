CREATE PROCEDURE [dbo].[uspSectorSuccessRatesDelete]  
	@providerID INT  , @SectorID INT
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
    -- Insert statements for procedure here  
 DELETE dbo.SEctorSUccessRates  
  WHERE SectorId = @SectorId 
  and ProviderID = @ProviderID
END