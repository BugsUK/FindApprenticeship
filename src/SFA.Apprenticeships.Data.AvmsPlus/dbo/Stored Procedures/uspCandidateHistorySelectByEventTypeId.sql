CREATE PROCEDURE [dbo].[uspCandidateHistorySelectByEventTypeId]         
	@CandidateId INT,           
	@EventTypeId int
AS        
BEGIN        
	SET NOCOUNT ON        
         
	SELECT TOP 1
		CandidateHistoryId,
		CandidateId,
		CandidateHistoryEventTypeId,
		CandidateHistorySubEventTypeId,
		EventDate,
		Comment AS 'Comment',
		ISNULL(UserName,'') AS 'UserName'
	FROM dbo.CandidateHistory
	WHERE CandidateId = @CandidateId AND CandidateHistoryEventTypeId=@EventTypeId
	ORDER BY eventdate DESC
	
	SET NOCOUNT OFF        
END