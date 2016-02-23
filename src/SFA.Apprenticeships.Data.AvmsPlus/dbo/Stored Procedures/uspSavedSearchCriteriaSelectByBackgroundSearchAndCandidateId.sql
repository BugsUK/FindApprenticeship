CREATE PROCEDURE [dbo].[uspSavedSearchCriteriaSelectByBackgroundSearchAndCandidateId]        
  @CandidateId int
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
 [savedSearchCriteria].[CountBackgroundMatches] AS 'CountBackgroundMatches',  
 [savedSearchCriteria].[ApprenticeshipTypeId] AS 'ApprenticeshipTypeId',
 [ApprenticeshipFramework].[ApprenticeshipFrameworkId],    
 [ApprenticeshipOccupation].[ApprenticeshipOccupationId],    
 [ApprenticeshipOccupation].[FullName] as ApprenticeOccupation,    
 [ApprenticeshipFramework].[FullName] as ApprenticeFramework,
 [ApprenticeshipFramework].[ApprenticeshipFrameworkStatusTypeId] as ApprenticeshipFrameworkStatusTypeId
  
 FROM   
 -- left joins should be inner joins when there is info in savedSearchCriteria.Employer  
 SavedSearchCriteria   
 LEFT JOIN SearchFrameworks ON SearchFrameworks.SavedSearchCriteriaId = SavedSearchCriteria.SavedSearchCriteriaId
	 LEFT JOIN ApprenticeshipFramework ON ApprenticeshipFramework.ApprenticeshipFrameworkId = SearchFrameworks.FrameworkId
	 LEFT JOIN ApprenticeshipOccupation ON ApprenticeshipOccupation.ApprenticeshipOccupationId = ApprenticeshipFramework.ApprenticeshipOccupationId 
 WHERE [savedSearchCriteria].[BackgroundSearch] = 1    
 AND [savedSearchCriteria].[CandidateId]=@CandidateId
 AND [AlertSent]=0

 SET NOCOUNT OFF    
END