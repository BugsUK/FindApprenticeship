CREATE PROCEDURE [dbo].[uspApplicationDeleteDraft]
	@ApplicationId int
	
AS
  BEGIN
 	SET NOCOUNT ON
	
		BEGIN TRY
	   
				
				DELETE FROM APPLICATIONHISTORY
					WHERE 
						APPLICATIONID = @ApplicationId
				
				DELETE FROM CAFFIELDS
					WHERE 
						APPLICATIONID = @ApplicationId
						
				DELETE FROM ADDITIONALANSWER
					WHERE 
						APPLICATIONID = @ApplicationId

				DELETE FROM APPLICATION 
					WHERE 
						APPLICATIONID = @ApplicationId
				
	   
		END TRY

		BEGIN CATCH
			EXEC RethrowError;
		END CATCH
    
   SET NOCOUNT OFF
 END