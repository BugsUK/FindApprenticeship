CREATE PROCEDURE [dbo].[uspAdditionalQuestionAnswerSelectByVacancyId]  
	@vacancyId int,
	@applicationId int
AS
BEGIN
	SET NOCOUNT ON
	
	SELECT
		[additionalQuestion].[VacancyId] AS 'VacancyId',
		[additionalAnswer].[ApplicationId] AS 'ApplicationId',
		[additionalQuestion].[AdditionalQuestionId] AS 'AdditionalQuestionId',
		[additionalQuestion].[QuestionId] As 'QuestionId',
		[additionalQuestion].[Question] As 'Question',
		[additionalAnswer].[AdditionalanswerId] As 'AdditionalAnswerId',
		[additionalAnswer].[Answer] As 'Answer'
	FROM [dbo].[additionalQuestion] 
		INNER JOIN [additionalAnswer] ON [additionalQuestion].[AdditionalQuestionId] = [additionalAnswer].[AdditionalQuestionId] AND 
			[additionalAnswer].[ApplicationId] = @applicationId
	WHERE [additionalQuestion].[VacancyId] = @vacancyId

	SET NOCOUNT OFF
END