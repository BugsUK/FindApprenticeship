CREATE PROCEDURE [dbo].[uspAdditionalAnswerSelectByApplicationId]    
 @vacancyId int,  
 @applicationId int  
AS  
BEGIN  
 SET NOCOUNT ON  
   
 SELECT  
  [AdditionalQuestion].[VacancyId] AS 'VacancyId',  
  [additionalAnswer].[ApplicationId] AS 'ApplicationId',  
  [AdditionalQuestion].[AdditionalQuestionId] AS 'AdditionalQuestionId',  
  [AdditionalQuestion].[QuestionId] As 'QuestionId',  
  [AdditionalQuestion].[Question] As 'Question',  
  [additionalAnswer].[AdditionalanswerId] As 'AdditionalAnswerId',  
  [additionalAnswer].[Answer] As 'Answer'  
 FROM [dbo].[additionalQuestion]   
  INNER JOIN [additionalAnswer] ON [AdditionalQuestion].[AdditionalQuestionId] = [additionalAnswer].[AdditionalQuestionId] AND   
   [additionalAnswer].[ApplicationId] = @applicationId  
 WHERE [AdditionalQuestion].[VacancyId] = @vacancyId  
  
 SET NOCOUNT OFF  
END