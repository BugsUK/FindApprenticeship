CREATE PROCEDURE [dbo].[uspGetAllTermsAndConditions]

AS
SELECT   
  TermsAndConditionsId,   
  FullName,   
  [Content] 
 FROM   
  TermsAndConditions   
RETURN 0;