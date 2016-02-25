CREATE PROCEDURE [dbo].[uspEShotJobUpdate]
	@JobId INT, 
	@JobStatus INT
AS
	SET NOCOUNT ON

	UPDATE EShotJob 
	SET EShotJobStatusId = @JobStatus, DateUpdated = GETDATE()
	WHERE EShotJobId = @JobId