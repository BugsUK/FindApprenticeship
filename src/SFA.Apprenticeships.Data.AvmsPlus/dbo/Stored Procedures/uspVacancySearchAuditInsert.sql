CREATE PROCEDURE [dbo].[uspVacancySearchAuditInsert]
    @searchType						INT				= NULL,
    @searchTerm						NVARCHAR (200)  = NULL,
    @apprenticeOccupation			INT				= NULL,
    @apprenticeFrameworks			NVARCHAR (4000) = NULL,
    @locationId						INT             = NULL,
    @vacancyPostedDate				INT		        = NULL,
    @postcode						NVARCHAR(8)		= NULL,
    @distancefromInMiles			INT				= NULL,
    @distancefromInMeters			INT             = NULL,
    @easting						INT             = NULL,
    @northing						INT             = NULL,
    @weeklyWagesFrom				INT             = NULL,
    @weeklyWagesTo					INT             = NULL,
    @pageNo							INT             = NULL,
    @pageSize						INT             = NULL,
    @sortByField					NVARCHAR (100)  = NULL,
    @sortOrder						NVARCHAR(20)    = NULL,	
	@totalVacancies					INT				= NULL,
	@totalPositions					INT				= NULL,
	@SearchDate						DATETIME		= NULL,
	@ApprenticeshipTypeId			INT				= NULL
AS  
BEGIN  
	SET NOCOUNT ON  
   
	BEGIN TRY  
		INSERT INTO VacancySearchAudit   
			(
			SearchType, 
			SearchTerm, 
			ApprenticeshipOccupation, 
			ApprenticeFrameworks, 
			LocationId, 
			VacancyPostedSince, 
			Postcode,
			DistanceFromInMiles, 
			DistanceFromInMeters, 
			Easting, 
			Northing, 
			WeeklyWagesFrom, 
			WeeklyWagesTo, 
			PageNo, 
			PageSize, 
			SortByField, 
			SortOrder, 
			TotalVacancies,
			TotalPositions,
			SearchDate,
			ApprenticeshipTypeId)  
		VALUES (
			@searchType,
			@searchTerm, 
			@apprenticeOccupation, 
			@apprenticeFrameworks, 
			@locationId, 
			@vacancyPostedDate, 
			@postcode, 
			@distancefromInMiles,
			@distancefromInMeters, 
			@easting, 
			@northing, 
			@weeklyWagesFrom, 
			@weeklyWagesTo, 
			@pageNo, 
			@pageSize, 
			@sortByField, 
			@sortOrder, 
			@totalVacancies, 
			@totalPositions,
			@SearchDate,
			@ApprenticeshipTypeId)
	END TRY  
  
	BEGIN CATCH  
		EXEC RethrowError;  
	END CATCH  
      
    SET NOCOUNT OFF  

END