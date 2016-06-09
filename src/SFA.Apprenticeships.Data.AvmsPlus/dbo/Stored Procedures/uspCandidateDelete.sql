CREATE PROCEDURE [dbo].[uspCandidateDelete]
	@candidateId int      
AS          
BEGIN          

	Declare	@personId int

	SET NOCOUNT ON          
	BEGIN TRY          
		-- Obtain Person information before deletion
		Select	@personId = PersonId From Candidate 
		Where	CandidateId = @candidateId

		-- Attempt 'CandidatePreferences' deletion
		Delete From CandidatePreferences Where CandidateId = @candidateId

		-- Attempt 'Candidate' deletion
		Delete From Candidate Where CandidateId = @candidateId
		
		
		-- Attempt 'Person' deletion
		Delete From Person Where PersonId = @personId
		IF @@ROWCOUNT = 0          
		BEGIN          
			RAISERROR('Could not delete Candidate / Person Information.  Operation aborted.', 16, 2)     
		END
	END TRY          
          
    BEGIN CATCH          
		EXEC RethrowError;          
	END CATCH

	SET NOCOUNT OFF          
END