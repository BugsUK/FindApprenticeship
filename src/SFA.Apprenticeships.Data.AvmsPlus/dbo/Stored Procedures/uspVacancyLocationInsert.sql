CREATE PROCEDURE [dbo].[uspVacancyLocationInsert]
	@VacancyId int,
	@NumberOfPositions int,
	@AddressLine1 nvarchar(50),
	@AddressLine2 nvarchar(50),
	@AddressLine3 nvarchar(50),
	@AddressLine4 nvarchar(50),
	@AddressLine5 nvarchar(50),
	@Town nvarchar(40),
	@CountyId int,
	@PostCode nvarchar(8),
	@LocalAuthorityId int,
	@GeocodeEasting decimal(22, 11),  
	@GeocodeNorthing decimal(22, 11),  
	@Longitude decimal(22, 11),  
	@Latitude decimal(22, 11), 
	@EmployersWebsite nvarchar(256)
		            
AS
            
BEGIN 

SET NOCOUNT ON

	BEGIN TRY

		INSERT INTO [dbo].[VacancyLocation]   
			(
				VacancyId,
				NumberOfPositions,
				AddressLine1,
				AddressLine2,
				AddressLine3,
				AddressLine4,
				AddressLine5,
				Town,
				CountyId,
				Postcode,
				LocalAuthorityId,
				GeocodeEasting,
				GeocodeNorthing,
				Longitude,
				Latitude,
				EmployersWebsite
			)
				
		VALUES
			(
				@VacancyId,
				@NumberOfPositions,
				@AddressLine1,
				@AddressLine2,
				@AddressLine3,
				@AddressLine4,
				@AddressLine5,
				@Town,
				@CountyId,
				@PostCode,
				@LocalAuthorityId,
				@GeocodeEasting,  
				@GeocodeNorthing,  
				@Longitude,  
				@Latitude, 
				@EmployersWebsite
			)
			
	END TRY
	
	BEGIN CATCH
	        
		EXEC RethrowError; 
		       
	END CATCH   
		
SET NOCOUNT OFF
            
END