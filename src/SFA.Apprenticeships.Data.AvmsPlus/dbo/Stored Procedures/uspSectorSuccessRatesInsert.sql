CREATE PROCEDURE [dbo].[uspSectorSuccessRatesInsert]
    @TrainingProviderId INT ,
    @SectorId INT ,
    @PassRate SMALLINT ,
    @NewSector BIT
AS 
    BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
        SET NOCOUNT ON ;    
   
        IF NOT EXISTS ( SELECT  1
                        FROM    SectorSuccessRates
                        WHERE   SectorId = @SectorId
                                AND ProviderID = @TrainingProviderId ) 
            BEGIN    
    -- Insert statements for procedure here    
                INSERT  INTO dbo.SectorSuccessRates
                        ( ProviderID ,
                          SectorId ,
                          PassRate ,
                          [NEW]    
                        )
                VALUES  ( @TrainingProviderId ,
                          @SectorId ,
                          @PassRate ,
                          @NewSector    
                        )    
 
            END
    END