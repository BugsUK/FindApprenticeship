CREATE PROCEDURE [dbo].[uspGetVacancyLocationTypes]         
AS
            
BEGIN 

SET NOCOUNT ON

	BEGIN TRY
	 
		SELECT	* FROM VacancyLocationType
		
	END TRY

	BEGIN CATCH
	        
		EXEC RethrowError; 
		       
	END CATCH   
		
SET NOCOUNT OFF
            
END