CREATE PROCEDURE [dbo].[uspVacancyLocationUpdate]
	@VacancyLocationId int,
	@VacancyId int,
	@NumberofPositions smallint,
	@AddressLine1 nvarchar(100),
	@AddressLine2 nvarchar(100),
	@AddressLine3 nvarchar(100),
	@AddressLine4 nvarchar(100),
	@AddressLine5 nvarchar(100),
	@Town nvarchar(80), 
	@CountyId int,
	@PostCode nvarchar(16),
	@EmployersWebsite nvarchar(512)

	
	AS        
BEGIN        
 SET NOCOUNT ON        
 BEGIN TRY   

	UPDATE
		VacancyLocation
	SET
		NumberofPositions = @NumberofPositions,
		AddressLine1 = @AddressLine1,
		AddressLine2 = @AddressLine2,
		AddressLine3 = @AddressLine3,
		AddressLine4 = @AddressLine4,
		AddressLine5 = @AddressLine5,
		Town = @Town,
		CountyId = @CountyId,
		PostCode = @PostCode,
		EmployersWebsite	= @EmployersWebsite
	WHERE
		VacancyLocationId = @VacancyLocationId	

 END TRY        
        
 BEGIN CATCH        
	EXEC RethrowError;        
 END CATCH         
        
    SET NOCOUNT OFF        
END