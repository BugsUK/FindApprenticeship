CREATE PROCEDURE [dbo].[uspEShotJobItemUpdate]
	@JobItemId INT, 
	@JobItemStatus INT,
	@Error NVARCHAR(MAX)
AS
	SET NOCOUNT ON
	
	BEGIN TRANSACTION
	BEGIN TRY

		UPDATE EShotJobItem 
		SET EShotJobItemStatusId = @JobItemStatus, Error = @Error
		WHERE EShotJobItemId = @JobItemId

		UPDATE EShotJob 
		SET EShotJob.DateUpdated = GETDATE()
		FROM EShotJob INNER JOIN EShotJobItem  ON EShotJobItem.EShotJobId = EShotJob.EShotJobId
		WHERE EShotJobItem.EShotJobItemId = @JobItemId
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF (@@TRANCOUNT > 0)
			ROLLBACK TRAN;

		EXEC RethrowError;      
	END CATCH