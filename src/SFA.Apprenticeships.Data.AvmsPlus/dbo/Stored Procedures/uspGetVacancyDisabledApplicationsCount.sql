CREATE PROCEDURE [dbo].[uspGetVacancyDisabledApplicationsCount] 
	@vacancyId INT
AS
BEGIN
	SET NOCOUNT ON

DECLARE @StatusList varchar(50) = '2,3,4,5,6'

	SELECT
		COUNT(DISTINCT a.ApplicationId) as [Count]
	FROM
		[dbo].[Application] a
	INNER JOIN Candidate ON   
				a.[CandidateId] = [Candidate].[CandidateId]
	Inner Join (SELECT * from dbo.fnx_SplitListToTable(@StatusList)) as tmp on tmp.ID = a.ApplicationStatusTypeId
	WHERE
		a.VacancyId = @vacancyId
		AND [Candidate].Disability > 0
		AND [Candidate].Disability < 14

END