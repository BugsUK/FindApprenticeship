CREATE PROCEDURE [dbo].[uspApplicationHistoryDeleteByStatus]
	@ApplicationID int,
	@ApplicationStatus int
AS

BEGIN

SET NOCOUNT ON  
   
	BEGIN TRY
		
		DELETE FROM 
			ApplicationHistory
		WHERE
			ApplicationId = @ApplicationID
		AND
			ApplicationHistoryEventSubTypeID = @ApplicationStatus
	       
	END TRY  
	  
	BEGIN CATCH  
	    
		EXEC RethrowError;  
	  
	END CATCH  
      
SET NOCOUNT OFF  

END