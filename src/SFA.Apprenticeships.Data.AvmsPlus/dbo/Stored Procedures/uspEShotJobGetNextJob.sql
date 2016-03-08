CREATE PROCEDURE [dbo].[uspEShotJobGetNextJob]
	@JobId INT OUTPUT,
	@Subject NVARCHAR(400) OUTPUT,
	@Message NVARCHAR(MAX) OUTPUT
AS
	SET NOCOUNT ON

	DECLARE @WIPStatusId INT
	SELECT @WIPStatusId = EShotJobStatusTypeId FROM EShotJobStatusType WHERE CodeName = 'WIP'
	
	BEGIN TRANSACTION
	BEGIN TRY
		SET @JobId = NULL
		SELECT @JobId = EShotJobId
			FROM 
			(	
				SELECT TOP 1 EShotJobId
				FROM EShotJob j (TABLOCKX)
					INNER JOIN EShotJobStatusType s on s.EShotJobStatusTypeId = j.EShotJobStatusId
				WHERE 
					s.CodeName = 'PEN'
				ORDER BY 
					j.DateCreated ASC
			) a

		IF (@JobId IS NOT NULL)
		BEGIN 
			UPDATE EShotJob
			SET EShotJobStatusId = @WIPStatusId 
			WHERE EShotJobId = @JobId

			SELECT @Subject = [Subject], @Message = [Message]
			FROM EShotJob
			WHERE EShotJobId = @JobId

			SELECT EShotJobItemId, Email
			FROM EShotJobItem i
				INNER JOIN EShotJobItemStatusType s on s.EShotJobItemStatusTypeId = i.EShotJobItemStatusId
			WHERE EShotJobId = @JobId AND s.CodeName = 'PEN'
		END
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF (@@TRANCOUNT > 0)
			ROLLBACK TRAN;

		EXEC RethrowError;      
	END CATCH