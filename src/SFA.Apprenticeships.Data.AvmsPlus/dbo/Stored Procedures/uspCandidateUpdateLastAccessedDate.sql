Create PROCEDURE [dbo].[uspCandidateUpdateLastAccessedDate]
	@candidateId int 
AS      
BEGIN      
	SET NOCOUNT ON      
       
    UPDATE Candidate
        SET LastAccessedDate = getdate()
    WHERE
        CandidateId = @candidateId
          
	SET NOCOUNT OFF      
END