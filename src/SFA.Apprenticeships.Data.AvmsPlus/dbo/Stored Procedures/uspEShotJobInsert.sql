CREATE PROCEDURE [dbo].[uspEShotJobInsert]
	@Subject NVARCHAR(400), 
	@Message NVARCHAR(MAX),
	@Emails NVARCHAR(MAX),
	@User NVARCHAR(400) 
AS
	DECLARE @JobId INT
	SET NOCOUNT ON

	BEGIN TRANSACTION
		BEGIN TRY
			DECLARE @PendingStatusId INT, @ItemPendingStatusId INT
			SELECT @PendingStatusId = EShotJobStatusTypeId FROM EShotJobStatusType WHERE CodeName = 'PEN'
			SELECT @ItemPendingStatusId = EShotJobItemStatusTypeId FROM EShotJobItemStatusType WHERE CodeName = 'PEN'
	
			INSERT EShotJob 
			(EShotJobStatusId, [Subject], [Message], DateCreated, DateUpdated, [User])
			VALUES
			(@PendingStatusId, @Subject, @Message, GETDATE(), GETDATE(), @User)
			SELECT @JobId = SCOPE_IDENTITY()
	
			INSERT INTO EShotJobItem (EShotJobId, EShotJobItemStatusId, Email, [Error])
			SELECT @JobId, @ItemPendingStatusId, e.ID, NULL 
			FROM fnx_SplitListToTable(@Emails) e

			EXEC uspEShotJobGetStatus @JobId
			COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF (@@TRANCOUNT > 0)
			ROLLBACK TRAN;

		EXEC RethrowError;      
	END CATCH