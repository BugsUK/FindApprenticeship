CREATE PROCEDURE [dbo].[uspVacancyLocationDelete]
	@VacancyLocationId int
AS

BEGIN        
 SET NOCOUNT ON        
	BEGIN TRY   

	
		DELETE FROM
			VacancyLocation
		WHERE
			VacancyLocationId = @VacancyLocationId
	
	END TRY        
        
	BEGIN CATCH        
		EXEC RethrowError;        
	END CATCH         
        
    SET NOCOUNT OFF        
END