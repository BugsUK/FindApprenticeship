CREATE PROCEDURE [dbo].[uspSubVacancyDelete]
	@VacancyID int, 
	@ApplicationID int
AS

BEGIN

SET NOCOUNT ON  
   
	BEGIN TRY
		
		DELETE FROM 
			SubVacancy
		WHERE
			VacancyId = @VacancyID
		AND
			AllocatedApplicationID = @ApplicationID
	       
	END TRY  
	  
	BEGIN CATCH  
	    
		EXEC RethrowError;  
	  
	END CATCH  
      
SET NOCOUNT OFF  

END