CREATE PROCEDURE [dbo].[uspStakeHolderUpdateConfirmEmail]
	@StakeHolderId INT
AS
BEGIN
	SET NOCOUNT ON

	BEGIN TRY
		Begin Tran
			-- Getting required details
			Declare @pId			int			-- PersonId
			Declare @newEmail	nvarchar(50)	-- UnconfirmedEmailAddress

			Select	@pId = PersonId, @newEmail = UnconfirmedEmailAddress 
			From	StakeHolder
			Where	StakeHolderId = @StakeHolderId

			If @newEmail <> 'NULL'
			Begin

				Update	Person
				Set		Email = @newEmail
				Where	PersonId = @pId

				Update	StakeHolder
				Set		UnconfirmedEmailAddress = Null
				Where	StakeHolderId = @StakeHolderId
			End

		Commit Tran
    END TRY
	BEGIN CATCH
		If @@TranCount > 0
		Begin
			Rollback Tran
		End

		RAISERROR('StakeHolder Information could not be found.  Operation aborted.', 16, 2)
		EXEC RethrowError;
	END CATCH
	SET NOCOUNT OFF
END