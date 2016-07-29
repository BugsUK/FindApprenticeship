CREATE PROCEDURE [dbo].[uspNASSupportContactSelectEmailById]
	@ManagingAreaId int
AS    
BEGIN    
 SET NOCOUNT ON    
  
	BEGIN TRY    
	Select EmailAddress From NASSupportContact
		Where ManagingAreaID = @ManagingAreaId
	END TRY
    
    BEGIN CATCH    
		EXEC RethrowError;    
	END CATCH    
 SET NOCOUNT OFF    
END