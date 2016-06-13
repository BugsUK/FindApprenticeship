CREATE PROCEDURE [dbo].[uspGetAllTermsAndConditions]

AS
SELECT   
  TermsAndConditionsId,   
  Fullname,   
  [Content] 
 FROM   
  TermsAndConditions   
RETURN 0;