CREATE PROCEDURE [dbo].[uspFaqGetByUserType]  
@userTypeId INT  
AS  
SELECT   
  FaqId,   
  Title,   
  [Content],
  SortOrder 
 FROM   
  FAQ   
 WHERE   
  UserTypeId = @userTypeId   
 ORDER BY   
  SortOrder  
RETURN 0;