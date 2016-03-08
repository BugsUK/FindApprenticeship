CREATE PROCEDURE [dbo].[uspCandidateUpdateConfirmEmail]
	@candidateId int
 AS
 BEGIN
    
	SET NOCOUNT ON
	BEGIN TRY
		Begin Tran
			-- Getting required details
			Declare @pId			int			-- PersonId
			Declare @newEmail	nvarchar(50)	-- UnconfirmedEmailAddress

			Select	@pId = PersonId, @newEmail = UnconfirmedEmailAddress 
			From	Candidate
			Where	CandidateId = @candidateId

			If @newEmail <> 'NULL'
			Begin

				Update	Person
				Set		Email = @newEmail
				Where	PersonId = @pId

				Update	Candidate
				Set		UnconfirmedEmailAddress = Null
				Where	CandidateId = @candidateId
			End

		Commit Tran
    END TRY
	BEGIN CATCH
		If @@TranCount > 0
		Begin
			Rollback Tran
		End

		RAISERROR('Candidate Information could not be found.  Operation aborted.', 16, 2)
		EXEC RethrowError;
	END CATCH
	SET NOCOUNT OFF
END