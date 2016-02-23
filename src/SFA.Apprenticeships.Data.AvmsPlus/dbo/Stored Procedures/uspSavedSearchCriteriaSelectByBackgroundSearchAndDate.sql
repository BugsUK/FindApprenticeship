CREATE PROCEDURE [dbo].[uspSavedSearchCriteriaSelectByBackgroundSearchAndDate]        
  @DateFrom DateTime    
AS    
BEGIN    
    
 SET NOCOUNT ON     
 SELECT    
 [savedSearchCriteria].[SavedSearchCriteriaId] AS 'SavedSearchCriteriaId',    
 [savedSearchCriteria].[CandidateId] AS 'CandidateId',    
 [savedSearchCriteria].[Name] AS 'Name',    
 [savedSearchCriteria].[SearchType] AS 'SearchType',    
 --[savedSearchCriteria].[Region] AS 'Region',    
 [savedSearchCriteria].CountyId AS 'CountyId',    
 [savedSearchCriteria].[Postcode] AS 'Postcode',    
 [savedSearchCriteria].[Longitude] AS 'Longitude',    
 [savedSearchCriteria].[Latitude] AS 'Latitude',    
 [savedSearchCriteria].[GeocodeEasting] AS 'GeocodeEasting',    
 [savedSearchCriteria].[GeocodeNorthing] AS 'GeocodeNorthing',    
 [savedSearchCriteria].[DistanceFromPostcode] AS 'DistanceFromPostcode',    
 [savedSearchCriteria].[MinWages] AS 'MinWages',    
 [savedSearchCriteria].[MaxWages] AS 'MaxWages',    
 [savedSearchCriteria].[VacancyReferenceNumber] AS 'VacancyReferenceNumber',    
 [savedSearchCriteria].[Employer] AS 'Employer',    
 [savedSearchCriteria].[TrainingProvider] AS 'TrainingProvider',    
 [savedSearchCriteria].[Keywords] AS 'Keywords',    
 [savedSearchCriteria].[DateSearched] AS 'DateSearched',    
 [savedSearchCriteria].[BackgroundSearch] AS 'BackgroundSearch',    
 [savedSearchCriteria].[AlertSent] AS 'AlertSent',    
 [savedSearchCriteria].[CountBackgroundMatches] AS 'CountBackgroundMatches'    
 FROM [dbo].[SavedSearchCriteria] [savedSearchCriteria]    
 WHERE [savedSearchCriteria].[BackgroundSearch] = 1    
 AND DateSearched <= @DateFrom    
     
 SET NOCOUNT OFF    
END