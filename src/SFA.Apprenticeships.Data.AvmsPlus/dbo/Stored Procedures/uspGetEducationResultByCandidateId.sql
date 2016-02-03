CREATE PROCEDURE [dbo].[uspGetEducationResultByCandidateId] 
	@candidateId int
AS
BEGIN

	SET NOCOUNT ON
	
	SELECT
	[educationResult].[CandidateId] AS 'CandidateId',
	[educationResult].[DateAchieved] AS 'DateAchieved',
	[educationResult].[EducationResultId] AS 'EducationResultId',
	--[educationResult].[FavoriteSubject] AS 'FavoriteSubject',
	[educationResult].[Grade] AS 'Grade',
	[educationResult].[Level] AS 'Level',
	[educationResult].[LevelOther] AS 'LevelOther',
	[educationResult].[Subject] AS 'Subject'
	FROM [dbo].[EducationResult] [educationResult]
	WHERE [CandidateId]=@candidateId

	SET NOCOUNT OFF
END