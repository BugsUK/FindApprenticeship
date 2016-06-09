CREATE PROCEDURE [dbo].[uspThirdPartyInsert]  
    @EdsUrn int ,  
    @ThirdPartyName nvarchar(255) ,  
    @AddressLine1 nvarchar(50) ,  
    @AddressLine2 nvarchar(50) ,  
    @AddressLine3 nvarchar(50) ,  
    @AddressLine4 nvarchar(50) ,  
    @Town nvarchar(50) ,  
    @County int,  
    @PostCode nvarchar(50) ,  
    @Latitude decimal(20, 15) ,  
    @Longitude decimal(20, 15),  
    @GeocodeEasting decimal(20, 15) ,  
    @GeocodeNorthing decimal(20, 15),  
    @ThirdPartyId int out
AS  
BEGIN  
 SET NOCOUNT ON  
   
 BEGIN TRY  
   
    INSERT INTO [dbo].[ThirdParty]  
    (  
                  EDSURN,     
                  ThirdPartyName,     
                  AddressLine1,
                  AddressLine2,  
                  AddressLine3,
                  AddressLine4,
                  Town,
                  CountyId,
                  PostCode, 
                  Latitude,     
                  Longitude,     
                  GeocodeEasting,     
                  GeocodeNorthing
	)
    VALUES  
            (  
                @EdsUrn,
                @ThirdPartyName,    
                @AddressLine1,    
                @AddressLine2,  
                @AddressLine3,    
                @AddressLine4,    
                @Town,    
                @County,    
                @PostCode, 
                @Latitude,    
                @Longitude,    
                @GeocodeEasting,    
                @GeocodeNorthing
            )
  
        SET @ThirdPartyId = SCOPE_IDENTITY()  
        
    END TRY  
  
    BEGIN CATCH  
  EXEC RethrowError;  
 END CATCH  
      
    SET NOCOUNT OFF  
END