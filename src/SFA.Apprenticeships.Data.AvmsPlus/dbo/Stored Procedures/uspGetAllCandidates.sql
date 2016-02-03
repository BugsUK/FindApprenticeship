CREATE PROCEDURE  [dbo].[uspGetAllCandidates]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    
	 SELECT candidate.CandidateId from candidate  
	 inner join SavedSearchCriteria on Candidate.CandidateId = SavedSearchCriteria.CandidateId
     where SavedSearchCriteria.BackgroundSearch = 1
END