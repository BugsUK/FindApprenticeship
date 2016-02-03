CREATE PROCEDURE [dbo].[uspFaqGetByUserType]  
@userTypeId INT  
AS  
SELECT   
  FaqId,   
  Title,   
  [Content],
  SortOrder 
 FROM   
  Faq   
 WHERE   
  UserTypeId = @userTypeId   
 ORDER BY   
  SortOrder  
RETURN 0;