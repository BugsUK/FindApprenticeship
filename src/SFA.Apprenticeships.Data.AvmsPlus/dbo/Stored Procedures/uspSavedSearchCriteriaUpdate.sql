CREATE PROCEDURE [dbo].[uspSavedSearchCriteriaUpdate]      
 @alertSent bit = NULL,      
 @backgroundSearch bit = NULL,      
 @countBackgroundMatches int = NULL,      
 @dateSearched datetime = NULL,      
 @employer nvarchar(255) = NULL,      
 @geocodeEasting int = NULL,      
 @geocodeNorthing int = NULL,      
 @keywords nvarchar(100) = NULL,      
 @latitude decimal(20,15) = NULL,      
 @CountyId smallint = NULL,      
 @longitude decimal(20,15) = NULL,      
 @maxWages smallint = NULL,      
 @minWages smallint = NULL,      
 @name nvarchar(50),      
 @candidateId int,      
 @postcode nvarchar(8) = NULL,    
 @savedSearchCriteriaId int = null,      
 @DistanceFromPostcode smallint = NULL,      
 @searchType smallint = NULL,      
 @trainingProvider nvarchar(255) = NULL,      
 @vacancyReferenceNumber nvarchar(20) = NULL,        
 @apprenticeshipFrameworks varchar(4000),     
 @vacancyPostedSince smallint,
 @apprenticeshipTypeValue int = NULL      
AS      
BEGIN      
      
 --The [dbo].[SavedSearchCriteria] table doesn't have a timestamp column. Optimistic concurrency logic cannot be generated      
 SET NOCOUNT ON      
      
 BEGIN TRY      
 --if @originalName = NULL       
 --BEGIN      
 -- SET @OriginalName = @name      
 --END      
 UPDATE [dbo].[SavedSearchCriteria]       
 SET       
  [AlertSent] = @alertSent,
  [CountBackgroundMatches] = @countBackgroundMatches,       
  [DateSearched] = @dateSearched,       
  [Employer] = @employer,       
  [GeocodeEasting] = @geocodeEasting,       
  [GeocodeNorthing] = @geocodeNorthing,       
  [Keywords] = @keywords,       
  [Latitude] = @latitude,     
  [CountyId] = @CountyId,    
  --[Location] = ISNULL(@location, [Location]),       
  [Longitude] = @longitude,       
  [MaxWages] = @maxWages,        
  [MinWages] = @minWages,       
  [Postcode] = @postcode,      
  [Name] = @name,         
  --[SearchRadius] = @searchRadius,       
  [SearchType] = @searchType,       
  [TrainingProvider] = @trainingProvider,       
  [VacancyReferenceNumber] = @vacancyReferenceNumber,  
  [DistanceFromPostcode]= @DistanceFromPostcode,
  [VacancyPostedSince] = @vacancyPostedSince,
  [ApprenticeshipTypeId] = @apprenticeshipTypeValue
 WHERE       
 [SavedSearchCriteriaId]=@savedSearchCriteriaId      
      
 IF @@ROWCOUNT = 0      
 BEGIN      
  RAISERROR('Concurrent update error. Updated aborted.', 16, 2)      
 END

	DELETE FROM [dbo].[SearchFrameworks]
	WHERE SavedSearchCriteriaId = @savedSearchCriteriaId
	
	IF(@apprenticeshipFrameworks <> '|0|')  
	BEGIN 
		INSERT INTO [dbo].[SearchFrameworks]
		(
			FrameworkId, 
			SavedSearchCriteriaId
		)
		select 
			[value] ,
			@savedSearchCriteriaId
		from dbo.fnx_ConvertToArrayTable(@apprenticeshipFrameworks, '|')
	END
    END TRY      
      
    BEGIN CATCH      
  EXEC RethrowError;      
 END CATCH       
      
 SET NOCOUNT OFF      
END