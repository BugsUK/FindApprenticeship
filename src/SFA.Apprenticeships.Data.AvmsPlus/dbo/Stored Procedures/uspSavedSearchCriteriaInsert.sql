CREATE PROCEDURE [dbo].[uspSavedSearchCriteriaInsert]    
 @alertSent bit = NULL,    
 @backgroundSearch bit = NULL,    
 @countBackgroundMatches int = NULL,    
 @dateSearched datetime = NULL,    
 @employer nvarchar(255) = NULL,    
 @employerId int=NULL,    
 @geocodeEasting int = NULL,    
 @geocodeNorthing int = NULL,    
 @keywords nvarchar(100) = NULL,    
 @latitude decimal(20,15) = NULL,    
 @longitude decimal(20,15) = NULL,    
 @CountyId int,  
 @maxWages smallint = NULL,    
 @minWages smallint = NULL,    
 @name nvarchar(50),    
 @candidateId int,    
 @postcode nvarchar(8) = NULL,     
 @DistanceFromPostcode smallint = NULL,  -- rename the field SearchRadius   
 @searchType smallint=NULL,    
 @trainingProvider nvarchar(255) = NULL,    
 @vacancyReferenceNumber nvarchar(20) = NULL,    
 @apprenticeshipFrameworks varchar(4000),    
 @vacancyPostedSince smallint,
 @apprenticeshipTypeValue int = NULL    
AS    
BEGIN    
 SET NOCOUNT ON    
     
 BEGIN TRY    
    INSERT INTO [dbo].[SavedSearchCriteria]     
 (    
  [AlertSent],     
  [BackgroundSearch],     
  [CountBackgroundMatches],     
  [DateSearched],     
  [Employer],         
  [GeocodeEasting],     
  [GeocodeNorthing],     
  [Keywords],     
  [Latitude],     
  CountyId,  
  [Longitude],     
  [MaxWages],     
  [MinWages],     
  [Name],     
  [CandidateId],     
  [Postcode],     
  [DistanceFromPostcode],     
  [SearchType],     
  [TrainingProvider],     
  [VacancyReferenceNumber],    
  [VacancyPostedSince],
  [ApprenticeshipTypeId]    
 )    
 VALUES     
 (    
  @alertSent,     
  ISNULL(@backgroundSearch, 0),
  @countBackgroundMatches,    
  @dateSearched,     
  @employer,      
  @geocodeEasting,     
  @geocodeNorthing,     
  @keywords,     
  @latitude,     
 -- @location,    
  @CountyId,   
  @longitude,     
  @maxWages,     
  @minWages,     
  @name,     
  @candidateId,     
  @postcode,     
  @DistanceFromPostcode,     
  @searchType,     
  @trainingProvider,     
  @vacancyReferenceNumber,    
  @vacancyPostedSince,
  @apprenticeshipTypeValue    
 )    
   
	DECLARE @savedSearchCriteriaId int

	SELECT @savedSearchCriteriaId = SCOPE_IDENTITY()

	
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
	SELECT @savedSearchCriteriaId as SavedSearchCriteriaId
 
    END TRY    
    
    BEGIN CATCH    
  EXEC RethrowError;    
 END CATCH    
        
    SET NOCOUNT OFF    
END