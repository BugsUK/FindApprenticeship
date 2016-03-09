CREATE PROCEDURE [dbo].[uspEShotJobGetStatus]
	@JobId INT
AS
	SET NOCOUNT ON

	SELECT j.EShotJobId, 
	   j.EShotJobStatusId,
	   COUNT(i.EShotJobItemId) as 'NumEmails',
	   COUNT(CASE s.CodeName WHEN 'SUC' THEN 1 ELSE NULL END ) as 'NumEmailsSucceeded',
	   COUNT(CASE s.CodeName WHEN 'NIS' THEN 1 WHEN 'NOP' THEN 1 ELSE NULL END ) as 'NumEmailsNA',
	   COUNT(CASE s.CodeName WHEN 'ERR' THEN 1 ELSE NULL END ) as 'NumEmailsErrored'
	FROM EShotJob j
		INNER JOIN EShotJobItem i on i.EShotJobId = j.EShotJobId
		INNER JOIN EShotJobItemStatusType s on s.EShotJobItemStatusTypeId = i.EShotJobItemStatusId
	WHERE j.EShotJobId = @JobId
	GROUP BY j.EShotJobId, j.EShotJobStatusId