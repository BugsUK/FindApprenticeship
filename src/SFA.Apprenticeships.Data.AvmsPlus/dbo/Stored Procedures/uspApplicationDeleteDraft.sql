CREATE PROCEDURE [dbo].[uspApplicationDeleteDraft]
	@ApplicationId int
	
AS
  BEGIN
 	SET NOCOUNT ON
	
		BEGIN TRY
	   
				
				DELETE FROM ApplicationHistory
					WHERE 
						ApplicationId = @ApplicationId
				
				DELETE FROM CAFFIELDS
					WHERE 
						ApplicationId = @ApplicationId
						
				DELETE FROM ADDITIONALANSWER
					WHERE 
						ApplicationId = @ApplicationId

				DELETE FROM Application 
					WHERE 
						ApplicationId = @ApplicationId
				
	   
		END TRY

		BEGIN CATCH
			EXEC RethrowError;
		END CATCH
    
   SET NOCOUNT OFF
 END