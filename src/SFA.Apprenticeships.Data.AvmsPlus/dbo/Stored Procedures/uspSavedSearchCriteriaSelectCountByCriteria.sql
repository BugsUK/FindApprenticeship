CREATE PROCEDURE [dbo].[uspSavedSearchCriteriaSelectCountByCriteria]     
 @SavedSearchCriteriaId uniqueidentifier,    
 @CandidateId int,    
 @Name nvarchar(50),    
 @SearchType smallint,    
 @Region nvarchar(30),    
 @Locations smallint,    
 @Postcode nvarchar(8),    
 @Longitude decimal(20, 15),    
 @Latitude decimal(20, 15),    
 @GeocodeEasting decimal(20, 15),    
 @GeocodeNorthing decimal(20, 15),    
 @MinWages smallint,    
 @MaxWages smallint,    
 @VacancyReferenceNumber nvarchar(20),    
 @Employer nvarchar(255),    
 @TrainingProvider nvarchar(255),    
 @Keywords nvarchar(100)    
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;    
    
    SELECT COUNT(1) as Result FROM dbo.SavedSearchCriteria    
  
 WHERE     
  (CandidateId=@CandidateId OR @CandidateId IS NULL) AND    
  (SearchType=@SearchType OR @searchType IS NULL) AND     
    
  (Postcode=@Postcode OR @postcode IS NULL) AND    
  (Longitude=@Longitude OR @longitude IS NULL) AND    
  (Latitude=@Latitude OR @latitude IS NULL) AND    
  (GeocodeEasting=@GeocodeEasting OR @geocodeEasting IS NULL) AND    
  (GeocodeNorthing=@GeocodeNorthing OR @geocodeNorthing IS NULL) AND    
  (MinWages=@MinWages OR @minwages IS NULL) AND    
  (MaxWages=@MaxWages OR @maxwages IS NULL) AND    
  (VacancyReferenceNumber=@VacancyReferenceNumber OR @vacancyReferenceNumber IS NULL) AND    
  (Employer=@Employer OR @employer IS NULL) AND    
  (TrainingProvider=@TrainingProvider OR @trainingprovider IS NULL) AND    
  (Keywords=@Keywords OR @keywords IS NULL)    
END