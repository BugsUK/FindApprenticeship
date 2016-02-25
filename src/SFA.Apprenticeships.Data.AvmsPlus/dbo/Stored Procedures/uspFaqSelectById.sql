CREATE PROCEDURE [dbo].[uspFaqSelectById]
@faqId INT
AS
SELECT   
  FaqId,   
  Title,   
  [Content],
  UserTypeId,
  SortOrder 
 FROM   
  Faq   
 WHERE   
  FaqId = @faqId