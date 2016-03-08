CREATE PROCEDURE [dbo].[uspVacancyLocationSelectByVacancyId]
	@VacancyId int            
AS
            
BEGIN 

SET NOCOUNT ON

	BEGIN TRY
	 
		SELECT	
				VL.VacancyId,
				VL.VacancyLocationId,
				VL.NumberOfPositions,
				VL.AddressLine1,
				VL.AddressLine2,
				VL.AddressLine3,
				VL.AddressLine4,
				VL.AddressLine5,
				VL.Town,
				VL.CountyId,
				VL.Postcode,
				VL.LocalAuthorityId,
				VL.GeocodeEasting,
				VL.GeocodeNorthing,
				VL.Longitude,
				VL.Latitude,
				VL.EmployersWebsite
				
		FROM	VacancyLocation VL
		
		WHERE	VL.VacancyId = @VacancyId
		
		ORDER BY VL.Town ASC
		
	END TRY

	BEGIN CATCH
	        
		EXEC RethrowError; 
		       
	END CATCH   
		
SET NOCOUNT OFF
            
END