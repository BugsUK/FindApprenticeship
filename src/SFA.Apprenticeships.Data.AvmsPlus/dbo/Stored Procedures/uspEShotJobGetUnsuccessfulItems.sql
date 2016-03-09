CREATE PROCEDURE [dbo].[uspEShotJobGetUnsuccessfulItems]
	@JobId INT
AS
	SET NOCOUNT ON

	SELECT i.EShotJobItemId, i.EShotJobItemStatusId, i.Email
	FROM EShotJobItem i
		INNER JOIN EShotJobItemStatusType s on s.EShotJobItemStatusTypeId = i.EShotJobItemStatusId
	WHERE EShotJobId = @JobId AND s.CodeName <> 'SUC'