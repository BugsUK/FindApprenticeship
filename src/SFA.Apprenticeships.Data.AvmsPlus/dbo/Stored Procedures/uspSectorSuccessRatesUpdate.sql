CREATE PROCEDURE [dbo].[uspSectorSuccessRatesUpdate]
 @ProviderId INT, @SectorId INT, @PassRate INT, @newSector BIT
AS
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
 
 
	 -- is this an update or insert?
	 IF EXISTS (SELECT 1 FROM SectorSuccessRates WHERE SectorId  = @SectorId AND ProviderID = @ProviderID)
	 BEGIN
		 UPDATE dbo.SectorSuccessRates
		  SET 
		   PassRate = @PassRate,
		   New = @newSector
		     
		  WHERE SectorId  = @SectorId 
		  AND ProviderID = @ProviderID
	END
	ELSE
	BEGIN
	   INSERT INTO dbo.SectorSuccessRates (  
		  ProviderID,  
		  SectorID,  
		  PassRate,
		  [NEW]  
		 ) VALUES (   
		  @ProviderID,  
		  @SectorId,  
		  @PassRate,
		  @NewSector  
		 )  
	END
END