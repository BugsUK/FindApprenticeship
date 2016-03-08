CREATE PROCEDURE [dbo].[uspSavedSearchCriteriaSelectBySavedSearchCriteriaId]    
	@savedSearchCriteriaId int    
AS    
BEGIN    
    -- This stored proc returns 2 result sets, one with search criteria that only have one value 
    -- and a second with the for frameworks as one search can search on multiple frameworks
SET NOCOUNT ON;    
    
    SELECT SavedSearchCriteria.[SavedSearchCriteriaId],    
		SavedSearchCriteria.[CandidateId],    
		SavedSearchCriteria.[Name],    
		SavedSearchCriteria.[SearchType],
		SavedSearchCriteria.[CountyId],
		SavedSearchCriteria.[Postcode], 
		SavedSearchCriteria.[GeocodeEasting],  
		SavedSearchCriteria.[GeocodeNorthing],    
		SavedSearchCriteria.[Longitude],    
		SavedSearchCriteria.[Latitude],     
		SavedSearchCriteria.[DistanceFromPostcode],
		SavedSearchCriteria.[SearchType],    
		SavedSearchCriteria.[MinWages],    
		SavedSearchCriteria.[MaxWages],    
		SavedSearchCriteria.[VacancyReferenceNumber],    
		0 as [EmployerId],    
		SavedSearchCriteria.[Employer] as EmployerName,    
		SavedSearchCriteria.[TrainingProvider],    
		SavedSearchCriteria.[Keywords],    
		SavedSearchCriteria.[DateSearched],    
		SavedSearchCriteria.[BackgroundSearch],    
		SavedSearchCriteria.[AlertSent],    
		SavedSearchCriteria.[CountBackgroundMatches],   
		VacancyPostedSince,
		SavedSearchCriteria.[ApprenticeshipTypeId]   	    
	FROM
		SavedSearchCriteria
	WHERE     
		[SavedSearchCriteria].[SavedSearchCriteriaId] = @savedSearchCriteriaId
--Frameworks
	SELECT 
		ApprenticeshipFrameworkId, 
		ApprenticeShipOccupationId,
		ApprenticeshipFrameworkStatusTypeId
	FROM
		 SearchFrameworks 
	INNER JOIN ApprenticeshipFramework ON 
		ApprenticeshipFramework.ApprenticeshipFrameworkId = SearchFrameworks.FrameworkId
	WHERE     
	   SearchFrameworks.[SavedSearchCriteriaId] = @savedSearchCriteriaId    
END