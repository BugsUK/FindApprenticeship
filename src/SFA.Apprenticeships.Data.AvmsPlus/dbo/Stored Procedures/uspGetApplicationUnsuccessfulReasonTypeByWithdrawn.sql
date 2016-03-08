CREATE PROCEDURE [dbo].[uspGetApplicationUnsuccessfulReasonTypeByWithdrawn]
@isWithdrawn BIT
AS
BEGIN  
 SET NOCOUNT ON  
	 SELECT ApplicationUnsuccessfulReasonTypeId, 
			CodeName,
			ShortName,
			FullName,
			ReferralPoints,
			CandidateDisplayText,
			CandidateFullName,
			Withdrawn
	 FROM	Applicationunsuccessfulreasontype  
	 WHERE  Withdrawn = @isWithdrawn
	 
    SET NOCOUNT OFF  
END