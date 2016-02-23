CREATE PROCEDURE [dbo].[uspCandidateStatusChange]
@candidateId INT, @candidateStatusId INT
AS
BEGIN	
	-- SET NOCOUNT ON added to prevent extra result sets from  
	-- interfering with SELECT statements.  
	SET NOCOUNT ON            
	BEGIN TRY    
 
		-- Setting the status of candidate to Provided Value
		UPDATE Candidate
		SET CandidateStatusTypeId = @candidateStatusId
		WHERE CandidateId = @candidateId
	
	IF @@ROWCOUNT = 0            
	BEGIN            
	  RAISERROR('Could not set status of Candidate.  Operation aborted.', 16, 2)       
	END  
	END TRY  
	BEGIN CATCH            
		  EXEC RethrowError;            
	END CATCH  
  
	SET NOCOUNT OFF            
END