CREATE PROCEDURE [dbo].[uspAdditionalQuestionSelectByVacancyId]    
	 @vacancyId int
AS
BEGIN
	SET NOCOUNT ON
	
	SELECT
		AdditionalQuestionId,
		QuestionId,
		Question
	FROM [dbo].[AdditionalQuestion]
	WHERE VacancyId = @vacancyId

	SET NOCOUNT OFF
END