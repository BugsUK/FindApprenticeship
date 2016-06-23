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
  FAQ   
 WHERE   
  FAQId = @faqId