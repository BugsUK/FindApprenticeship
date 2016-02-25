CREATE PROCEDURE dbo.uspVacancyTransfer
@contractOwnerId	 INT = -1,
@vacancyManagerId INT = -1,
@delivererId	 INT = -1,
@VacancyId int, 
@MasterVacancyId int = null,     
@RelationShipId int                 
AS
BEGIN  
      
 SET NOCOUNT ON      
   
 BEGIN TRY        
 
  If @MasterVacancyId IS NULL
  BEGIN
		UPDATE [dbo].[vacancy] SET
			[VacancyOwnerRelationshipId] = @RelationShipId,
			VacancyManagerID = @vacancyManagerId,
			DeliveryOrganisationID = @delivererId,
			ContractOwnerID = @contractOwnerId 
		WHERE        
			VacancyId = @VacancyId  
					    
  END
  ELSE
  BEGIN     
		UPDATE [dbo].[vacancy] SET
			MasterVacancyId = @MasterVacancyId,
			[VacancyOwnerRelationshipId] = @RelationShipId,
			VacancyManagerID = @vacancyManagerId,
			DeliveryOrganisationID = @delivererId,
			ContractOwnerID = @contractOwnerId 
	         
		WHERE        
			MasterVacancyId = @MasterVacancyId        
   END
   END TRY       
        
    BEGIN CATCH        
  EXEC RethrowError;        
 END CATCH         
        
    SET NOCOUNT OFF        
    
END