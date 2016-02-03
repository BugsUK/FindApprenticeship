CREATE PROCEDURE [dbo].[uspGetCandidateSavedSearchCount]    
 @candidateId int,
 @candidateCount int OUT  


AS


BEGIN
	SET NOCOUNT ON
    
    SELECT @candidateCount = COUNT(SavedSearchCriteriaId) 
        FROM 
    SAVEDSEARCHCRITERIA 
        WHERE 
    CANDIDATEID = @candidateId


	SET NOCOUNT OFF
END