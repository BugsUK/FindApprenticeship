CREATE PROCEDURE [dbo].[uspAdditionalAnswerSelectByAdditionalAnswerId]   
 @additionalAnswerId int  
AS  
BEGIN  
  
 SET NOCOUNT ON  
   
 SELECT  
 [additionalAnswer].[AdditionalAnswerId] AS 'AdditionalAnswerId',  
 [additionalAnswer].[AdditionalQuestionId] AS 'AdditionalQuestionId',  
 [additionalAnswer].[Answer] AS 'Answer',  
 [additionalAnswer].[ApplicationId] AS 'ApplicationId'  
 FROM [dbo].[AdditionalAnswer] [additionalAnswer]  
 WHERE [AdditionalAnswerId]=@additionalAnswerId  
  
 SET NOCOUNT OFF  
END